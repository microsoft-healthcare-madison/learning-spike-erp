// <copyright file="MeasureGroupingPopulation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A measure grouping population.</summary>
    public class MeasureGroupingPopulation
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MeasureGroupingPopulation"/> class.
        /// </summary>
        /// <param name="name">          The name.</param>
        /// <param name="populationType">The type of the population.</param>
        public MeasureGroupingPopulation(string name, FhirTriplet populationType)
        {
            Name = name;
            PopulationType = populationType;
        }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>Gets the type of the population.</summary>
        /// <value>The type of the population.</value>
        public FhirTriplet PopulationType { get; }
    }
}
