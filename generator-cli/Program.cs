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
        /// <summary>Main entry-point for this application.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="outputDirectory">Directory to write files.</param>
        /// <param name="outputFormat">   The output format, JSON or XML (default: JSON).</param>
        /// <param name="state">          State to restrict generation to (default: none).</param>
        /// <param name="postalCode">     Postal code to restrict generation to (default: none).</param>
        /// <param name="facilityCount">  Number of facilities to generate.</param>
        /// <param name="timeSteps">      Number of time-step updates to generate.</param>
        /// <param name="seed">           Starting seed to use in generation, 0 for none (default: 0).</param>
        /// <param name="bundleType">     Type of bundle to generate: batch|transaction (default: batch).</param>
        public static void Main(
            string outputDirectory,
            string outputFormat = "JSON",
            string state = null,
            string postalCode = null,
            int facilityCount = 10,
            int timeSteps = 10,
            int seed = 0,
            string bundleType = "batch")
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

            GeoManager.Init(seed);
            HospitalManager.Init(seed);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            FhirJsonSerializer jsonSerializer = new FhirJsonSerializer();
            FhirXmlSerializer xmlSerializer = new FhirXmlSerializer();

            bool useJson = outputFormat.ToUpperInvariant().Equals("JSON", StringComparison.Ordinal);

            Dictionary<string, Organization> orgById = new Dictionary<string, Organization>();
            Dictionary<string, Location> rootLocationByOrgId = new Dictionary<string, Location>();

            string currentDir = Path.Combine(outputDirectory, "t0");
            string filename = string.Empty;

            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }

            bool useBatch = string.IsNullOrEmpty(bundleType) || (bundleType.ToUpperInvariant() != "TRANSACTION");

            // generate our initial data set
            for (int facilityNumber = 0; facilityNumber < facilityCount; facilityNumber++)
            {
                string bundleId = FhirGenerator.NextId;

                Bundle bundle = new Bundle()
                {
                    Id = bundleId,
                    Identifier = FhirGenerator.IdentifierForId(bundleId),
                    Type = useBatch ? Bundle.BundleType.Batch : Bundle.BundleType.Transaction,
                    Timestamp = new DateTimeOffset(DateTime.Now),
                    Meta = new Meta(),
                };

                bundle.Entry = new List<Bundle.EntryComponent>();

                Organization org = HospitalManager.GetOrganization(state, postalCode);
                orgById.Add(org.Id, org);
                bundle.AddResourceEntry(org, $"{FhirGenerator.InternalSystem}/{org.Id}");

                Location orgLoc = FhirGenerator.RootLocationForOrg(org);
                rootLocationByOrgId.Add(org.Id, orgLoc);
                bundle.AddResourceEntry(orgLoc, $"{FhirGenerator.InternalSystem}/{orgLoc.Id}");

                if (useJson)
                {
                    filename = Path.Combine(currentDir, $"{org.Id}.json");
                    File.WriteAllText(filename, jsonSerializer.SerializeToString(bundle));
                }
                else
                {
                    filename = Path.Combine(currentDir, $"{org.Id}.xml");
                    File.WriteAllText(filename, xmlSerializer.SerializeToString(bundle));
                }
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    Address address = GeoManager.GetAnyAddress();
            //    Console.WriteLine($"Address: {address.City}, {address.State} {address.PostalCode}");
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    Organization org = HospitalManager.GetAnyOrganization();
            //    Console.WriteLine($"Org: {org.Name}");
            //}
        }
    }
}
