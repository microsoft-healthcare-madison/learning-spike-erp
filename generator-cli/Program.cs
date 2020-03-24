// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using generator_cli.Geographic;
using Hl7.Fhir.Model;
using Newtonsoft.Json;

namespace generator_cli
{
    /// <summary>A program.</summary>
    public class Program
    {
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

            GeoManager.Init();
        }
    }
}
