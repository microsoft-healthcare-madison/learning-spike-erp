// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using generator_cli.Generators;
using generator_cli.Geographic;
using generator_cli.Models;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;

namespace generator_cli
{
    /// <summary>A program.</summary>
    public static class Program
    {
        private const string _filenameBaseForMeasures = "Measures";

        private const string _filenameAdditionForBeds = "-beds";
        private const string _filenameAdditionForGroups = "-groups";
        private const string _filenameAdditionForMeasureReports = "-measureReports";

        private static Dictionary<string, Organization> _orgById = new Dictionary<string, Organization>();
        private static Dictionary<string, Location> _rootLocationByOrgId = new Dictionary<string, Location>();
        private static Dictionary<string, OrgBeds> _bedsByOrgId = new Dictionary<string, OrgBeds>();

        private static Dictionary<string, OrgDeviceData> _deviceDataByOrgId = new Dictionary<string, OrgDeviceData>();
        private static Dictionary<string, OrgPatientData> _patientDataByOrgId = new Dictionary<string, OrgPatientData>();
        private static Dictionary<string, OrgTestData> _testDataByOrgId = new Dictionary<string, OrgTestData>();

        private static List<BedConfiguration> _bedConfigurations = null;

        private static Random _rand = null;
        private static bool _useLookup = false;
        private static bool _useJson = false;
        private static double _changeFactor = 0;
        private static double _minIcuPercent = 0;
        private static double _maxIcuPercent = 0;
        private static double _ventilatorsPerIcu = 0;
        private static double _initialOccupancy = 0;
        private static double _positiveTestRate = 0;
        private static double _hospitalizationRate = 0;
        private static double _patientToIcuRate = 0;
        private static double _icuToVentilatorRate = 0;
        private static double _recoveryRate = 0;
        private static double _deathRate = 0;
        private static double _noResourceRecoveryRate = 0;
        private static double _noResourceDeathRate = 0;

        private static FhirJsonSerializer _jsonSerializer = null;
        private static FhirXmlSerializer _xmlSerializer = null;

        private static string _extension = ".json";

        /// <summary>Main entry-point for this application.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="outputDirectory">     Directory to write files.</param>
        /// <param name="dataDirectory">       Allow passing in of data directory.</param>
        /// <param name="outputFormat">        The output format, JSON or XML (default: JSON).</param>
        /// <param name="state">               State to restrict generation to (default: none).</param>
        /// <param name="postalCode">          Postal code to restrict generation to (default: none).</param>
        /// <param name="facilityCount">       Number of facilities to generate.</param>
        /// <param name="timeSteps">           Number of time-step updates to generate.</param>
        /// <param name="timePeriodHours">     Time-step period in hours (default: 24).</param>
        /// <param name="seed">                Starting seed to use in generation, 0 for none (default: 0).</param>
        /// <param name="recordsToSkip">       Number of records to skip before starting generation (default: 0).</param>
        /// <param name="orgSource">           Source for organization records: generate|csv (default: csv).</param>
        /// <param name="prettyPrint">         If output files should be formatted for display.</param>
        /// <param name="bedTypes">            Bar separated bed types: ICU|ER... (default: ICU|ER|HU).</param>
        /// <param name="operationalStatuses"> Bar separated operational status: U|O|K (default: O|U).</param>
        /// <param name="minBedsPerOrg">       The minimum number of beds per hospital (default: 10).</param>
        /// <param name="maxBedsPerOrg">       The maximum number of beds per hospital (default: 1000).</param>
        /// <param name="exportBeds">          If bundles of individual beds should be exported (default: false).</param>
        /// <param name="exportGroups">        If SANER-IG groups should be generated (deprecated - default: false).</param>
        /// <param name="changeFactor">        The amount of change in bed state per step (default 0.2).</param>
        /// <param name="minIcuPercent">       Minimum percentage of beds for an org which are ICU type.</param>
        /// <param name="maxIcuPercent">       Maximum percentage of beds for an org which are ICU type.</param>
        /// <param name="ventilatorsPerIcu">   Average number of ventilators per ICU bed.</param>
        /// <param name="initialOccupancy">    Initial occupancy of bed percentage.</param>
        /// <param name="positiveTestRate">    Rate of people being tested returning positive.</param>
        /// <param name="hospitalizationRate"> Rate of people testing positive requiring hospitalization.</param>
        /// <param name="patientToIcuRate">    Rate of people hospitalized requiring ICU.</param>
        /// <param name="icuToVentilatorRate"> Rate of people in ICU requiring ventilators.</param>
        /// <param name="recoveryRate">        Rate of people recovering during hospitalzation.</param>
        /// <param name="deathRate">           Rate of people dying in hospitalization, when care is available.</param>
        public static void Main(
            string outputDirectory,
            string dataDirectory = null,
            string outputFormat = "JSON",
            string state = null,
            string postalCode = null,
            int facilityCount = 10,
            int timeSteps = 2,
            int timePeriodHours = 24,
            int seed = 0,
            int recordsToSkip = 0,
            string orgSource = "csv",
            bool prettyPrint = true,
            string bedTypes = "ICU|ER|HU",
            string operationalStatuses = "O|U",
            int minBedsPerOrg = 10,
            int maxBedsPerOrg = 1000,
            bool exportBeds = false,
            bool exportGroups = false,
            double changeFactor = 0.2,
            double minIcuPercent = 0.05,
            double maxIcuPercent = 0.20,
            double ventilatorsPerIcu = 0.20,
            double initialOccupancy = 0.20,
            double positiveTestRate = 0.5,
            double hospitalizationRate = 0.30,
            double patientToIcuRate = 0.30,
            double icuToVentilatorRate = 0.70,
            double recoveryRate = 0.1,
            double deathRate = 0.05)
        {
            // sanity checks
            if (string.IsNullOrEmpty(outputDirectory))
            {
                throw new ArgumentNullException(nameof(outputDirectory));
            }

            if (string.IsNullOrEmpty(outputFormat))
            {
                throw new ArgumentNullException(nameof(outputFormat));
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            _changeFactor = changeFactor;
            _minIcuPercent = minIcuPercent;
            _maxIcuPercent = maxIcuPercent;
            _ventilatorsPerIcu = ventilatorsPerIcu;
            _initialOccupancy = initialOccupancy;
            _positiveTestRate = positiveTestRate;
            _hospitalizationRate = hospitalizationRate;
            _patientToIcuRate = patientToIcuRate;
            _icuToVentilatorRate = icuToVentilatorRate;
            _recoveryRate = recoveryRate;
            _noResourceRecoveryRate = recoveryRate * 0.1;
            _deathRate = deathRate;
            _noResourceDeathRate = Math.Min(deathRate * 10, 1.0);

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

            if (seed == 0)
            {
                _rand = new Random();
            }
            else
            {
                _rand = new Random(seed);
            }

            // always need the geo manager
            GeoManager.Init(seed, minBedsPerOrg, maxBedsPerOrg, dataDirectory);

            _bedConfigurations = BedConfiguration.StatesForParams(
                string.Empty,
                bedTypes,
                operationalStatuses,
                string.Empty);

            OrgBeds.Init(seed, _bedConfigurations);

            // only need hospital manager if we are using lookup (avoid loading otherwise)
            if (_useLookup)
            {
                HospitalManager.Init(seed, minBedsPerOrg, maxBedsPerOrg, dataDirectory);
            }

            MeasureGenerator.Init();

            string dir;

            // create our time step directories
            for (int step = 0; step < timeSteps; step++)
            {
                dir = Path.Combine(outputDirectory, $"t{step}");

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            // create our organization records
            CreateOrgs(facilityCount, state, postalCode, recordsToSkip);

            if (exportBeds)
            {
                ExportByBeds(
                    outputDirectory,
                    timeSteps,
                    timePeriodHours,
                    exportBeds,
                    exportGroups);
            }

            ExportAggregate(outputDirectory, timeSteps, timePeriodHours);
        }

        /// <summary>Export aggregate.</summary>
        /// <param name="outputDirectory">Directory to write files.</param>
        /// <param name="timeSteps">      Number of time-step updates to generate.</param>
        /// <param name="timePeriodHours">Time-step period in hours (default: 24).</param>
        private static void ExportAggregate(
            string outputDirectory,
            int timeSteps,
            int timePeriodHours)
        {
            // write measures only in t0
            WriteBundle(
                Path.Combine(outputDirectory, "t0", $"{_filenameBaseForMeasures}{_extension}"),
                MeasureGenerator.GetMeasureBundle());

            WriteGroupedMeasureBundles(Path.Combine(outputDirectory, "t0"));

            // iterate over the orgs generating their data
            foreach (string orgId in _orgById.Keys)
            {
                Console.WriteLine($"Processing org: {orgId}");

                CreateAggregateData(orgId);

                // loop over timeSteps
                for (int step = 0; step < timeSteps; step++)
                {
                    string dir = Path.Combine(outputDirectory, $"t{step}");

                    if (step == 0)
                    {
                        WriteOrgBundle(orgId, dir);
                    }

                    if (step != 0)
                    {
                        UpdateAggregateDataForStep(orgId);
                    }

                    TimeSpan hoursToSubtract = new TimeSpan(timePeriodHours * (timeSteps - step), 0, 0);
                    DateTime dt = DateTime.UtcNow.Subtract(hoursToSubtract);
                    FhirDateTime dateTime = new FhirDateTime(dt.Year, dt.Month, dt.Day);
                    Period period = new Period(dateTime, dateTime);

                    WriteOrgReportBundle(orgId, dir, period);
                }
            }
        }

        /// <summary>Writes a grouped measure bundles.</summary>
        /// <param name="dir">The dir.</param>
        private static void WriteGroupedMeasureBundles(string dir)
        {
            WriteBundle(
                Path.Combine(dir, $"{_filenameBaseForMeasures}-beds{_extension}"),
                MeasureGenerator.GetGroupedMeasureBundle("beds"));
        }

        /// <summary>Writes an organisation reports.</summary>
        /// <param name="orgId"> The organization.</param>
        /// <param name="dir">   The dir.</param>
        /// <param name="period">The period.</param>
        private static void WriteOrgReportBundle(
            string orgId,
            string dir,
            Period period)
        {
            string filename = Path.Combine(dir, $"{orgId}{_filenameAdditionForMeasureReports}{_extension}");

            MeasureReportGenerator reportGen = new MeasureReportGenerator(
                _orgById[orgId],
                _rootLocationByOrgId[orgId],
                _deviceDataByOrgId[orgId],
                _patientDataByOrgId[orgId],
                _testDataByOrgId[orgId],
                period);

            WriteBundle(filename, reportGen.GetReportBundle());

            WriteBundle(
                Path.Combine(dir, $"{orgId}-measureReportsGrouped{_extension}"),
                reportGen.GetGroupReportBundle());
        }

        /// <summary>Updates the aggregate data for step.</summary>
        private static void UpdateAggregateDataForStep(string orgId)
        {
            OrgDeviceData device = _deviceDataByOrgId[orgId];
            OrgPatientData patient = _patientDataByOrgId[orgId];
            OrgTestData test = _testDataByOrgId[orgId];

            // increase testing
            int testDelta = (int)(test.Performed * _changeFactor);
            if (testDelta == 0)
            {
                testDelta = 1;
            }

            int testPositiveDelta = (int)(testDelta * _positiveTestRate);
            int testNegativeDelta = testDelta - testPositiveDelta;
            int testPending = _rand.Next(0, 10);

            int patientsShortCare =
                Math.Max(0, patient.PositiveNeedVent - device.Ventilators) +
                Math.Max(0, patient.PositiveNeedIcu - device.ICU) +
                Math.Max(0, patient.PositiveNonIcu - device.Inpatient);

            int patientsWithCare = patient.Positive - patientsShortCare;

            double effectiveRecoveryRate = (patient.Positive == 0)
                ? 0
                : ((_recoveryRate * patientsWithCare) + (_noResourceRecoveryRate * patientsShortCare)) /
                  ((double)patientsWithCare + (double)patientsShortCare);

            double effectiveDeathRate = (patient.Positive == 0)
                ? 0
                : ((_deathRate * patientsWithCare) + (_noResourceDeathRate * patientsShortCare)) /
                  ((double)patientsWithCare + (double)patientsShortCare);

            double patientRemovalRate = effectiveDeathRate + effectiveRecoveryRate;

            int patientsRemoved = (int)(patient.Positive * patientRemovalRate);
            int patientsDied;
            int patientsRecovered;
            if (_rand.NextDouble() < 0.5)
            {
                patientsDied = (int)((double)patient.Positive * effectiveDeathRate);
                patientsRecovered = patientsRemoved - patientsDied;
            }
            else
            {
                patientsRecovered = (int)((double)patient.Positive * effectiveRecoveryRate);
                patientsDied = patientsRemoved - patientsRecovered;
            }

            int positive = patient.Positive
                - patientsRemoved
                + Math.Max((int)(testPositiveDelta * _hospitalizationRate), 1);
            int patients = patient.Total - patient.Positive + positive;
            int positiveNeedIcu = (int)(positive * _patientToIcuRate);
            int positiveNeedVent = (int)(positive * _icuToVentilatorRate);
            int negative = patients - positive;
            int negativeNeedIcu = 0; // (int)(negative * _patientToIcuRate);
            int negativeNeedVent = 0; // (int)(negativeNeedIcu * _icuToVentilatorRate);

            int dead = patient.Died + patientsDied;
            int recovered = patient.Recovered + patientsRecovered;

            // update records
            test.Update(
                testDelta,
                testPositiveDelta,
                testNegativeDelta,
                testPending);

            patient.Update(
                patients,
                positive,
                positiveNeedIcu,
                positiveNeedVent,
                negative,
                negativeNeedIcu,
                negativeNeedVent,
                0,
                recovered,
                dead);

            // Console.WriteLine($" - Tests: {test.Performed}, {test.Positive}, {test.Negative}, {test.Pending}");
            // Console.WriteLine($" - Patients: {patient.Total}, {patient.Positive}, {patient.Negative}, {patient.Recovered}, {patient.Died}");
        }

        /// <summary>Creates aggregate data.</summary>
        private static void CreateAggregateData(string orgId)
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

            double icuRate = (_rand.NextDouble() * (_maxIcuPercent - _minIcuPercent)) + _minIcuPercent;
            int icuBeds = (int)(initialBedCount * icuRate);
            if (icuBeds < 1)
            {
                icuBeds = 1;
            }

            int inpatientBeds = initialBedCount - icuBeds;

            int ventilators = (int)(icuBeds * _ventilatorsPerIcu);
            if (ventilators < 1)
            {
                ventilators = 1;
            }

            // create device data for this org
            OrgDeviceData device = new OrgDeviceData(
                initialBedCount,
                inpatientBeds,
                icuBeds,
                ventilators);

            // figure out patient numbers based on initial capacity
            int patients = (int)(initialBedCount * _initialOccupancy);
            int positive = (int)(patients * _positiveTestRate);
            int positiveNeedIcu = (int)(positive * _patientToIcuRate);
            int positiveNeedVent = (int)(positive * _icuToVentilatorRate);
            int negative = patients - positive;
            int negativeNeedIcu = 0; // (int)(negative * _patientToIcuRate);
            int negativeNeedVent = 0; // (int)(negativeNeedIcu * _icuToVentilatorRate);
            int onsetInCare = 0;
            int recovered = 0;
            int dead = 0;

            // create patient data
            OrgPatientData patient = new OrgPatientData(
                patients,
                positive,
                positiveNeedIcu,
                positiveNeedVent,
                negative,
                negativeNeedIcu,
                negativeNeedVent,
                onsetInCare,
                recovered,
                dead);

            // extrapolate test data
            int performedTests = (int)(patients / _hospitalizationRate);
            int positiveTests = (int)(performedTests * _positiveTestRate);
            int negativeTests = performedTests - positive;
            int pendingTests = _rand.Next(0, 10);
            performedTests += pendingTests;

            // create test data record
            OrgTestData test = new OrgTestData(
                performedTests,
                positiveTests,
                negativeTests,
                pendingTests);

            // add records
            _deviceDataByOrgId.Add(orgId, device);
            _patientDataByOrgId.Add(orgId, patient);
            _testDataByOrgId.Add(orgId, test);

            // Console.WriteLine($" - Tests: {test.Performed}, {test.Positive}, {test.Negative}, {test.Pending}");
            // Console.WriteLine($" - Patients: {patient.Total}, {patient.Positive}, {patient.Negative}, {patient.Recovered}, {patient.Died}");
        }

        /// <summary>Export by beds.</summary>
        /// <param name="outputDirectory">Directory to write files.</param>
        /// <param name="timeSteps">      Number of time-step updates to generate.</param>
        /// <param name="timePeriodHours">Time-step period in hours (default: 24).</param>
        /// <param name="exportBeds">     If bundles of individual beds should be exported (default: false).</param>
        /// <param name="exportGroups">   If SANER-IG groups should be generated (deprecated - default:
        ///  false).</param>
        private static void ExportByBeds(
            string outputDirectory,
            int timeSteps,
            int timePeriodHours,
            bool exportBeds,
            bool exportGroups)
        {
            // iterate over the orgs generating their data
            foreach (string orgId in _orgById.Keys)
            {
                Console.WriteLine($"Processing org: {orgId}");

                CreateOrgBeds(orgId);

                // loop over timeSteps
                for (int step = 0; step < timeSteps; step++)
                {
                    string dir = Path.Combine(outputDirectory, $"t{step}");

                    if (step != 0)
                    {
                        _bedsByOrgId[orgId].UpdateBedStatusForTimeStep(_changeFactor);
                    }

                    TimeSpan hoursToSubtract = new TimeSpan(timePeriodHours * (timeSteps - step), 0, 0);
                    DateTime dt = DateTime.UtcNow.Subtract(hoursToSubtract);
                    FhirDateTime dateTime = new FhirDateTime(dt.Year, dt.Month, dt.Day);
                    Period period = new Period(dateTime, dateTime);

                    WriteOrgBundle(orgId, dir);

                    if (exportBeds)
                    {
                        WriteBedBundle(orgId, dir);
                    }

                    WriteMeasureReportBundle(orgId, dir, period);

                    if (exportGroups)
                    {
                        WriteGroupBundle(orgId, dir, period);
                    }
                }

                DeleteOrgBeds(orgId);
            }
        }

        /// <summary>Writes a measure report bundle.</summary>
        /// <param name="orgId"> The organization.</param>
        /// <param name="dir">   The dir.</param>
        /// <param name="period">The period.</param>
        private static void WriteMeasureReportBundle(
            string orgId,
            string dir,
            Period period)
        {
            string filename = Path.Combine(dir, $"{orgId}{_filenameAdditionForMeasureReports}{_extension}");

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

            MeasureReport report = FhirGenerator.GenerateBedMeasureReportV01(
                _orgById[orgId],
                _rootLocationByOrgId[orgId],
                period,
                _bedsByOrgId[orgId].BedsByConfig);

            bundle.AddResourceEntry(
                report,
                $"{SystemLiterals.Internal}{report.ResourceType}/{report.Id}");

            WriteBundle(filename, bundle);
        }

        /// <summary>Writes a group bundle.</summary>
        /// <param name="orgId"> The organization.</param>
        /// <param name="dir">   The dir.</param>
        /// <param name="period">The period.</param>
        private static void WriteGroupBundle(
            string orgId,
            string dir,
            Period period)
        {
            string filename = Path.Combine(dir, $"{orgId}{_filenameAdditionForGroups}{_extension}");

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

            foreach (BedConfiguration config in _bedConfigurations)
            {
                int bedCount = _bedsByOrgId[orgId].BedCount(config);

                // check for no beds of this type
                if (bedCount == 0)
                {
                    continue;
                }

                Group group = FhirGenerator.GenerateGroup(
                    _orgById[orgId],
                    _rootLocationByOrgId[orgId],
                    $"{config.ToString()}",
                    config,
                    bedCount,
                    period);

                bundle.AddResourceEntry(
                    group,
                    $"{SystemLiterals.Internal}{group.ResourceType}/{group.Id}");
            }

            WriteBundle(filename, bundle);
        }

        /// <summary>Creates organization bed.</summary>
        /// <param name="orgId">The organization.</param>
        private static void CreateOrgBeds(string orgId)
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
                initialBedCount));
        }

        /// <summary>Deletes the organization bed described by orgId.</summary>
        /// <param name="orgId">The organization.</param>
        private static void DeleteOrgBeds(string orgId)
        {
            _bedsByOrgId.Remove(orgId);
        }

        /// <summary>Writes a bed bundle.</summary>
        /// <param name="orgId">The organization.</param>
        /// <param name="dir">  The dir.</param>
        private static void WriteBedBundle(
            string orgId,
            string dir)
        {
            string filename = Path.Combine(dir, $"{orgId}{_filenameAdditionForBeds}{_extension}");

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
                    $"{SystemLiterals.Internal}{bed.ResourceType}/{bed.Id}");
            }

            WriteBundle(filename, bundle);
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
                $"{SystemLiterals.Internal}{_orgById[orgId].ResourceType}/{orgId}");

            bundle.AddResourceEntry(
                _rootLocationByOrgId[orgId],
                $"{SystemLiterals.Internal}{_rootLocationByOrgId[orgId].ResourceType}/{_rootLocationByOrgId[orgId].Id}");

            WriteBundle(filename, bundle);
        }

        /// <summary>Creates the orgs.</summary>
        /// <param name="count">        Number of.</param>
        /// <param name="state">        State to restrict generation to (default: none).</param>
        /// <param name="postalCode">   Postal code to restrict generation to (default: none).</param>
        /// <param name="recordsToSkip">(Optional) Number of records to skip before starting generation
        ///  (default: 0).</param>
        private static void CreateOrgs(
            int count,
            string state,
            string postalCode,
            int recordsToSkip = 0)
        {
            List<Organization> orgs = _useLookup
                ? HospitalManager.GetOrganizations(count, state, postalCode, recordsToSkip)
                : GeoManager.GetOrganizations(count, state, postalCode);

            foreach (Organization org in orgs)
            {
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

        /// <summary>Writes a bundle.</summary>
        /// <param name="filename">Filename of the file.</param>
        /// <param name="bundle">  The bundle.</param>
        private static void WriteBundle(string filename, Bundle bundle)
        {
            if (_useJson)
            {
                File.WriteAllText(
                    filename,
                    _jsonSerializer.SerializeToString(bundle));
            }
            else
            {
                File.WriteAllText(
                    filename,
                    _xmlSerializer.SerializeToString(bundle));
            }
        }
    }
}
