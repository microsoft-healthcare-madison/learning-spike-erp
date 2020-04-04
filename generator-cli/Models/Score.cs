// <copyright file="Score.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>A score.</summary>
    public class Score
    {
        /// <summary>Initializes a new instance of the <see cref="Score"/> class.</summary>
        /// <param name="measureScore">The measure score.</param>
        /// <param name="numerator">   The numerator.</param>
        /// <param name="denominator"> The denominator.</param>
        public Score(
            decimal? measureScore,
            int? numerator,
            int? denominator)
        {
            if (measureScore != null)
            {
                MeasureScore = (decimal)measureScore;
            }

            Numerator = numerator;
            Denominator = denominator;

            if ((measureScore == null) &&
                (numerator != null) &&
                (denominator != null))
            {
                MeasureScore = (decimal)numerator / (decimal)denominator;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Score"/> class.</summary>
        /// <param name="measureScore">The measure score.</param>
        public Score(decimal measureScore)
            : this(measureScore, null, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Score"/> class.</summary>
        /// <param name="numerator">  The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public Score(int numerator, int denominator)
            : this(null, numerator, denominator)
        {
        }

        /// <summary>Gets the measure score.</summary>
        /// <value>The measure score.</value>
        public decimal MeasureScore { get; }

        /// <summary>Gets the numerator.</summary>
        /// <value>The numerator.</value>
        public int? Numerator { get; }

        /// <summary>Gets the denominator.</summary>
        /// <value>The denominator.</value>
        public int? Denominator { get; }
    }
}
