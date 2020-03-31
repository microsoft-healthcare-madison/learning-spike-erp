// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;

namespace measureReportTransformer
{
    /// <summary>A program.</summary>
    public class Program
    {
        /// <summary>The JSON parser.</summary>
        private static FhirJsonParser _jsonParser = new FhirJsonParser();

        private static Dictionary<string, Organization> _orgsByRef = new Dictionary<string, Organization>();
        private static Dictionary<string, Location> _locationsByRef = new Dictionary<string, Location>();
        private static Dictionary<string, Measure> _measuresByUrl = new Dictionary<string, Measure>();
        private static Dictionary<string, ReportCollection> _reportsByOrgRef = new Dictionary<string, ReportCollection>();

        /// <summary>Main entry-point for this application.</summary>
        /// <param name="inputDirectory"> Source Bundle folder (either this or --fhir-server-url is required).</param>
        /// <param name="outputDirectory">Location to write formatted data.</param>
        public static void Main(
            string inputDirectory,
            string outputDirectory)
        {
            if (string.IsNullOrEmpty(inputDirectory))
            {
                throw new ArgumentNullException(nameof(inputDirectory));
            }

            LoadFromDirectory(inputDirectory);
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
                Bundle bundle = _jsonParser.Parse<Bundle>(File.ReadAllText(filename));

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
                            string orgId = $"Organization/{entry.Resource.Id}";

                            if (!_orgsByRef.ContainsKey(orgId))
                            {
                                _orgsByRef.Add(orgId, (Organization)entry.Resource);
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
                                _locationsByRef.Add($"Location/{entry.Resource.Id}", loc);
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

                            if (_reportsByOrgRef.ContainsKey(report.Reporter.Reference))
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
