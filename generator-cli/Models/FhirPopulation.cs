// <copyright file="FhirPopulation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace generator_cli.Models
{
    /// <summary>A fhir population.</summary>
    public class FhirPopulation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FhirPopulation"/> class.
        /// </summary>
        /// <param name="populationCode">The population code.</param>
        /// <param name="description">   The description.</param>
        public FhirPopulation(
            FhirTriplet populationCode,
            string description)
        {
            PopulationCode = populationCode;
            Description = description;
        }

        /// <summary>Gets the population code.</summary>
        /// <value>The population code.</value>
        public FhirTriplet PopulationCode { get; }

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>Gets the component.</summary>
        /// <value>The component.</value>
        public Measure.PopulationComponent Component => new Measure.PopulationComponent()
        {
            Code = PopulationCode.Concept,
            Description = Description,
            Criteria = new Expression()
            {
                Language = "text/plain",
            },
        };

        /// <summary>Ratios.</summary>
        /// <param name="numeratorDescription">  Information describing the numerator.</param>
        /// <param name="denominatorDescription">Information describing the denominator.</param>
        /// <returns>A List&lt;FhirPopulation&gt;.</returns>
        public static List<FhirPopulation> Ratio(
            string numeratorDescription,
            string denominatorDescription)
        {
            return new List<FhirPopulation>()
            {
                new FhirPopulation(
                    FhirTriplet.Numerator,
                    numeratorDescription),
                new FhirPopulation(
                    FhirTriplet.Denominator,
                    denominatorDescription),
            };
        }

        /// <summary>Measure population.</summary>
        /// <param name="populationDescription">Information describing the population.</param>
        /// <returns>A List&lt;FhirPopulation&gt;.</returns>
        public static List<FhirPopulation> MeasurePopulation(
            string populationDescription)
        {
            return new List<FhirPopulation>()
            {
                new FhirPopulation(
                    FhirTriplet.MeasurePopulation,
                    populationDescription),
            };
        }
    }
}
