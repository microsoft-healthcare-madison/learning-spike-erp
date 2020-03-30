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
        private static FhirJsonParser _jsonParser = null;

        private static Dictionary<string, Organization> _orgsByRef = null;
        private static Dictionary<string, Location> _locationsByRef = null;
        private static Dictionary<string, MeasureReport> _reportsByRef = null;

        /// <summary>Main entry-point for this application.</summary>
        /// <param name="inputDirectory"> Source Bundle folder (either this or --fhir-server-url is required).</param>
        /// <param name="fhirServerUrl">  FHIR server to use (either this or --input-directory is required).</param>
        /// <param name="outputDirectory">Location to write formatted data.</param>
        public static void Main(
            string inputDirectory,
            string fhirServerUrl,
            string outputDirectory)
        {
            _jsonParser = new FhirJsonParser();
            _orgsByRef = new Dictionary<string, Organization>();
            _locationsByRef = new Dictionary<string, Location>();
            _reportsByRef = new Dictionary<string, MeasureReport>();

            if (!string.IsNullOrEmpty(inputDirectory))
            {
                LoadFromDirectory(inputDirectory);
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

                        case "MeasureReport":
                            _reportsByRef.Add($"MeasureReport/{entry.Resource.Id}", (MeasureReport)entry.Resource);
                            break;
                    }
                }
            }
        }
    }
}
