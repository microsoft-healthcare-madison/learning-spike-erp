// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using generator_cli.Generators;
using generator_cli.Geographic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;

namespace generator_cli
{
    /// <summary>A program.</summary>
    public class Program
    {
        private static Dictionary<string, Organization> _orgById = new Dictionary<string, Organization>();
        private static Dictionary<string, Location> _rootLocationByOrgId = new Dictionary<string, Location>();
        private static Dictionary<string, OrgBeds> _bedsByOrgId = new Dictionary<string, OrgBeds>();

        private static bool _useLookup = false;
        private static bool _useJson = false;
        private static FhirJsonSerializer _jsonSerializer = null;
        private static FhirXmlSerializer _xmlSerializer = null;

        private static string _extension = ".json";

        /// <summary>Main entry-point for this application.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="outputDirectory">    Directory to write files.</param>
        /// <param name="outputFormat">       The output format, JSON or XML (default: JSON).</param>
        /// <param name="state">              State to restrict generation to (default: none).</param>
        /// <param name="postalCode">         Postal code to restrict generation to (default: none).</param>
        /// <param name="facilityCount">      Number of facilities to generate.</param>
        /// <param name="timeSteps">          Number of time-step updates to generate.</param>
        /// <param name="timePeriodHours">    Time-step period in hours (default: 24).</param>
        /// <param name="seed">               Starting seed to use in generation, 0 for none (default: 0).</param>
        /// <param name="orgSource">          Source for organization records: generate|csv (default: csv).</param>
        /// <param name="prettyPrint">        If output files should be formatted for display.</param>
        /// <param name="bedTypes">           Bar separated bed types: ICU|ER... (default: ICU|ER|HU).</param>
        /// <param name="operationalStatuses">Bar separated operational status: U|O|K (default: O|U).</param>
        /// <param name="minBedsPerOrg">      The minimum number of beds per hospital (default: 10).</param>
        /// <param name="maxBedsPerOrg">      The maximum number of beds per hospital (default: 1000).</param>
        public static void Main(
            string outputDirectory,
            string outputFormat = "JSON",
            string state = null,
            string postalCode = null,
            int facilityCount = 10,
            int timeSteps = 10,
            int timePeriodHours = 24,
            int seed = 0,
            string orgSource = "csv",
            bool prettyPrint = true,
            string bedTypes = "ICU|ER|HU",
            string operationalStatuses = "O|U",
            int minBedsPerOrg = 10,
            int maxBedsPerOrg = 1000)
        {
            // sanity checks
            if (string.IsNullOrEmpty(outputDirectory))
            {
                Console.WriteLine($"Invalid {nameof(outputDirectory)}: --output-directory is required");
            }

            if (string.IsNullOrEmpty(outputFormat))
            {
                throw new ArgumentNullException(nameof(outputFormat));
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            _useJson = outputFormat.ToUpperInvariant().Equals("JSON", StringComparison.Ordinal);

            if (_useJson)
            {
                SerializerSettings settings = new SerializerSettings()
                {
                    Pretty = prettyPrint,
                };

                _jsonSerializer = new FhirJsonSerializer(settings);
                _extension = ".json";
            }
            else
            {
                SerializerSettings settings = new SerializerSettings()
                {
                    Pretty = prettyPrint,
                };

                _xmlSerializer = new FhirXmlSerializer(settings);
                _extension = ".xml";
            }

            _useLookup = string.IsNullOrEmpty(orgSource) || (orgSource.ToUpperInvariant() != "GENERATE");

            // always need the geo manager
            GeoManager.Init(seed);
            OrgBeds.Init(seed);

            // only need hospital manager if we are using lookup (avoid loading otherwise)
            if (_useLookup)
            {
                HospitalManager.Init(seed, minBedsPerOrg, maxBedsPerOrg);
            }

            // create our organization records
            CreateOrgs(facilityCount, state, postalCode);
            CreateOrgBeds(string.Empty, bedTypes, operationalStatuses, string.Empty);

            // write our org bundles in t0
            WriteOrgBundles(Path.Combine(outputDirectory, "t0"));

            WriteBedBundles(Path.Combine(outputDirectory, "t0"));
        }

        /// <summary>Creates organization beds.</summary>
        /// <param name="availabilities">     The availabilities.</param>
        /// <param name="bedTypes">           Bar separated bed types: ICU|ER... (default: ICU|ER|HU).</param>
        /// <param name="operationalStatuses">Bar separated operational status: U|O|K (default: O|U).</param>
        /// <param name="features">           The features.</param>
        private static void CreateOrgBeds(
            string availabilities,
            string bedTypes,
            string operationalStatuses,
            string features)
        {
            List<BedConfiguration> bedConfigurations = BedConfiguration.StatesForParams(
                availabilities,
                bedTypes,
                operationalStatuses,
                features);

            foreach (string orgId in _orgById.Keys)
            {
                int initialBedCount;

                if (_useLookup)
                {
                    initialBedCount = HospitalManager.BedsForHospital(orgId);
                }
                else
                {
                    initialBedCount = GeoManager.BedsForHospital();
                }

                _bedsByOrgId.Add(orgId, new OrgBeds(
                    _orgById[orgId],
                    _rootLocationByOrgId[orgId],
                    initialBedCount,
                    bedConfigurations));
            }
        }

        /// <summary>Writes a bed bundles.</summary>
        /// <param name="dir">The dir.</param>
        private static void WriteBedBundles(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            foreach (string orgId in _orgById.Keys)
            {
                WriteBedBundle(orgId, dir);
            }
        }

        /// <summary>Writes a bed bundle.</summary>
        /// <param name="orgId">The organization.</param>
        /// <param name="dir">  The dir.</param>
        private static void WriteBedBundle(
            string orgId,
            string dir)
        {
            string filename = Path.Combine(dir, $"{orgId}-beds{_extension}");

            string bundleId = FhirGenerator.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirGenerator.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            foreach (Location bed in _bedsByOrgId[orgId].Beds())
            {
                bundle.AddResourceEntry(
                    bed,
                    $"{FhirGenerator.InternalSystem}{bed.ResourceType}/{bed.Id}");
            }

            if (_useJson)
            {
                File.WriteAllText(filename, _jsonSerializer.SerializeToString(bundle));
            }
            else
            {
                File.WriteAllText(filename, _xmlSerializer.SerializeToString(bundle));
            }
        }

        /// <summary>Writes an organisation bundles.</summary>
        /// <param name="dir">The dir.</param>
        private static void WriteOrgBundles(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            foreach (string orgId in _orgById.Keys)
            {
                WriteOrgBundle(orgId, dir);
            }
        }

        /// <summary>Writes an organization bundle.</summary>
        /// <param name="orgId">The organization.</param>
        /// <param name="dir">  The dir.</param>
        private static void WriteOrgBundle(
            string orgId,
            string dir)
        {
            string filename = Path.Combine(dir, $"{orgId}{_extension}");

            string bundleId = FhirGenerator.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirGenerator.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                _orgById[orgId],
                $"{FhirGenerator.InternalSystem}{_orgById[orgId].ResourceType}/{orgId}");

            bundle.AddResourceEntry(
                _rootLocationByOrgId[orgId],
                $"{FhirGenerator.InternalSystem}{_rootLocationByOrgId[orgId].ResourceType}/{_rootLocationByOrgId[orgId].Id}");

            if (_useJson)
            {
                File.WriteAllText(filename, _jsonSerializer.SerializeToString(bundle));
            }
            else
            {
                File.WriteAllText(filename, _xmlSerializer.SerializeToString(bundle));
            }
        }

        /// <summary>Creates the orgs.</summary>
        /// <param name="count">     Number of.</param>
        /// <param name="state">     State to restrict generation to (default: none).</param>
        /// <param name="postalCode">Postal code to restrict generation to (default: none).</param>
        private static void CreateOrgs(
            int count,
            string state,
            string postalCode)
        {
            for (int facilityNumber = 0; facilityNumber < count; facilityNumber++)
            {
                Organization org = _useLookup
                    ? HospitalManager.GetOrganization(state, postalCode)
                    : GeoManager.GetOrganization(state, postalCode);

                if (_orgById.ContainsKey(org.Id))
                {
                    // ignore for now - need to figure out what to do for counts later
                    continue;
                }

                _orgById.Add(org.Id, org);

                Location orgLoc = FhirGenerator.RootLocationForOrg(org);
                _rootLocationByOrgId.Add(org.Id, orgLoc);
            }
        }
    }
}
