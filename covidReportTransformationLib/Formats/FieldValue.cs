// <copyright file="FieldValue.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A field value.</summary>
    public class FieldValue
    {
        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="measureScore">The measure score.</param>
        /// <param name="numerator">   The numerator.</param>
        /// <param name="denominator"> The denominator.</param>
        /// <param name="stringValue"> The string value.</param>
        public FieldValue(
            decimal? measureScore,
            int? numerator,
            int? denominator,
            string stringValue)
        {
            if (measureScore != null)
            {
                Score = (decimal)measureScore;
            }

            Numerator = numerator;
            Denominator = denominator;

            if ((measureScore == null) &&
                (numerator != null) &&
                (denominator != null))
            {
                Score = (decimal)numerator / (decimal)denominator;
            }

            StringValue = stringValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> class.
        /// </summary>
        public FieldValue()
            : this(null, null, null, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="stringValue">The string value.</param>
        public FieldValue(string stringValue)
            : this(null, null, null, stringValue)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="measureScore">The measure score.</param>
        public FieldValue(decimal measureScore)
            : this(measureScore, null, null, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="numerator">  The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public FieldValue(int numerator, int denominator)
            : this(null, numerator, denominator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> class.
        /// </summary>
        /// <param name="score">      The measure score.</param>
        /// <param name="numerator">  The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public FieldValue(decimal score, int numerator, int denominator)
            : this(score, numerator, denominator, null)
        {
        }

        /// <summary>Gets the string value.</summary>
        /// <value>The string value.</value>
        public string StringValue { get; }

        /// <summary>Gets the measure score.</summary>
        /// <value>The measure score.</value>
        public decimal? Score { get; }

        /// <summary>Gets the numerator.</summary>
        /// <value>The numerator.</value>
        public int? Numerator { get; }

        /// <summary>Gets the denominator.</summary>
        /// <value>The denominator.</value>
        public int? Denominator { get; }
    }
}
