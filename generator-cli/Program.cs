// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using fhir = generator_cli.fhir;

namespace generator_cli
{
    /// <summary>A program.</summary>
    public class Program
    {
        private static readonly fhir.CodeableConcept[] _codingIcu = new fhir.CodeableConcept[1]
        {
            new fhir.CodeableConcept()
            {
                Coding = new fhir.Coding[1]
                {
                    new fhir.Coding()
                    {
                        Code = "ICU",
                        System = "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                    },
                },
            },
        };

        private static readonly fhir.CodeableConcept[] _codingHospital = new fhir.CodeableConcept[1]
        {
            new fhir.CodeableConcept()
            {
                Coding = new fhir.Coding[1]
                {
                    new fhir.Coding()
                    {
                        Code = "HOSP",
                        System = "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                    },
                },
            },
        };

        /// <summary>Main entry-point for this application.</summary>
        /// <param name="postalCode">     Postal code to use when updating ERP data.</param>
        /// <param name="outputDirectory">Directory to write JSON files.</param>
        /// <param name="fhirServerUrl">  URL of a FHIR server to interact with.</param>
        /// <param name="clean">          Delete all ERP resources for the given postal code.</param>
        /// <param name="generateStatic"> Generate a set of resources and exit.</param>
        /// <param name="runSimulation">  Generate a set of resources and periodically update it.</param>
        public static void Main(
            string postalCode,
            string outputDirectory = null,
            string fhirServerUrl = null,
            bool clean = false,
            bool generateStatic = false,
            bool runSimulation = false)
        {
            // sanity checks
            //if (string.IsNullOrEmpty(postalCode))
            //{
            //    throw new ArgumentNullException(nameof(postalCode));
            //}

            //if (string.IsNullOrEmpty(outputDirectory) && string.IsNullOrEmpty(fhirServerUrl))
            //{
            //    throw new ArgumentException("Either --output-directory or --fhir-server-url must be specified.");
            //}

            LoadLocationData();
        }

        /// <summary>Loads location data.</summary>
        public static void LoadLocationData()
        {
            string filename = Path.Combine(Directory.GetCurrentDirectory(), "data", "us-zip-code-latitude-and-longitude.json");

            List<ZipGeoCode> zips = JsonConvert.DeserializeObject<List<ZipGeoCode>>(File.ReadAllText(filename));

            Console.WriteLine($"Loaded {zips.Count} location records.");

            foreach (ZipGeoCode loc in zips)
            {
                if ((loc.fields.state == "WI") && (loc.fields.city == "Madison"))
                {
                    Console.WriteLine($"Found: {loc.datasetid}:{loc.recordid} - {loc.fields.city}, {loc.fields.state} {loc.fields.zip}");
                }
            }
        }

        /// <summary>Generates a location.</summary>
        /// <param name="occupied">  True if this location is occupied.</param>
        /// <param name="isIcu">     True if is location is an ICU type, false if not.</param>
        /// <param name="postalCode">Postal code to use when updating ERP data.</param>
        /// <returns>The location.</returns>
        public static string GenerateBedLocation(bool occupied, bool isIcu, string postalCode)
        {
            fhir.Location location = new fhir.Location()
            {
                OperationalStatus = occupied ? fhir.LocationOperationalStatusValues.O : fhir.LocationOperationalStatusValues.U,
                Type = isIcu ? _codingIcu : _codingHospital,
                PhysicalType = new fhir.CodeableConcept() { Coding = new fhir.Coding[1] { fhir.LocationPhysicalTypeValues.bd, }, },
                Address = new fhir.Address()
                {
                    PostalCode = postalCode,
                },
            };

            return JsonConvert.SerializeObject(location);
        }
    }
}
