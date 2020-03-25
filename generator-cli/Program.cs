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
        /// <param name="outputDirectory">Directory to write files.</param>
        /// <param name="outputFormat">   The output format, JSON or XML.</param>
        /// <param name="state">          State to restrict generation to.</param>
        /// <param name="postalCode">     Postal code to restrict generation to.</param>
        /// <param name="facilityCount">  Number of facilities to generate.</param>
        /// <param name="timeSteps">      Number of time-step updates to generate.</param>
        /// <param name="seed">           Starting seed to use in generation.</param>
        public static void Main(
            string outputDirectory,
            string outputFormat = "JSON",
            string state = null,
            string postalCode = null,
            int facilityCount = 10,
            int timeSteps = 10,
            int seed = 0)
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

            List<Organization> orgs = new List<Organization>();
            List<Location> orgLocs = new List<Location>();

            string currentDir = Path.Combine(outputDirectory, "t0");
            string filename = string.Empty;

            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }

            // generate our initial data set
            for (int facilityNumber = 0; facilityNumber < facilityCount; facilityNumber++)
            {
                Organization org = HospitalManager.GetOrganization(state, postalCode);
                orgs.Add(org);
                if (useJson)
                {
                    filename = Path.Combine(currentDir, $"H{org.Identifier[0].Value}.json");
                    File.WriteAllText(filename, jsonSerializer.SerializeToString(org));
                }
                else
                {
                    filename = Path.Combine(currentDir, $"{org.Identifier[0].Value}.xml");
                    File.WriteAllText(filename, xmlSerializer.SerializeToString(org));
                }

                Location orgLoc = FhirGenerator.RootLocationForOrg(org);
                orgLocs.Add(orgLoc);
                if (useJson)
                {
                    filename = Path.Combine(currentDir, $"{org.Identifier[0].Value}-loc.json");
                    File.WriteAllText(filename, jsonSerializer.SerializeToString(orgLoc));
                }
                else
                {
                    filename = Path.Combine(currentDir, $"{org.Identifier[0].Value}-loc.xml");
                    File.WriteAllText(filename, xmlSerializer.SerializeToString(orgLoc));
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
