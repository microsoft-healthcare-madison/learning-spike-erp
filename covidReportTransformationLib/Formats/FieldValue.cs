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
        /// <param name="boolValue">   True to value.</param>
        public FieldValue(
            decimal? measureScore,
            int? numerator,
            int? denominator,
            string stringValue,
            bool? boolValue)
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

            if (boolValue != null)
            {
                IsBoolean = true;
                BoolValue = boolValue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> class.
        /// </summary>
        public FieldValue()
            : this(null, null, null, null, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="boolValue">Boolean value of this field.</param>
        public FieldValue(bool boolValue)
            : this(null, null, null, null, boolValue)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="stringValue">The string value.</param>
        public FieldValue(string stringValue)
            : this(null, null, null, stringValue, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="measureScore">The measure score.</param>
        public FieldValue(decimal measureScore)
            : this(measureScore, null, null, null, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FieldValue"/> class.</summary>
        /// <param name="numerator">  The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public FieldValue(int numerator, int denominator)
            : this(null, numerator, denominator, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldValue"/> class.
        /// </summary>
        /// <param name="score">      The measure score.</param>
        /// <param name="numerator">  The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public FieldValue(decimal score, int numerator, int denominator)
            : this(score, numerator, denominator, null, null)
        {
        }

        /// <summary>Gets a value indicating whether this object is boolean.</summary>
        /// <value>True if this object is boolean, false if not.</value>
        public bool IsBoolean { get; }

        /// <summary>Gets the value.</summary>
        /// <value>The bool value.</value>
        public bool? BoolValue { get; }

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
