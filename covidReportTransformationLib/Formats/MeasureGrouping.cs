// <copyright file="MeasureGrouping.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>Information about the measure group.</summary>
    public class MeasureGrouping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGrouping"/>
        /// class.
        /// </summary>
        /// <param name="codeCoding">      The code.</param>
        /// <param name="codeText">        The group code text.</param>
        /// <param name="groupAttributes"> The extensions.</param>
        /// <param name="populationFields">The populations.</param>
        public MeasureGrouping(
            FhirTriplet codeCoding,
            string codeText,
            List<MeasureGroupingExtension> groupAttributes,
            List<MeasureGroupingPopulation> populationFields)
        {
            FieldName = null;
            CodeCoding = codeCoding;
            CodeText = codeText;
            PopulationFields = populationFields;
            GroupAttributes = groupAttributes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGrouping"/>
        /// class.
        /// </summary>
        /// <param name="fieldName">      The name of the field.</param>
        /// <param name="groupAttributes">The extensions.</param>
        public MeasureGrouping(
            string fieldName,
            List<MeasureGroupingExtension> groupAttributes)
        {
            FieldName = fieldName;
            CodeCoding = null;
            CodeText = null;
            PopulationFields = null;
            GroupAttributes = groupAttributes;
        }

        /// <summary>Values that represent group systems.</summary>
        public enum SanerGroupSystem
        {
            /// <summary>An enum constant representing the none option.</summary>
            None,

            /// <summary>An enum constant representing the beds option.</summary>
            Beds,

            /// <summary>An enum constant representing the ventilators option.</summary>
            Ventilators,

            /// <summary>An enum constant representing the encounters option.</summary>
            Encounters,
        }

        /// <summary>Gets the name of the field.</summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; }

        /// <summary>Gets the code.</summary>
        /// <value>The code.</value>
        public FhirTriplet CodeCoding { get; }

        /// <summary>Gets the group code text.</summary>
        /// <value>The group code text.</value>
        public string CodeText { get; }

        /// <summary>Gets the populations.</summary>
        /// <value>The populations.</value>
        public List<MeasureGroupingPopulation> PopulationFields { get; }

        /// <summary>Gets the extensions.</summary>
        /// <value>The extensions.</value>
        public List<MeasureGroupingExtension> GroupAttributes { get; }
    }
}
