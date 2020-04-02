// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using measureReportTransformer.CDC;
using measureReportTransformer.Plotting;
using Newtonsoft.Json;

namespace measureReportTransformer
{
    /// <summary>A program.</summary>
    public static class Program
    {
        /// <summary>The JSON parser.</summary>
        private static FhirJsonParser _fhirJsonParser = new FhirJsonParser();

        private static Dictionary<string, Organization> _orgsByRef = new Dictionary<string, Organization>();
        private static Dictionary<string, Location> _locationsByRef = new Dictionary<string, Location>();
        private static Dictionary<string, Measure> _measuresByUrl = new Dictionary<string, Measure>();
        private static Dictionary<string, ReportCollection> _reportsByOrgRef = new Dictionary<string, ReportCollection>();

        /// <summary>Main entry-point for this application.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="inputDirectory">   Source Bundle folder.</param>
        /// <param name="outputDirectory">  Location to write formatted data.</param>
        /// <param name="templateDirectory">Location of template files.</param>
        /// <param name="exportCdc">        True to export CDC CSV files.</param>
        /// <param name="exportPlotting">   True to export CSV Plotting files (experimental).</param>
        public static void Main(
            string inputDirectory,
            string outputDirectory,
            string templateDirectory = null,
            bool exportCdc = true,
            bool exportPlotting = true)
        {
            if (string.IsNullOrEmpty(inputDirectory))
            {
                throw new ArgumentNullException(nameof(inputDirectory));
            }

            if (string.IsNullOrEmpty(outputDirectory))
            {
                throw new ArgumentNullException(nameof(outputDirectory));
            }

            LoadFromDirectory(inputDirectory);

            foreach (string orgRef in _orgsByRef.Keys)
            {
                if (exportCdc)
                {
                    WriteCdcCsv(orgRef, outputDirectory);
                }
            }

            if (exportPlotting)
            {
                WritePlottingCsv(outputDirectory);
            }
        }

        /// <summary>Writes a fema sheets.</summary>
        /// <exception cref="FileNotFoundException">Thrown when the requested file is not present.</exception>
        /// <param name="orgRef">           Identifier for the organization.</param>
        /// <param name="dir">              The dir.</param>
        /// <param name="templateDirectory">Pathname of the data directory.</param>
        public static void WriteFemaSheets(
            string orgRef,
            string dir,
            string templateDirectory)
        {
            string templateFilename = string.IsNullOrEmpty(templateDirectory)
                ? Path.Combine(Directory.GetCurrentDirectory(), "data", "FEMA_Template.xlsx")
                : Path.Combine(templateDirectory, "FEMA_Template.xlsx");

            if (!File.Exists(templateFilename))
            {
                throw new FileNotFoundException($"Could not find: {templateFilename}");
            }



        }

        /// <summary>Location data for organization.</summary>
        /// <param name="orgRef">   Identifier for the organization.</param>
        /// <param name="latitude"> [out] The latitude.</param>
        /// <param name="longitude">[out] The longitude.</param>
        public static void LocationDataForOrg(
            string orgRef,
            out double latitude,
            out double longitude)
        {
            string locRef = $"Location/Loc-{_orgsByRef[orgRef].Id}";

            if (_locationsByRef.ContainsKey(locRef) &&
                (_locationsByRef[locRef].Position != null) &&
                (_locationsByRef[locRef].Position.Latitude != null) &&
                (_locationsByRef[locRef].Position.Longitude != null))
            {
                latitude = (double)_locationsByRef[locRef].Position.Latitude;
                longitude = (double)_locationsByRef[locRef].Position.Longitude;
                return;
            }

            latitude = 0;
            longitude = 0;
        }

        /// <summary>Writes a CDC CSV.</summary>
        /// <param name="orgRef">Identifier for the organization.</param>
        /// <param name="dir">  The dir.</param>
        private static void WriteCdcCsv(
            string orgRef,
            string dir)
        {
            if (!_reportsByOrgRef.ContainsKey(orgRef))
            {
                return;
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            List<CdcModel> cdcData = _reportsByOrgRef[orgRef].CdcData;

            string orgId = _orgsByRef[orgRef].Id;

            string filename = Path.Combine(dir, $"{orgId}-cdc.csv");

            using (StreamWriter writer = new StreamWriter(filename))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(cdcData);
            }
        }

        /// <summary>Writes a plotting JSON.</summary>
        /// <param name="dir">The dir.</param>
        private static void WritePlottingCsv(
            string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            List<PlottingModel> data = new List<PlottingModel>();

            foreach (string orgRef in _orgsByRef.Keys)
            {
                data.AddRange(_reportsByOrgRef[orgRef].PlottingData);
            }

            string filename = Path.Combine(dir, "plotting.csv");

            using (StreamWriter writer = new StreamWriter(filename))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }

        /// <summary>Loads from directory.</summary>
        /// <exception cref="DirectoryNotFoundException">Thrown when the requested directory is not
        ///  present.</exception>
        /// <param name="dir">   The dir.</param>
        private static void LoadFromDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException($"input directory: {dir} not found!");
            }

            string[] files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories);

            foreach (string filename in files)
            {
                Bundle bundle = _fhirJsonParser.Parse<Bundle>(File.ReadAllText(filename));

                if ((bundle.Entry == null) || (bundle.Entry.Count == 0))
                {
                    continue;
                }

                // traverse the bundle entries
                foreach (Bundle.EntryComponent entry in bundle.Entry)
                {
                    if ((entry == null) || (entry.Resource == null))
                    {
                        continue;
                    }

                    switch (entry.Resource.TypeName)
                    {
                        case "Organization":
                            string orgRef = $"Organization/{entry.Resource.Id}";

                            if (!_orgsByRef.ContainsKey(orgRef))
                            {
                                _orgsByRef.Add(orgRef, (Organization)entry.Resource);
                            }

                            break;

                        case "Location":
                            // only load site locations
                            Location loc = (Location)entry.Resource;

                            if ((loc.PhysicalType != null) &&
                                (loc.PhysicalType.Coding != null) &&
                                (loc.PhysicalType.Coding.Count != 0) &&
                                (loc.PhysicalType.Coding[0].Code == "si"))
                            {
                                string locRef = $"Location/{entry.Resource.Id}";

                                if (!_locationsByRef.ContainsKey(locRef))
                                {
                                    _locationsByRef.Add(locRef, loc);
                                }
                            }

                            break;

                        case "Measure":
                            Measure measure = (Measure)entry.Resource;

                            if (!_measuresByUrl.ContainsKey(measure.Url))
                            {
                                _measuresByUrl.Add(measure.Url, measure);
                            }

                            break;

                        case "MeasureReport":
                            MeasureReport report = (MeasureReport)entry.Resource;

                            if (!_reportsByOrgRef.ContainsKey(report.Reporter.Reference))
                            {
                                _reportsByOrgRef.Add(
                                    report.Reporter.Reference,
                                    new ReportCollection(report.Reporter.Reference));
                            }

                            _reportsByOrgRef[report.Reporter.Reference].AddReport(report);
                            break;
                    }
                }
            }
        }
    }
}
