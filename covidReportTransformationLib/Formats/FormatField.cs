// <copyright file="FormatField.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A format field.</summary>
    public class FormatField
    {
        /// <summary>Initializes a new instance of the <see cref="FormatField"/> class.</summary>
        /// <param name="name">       The name.</param>
        /// <param name="title">      The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="type">       The type.</param>
        /// <param name="measureType">The type of the measure.</param>
        /// <param name="isRequired"> True if this object is required, false if not.</param>
        /// <param name="countMin">   The count minimum.</param>
        /// <param name="countMax">   The count maximum.</param>
        /// <param name="options">    The options allowed for this question (choices).</param>
        public FormatField(
            string name,
            string title,
            string description,
            FieldType type,
            FhirMeasureType measureType,
            bool isRequired,
            int? countMin,
            int? countMax,
            List<FormatFieldOption> options)
        {
            Name = name;
            Title = title;
            Description = description;
            Type = type;
            MeasureType = measureType;
            IsRequired = isRequired;
            CountMin = countMin;
            CountMax = countMax;
            Options = options;
        }

        /// <summary>Values that represent field types.</summary>
        public enum FieldType
        {
            /// <summary>An enum constant representing the display option.</summary>
            Display,

            /// <summary>An enum constant representing the date option.</summary>
            Date,

            /// <summary>An enum constant representing the count option.</summary>
            Count,

            /// <summary>An enum constant representing the percentage option.</summary>
            Percentage,

            /// <summary>An enum constant representing the boolean option.</summary>
            Boolean,

            /// <summary>An enum constant representing the choice option.</summary>
            Choice,

            /// <summary>An enum constant representing the Text option.</summary>
            Text,

            /// <summary>An enum constant representing the string option.</summary>
            ShortString,
        }

        /// <summary>Values that represent measure types.</summary>
        public enum FhirMeasureType
        {
            /// <summary>An enum constant representing the none option.</summary>
            None,

            /// <summary>An enum constant representing the structure option.</summary>
            Structure,

            /// <summary>An enum constant representing the outcome option.</summary>
            Outcome,

            /// <summary>An enum constant representing the composite option.</summary>
            Composite,
        }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title { get; }

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public FieldType Type { get; }

        /// <summary>Gets the type of the measure.</summary>
        /// <value>The type of the measure.</value>
        public FhirMeasureType MeasureType { get; }

        /// <summary>Gets a value indicating whether this object is required.</summary>
        /// <value>True if this object is required, false if not.</value>
        public bool IsRequired { get; }

        /// <summary>Gets the minimum value for count, if a range is defined.</summary>
        /// <value>The count minimum.</value>
        public int? CountMin { get; }

        /// <summary>Gets the maximum value for count, if a range is defined.</summary>
        /// <value>The count maximum.</value>
        public int? CountMax { get; }

        /// <summary>Gets the choices.</summary>
        /// <value>The choices.</value>
        public List<FormatFieldOption> Options { get; }
    }
}
