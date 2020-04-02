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
        /// <summary>Initializes a new instance of the <see cref="FhirPopulation"/> class.</summary>
        /// <param name="locationDescription">The description.</param>
        /// <param name="criteriaDescription">Information describing the criteria.</param>
        /// <param name="criteriaExpression"> The criteria expression.</param>
        public FhirPopulation(
            string locationDescription,
            string criteriaDescription,
            string criteriaExpression)
        {
            LocationDescription = locationDescription;
            CriteriaDescription = criteriaDescription;
            CriteriaExpression = criteriaExpression;
        }

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string LocationDescription { get; }

        /// <summary>Gets information describing the criteria.</summary>
        /// <value>Information describing the criteria.</value>
        public string CriteriaDescription { get; }

        /// <summary>Gets the criteria expression.</summary>
        /// <value>The criteria expression.</value>
        public string CriteriaExpression { get; }

        /// <summary>Gets the component.</summary>
        /// <value>The component.</value>
        public List<Measure.PopulationComponent> ComponentList => new List<Measure.PopulationComponent>()
        {
            new Measure.PopulationComponent()
            {
                Code = FhirTriplet.MeasurePopulation.Concept,
                Description = LocationDescription,
            },
            new Measure.PopulationComponent()
            {
                Code = FhirTriplet.MeasureObservation.Concept,
                Criteria = new Expression()
                {
                    Language = "text/plain",
                    Expression_ = CriteriaExpression,
                },
            },
        };
    }
}
