// <copyright file="AggregateQuantity.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>An aggregate quantity.</summary>
    public class AggregateQuantity
    {
        /// <summary>Initializes a new instance of the <see cref="AggregateQuantity"/> class.</summary>
        /// <param name="positive">The positive.</param>
        /// <param name="negative">The negative.</param>
        /// <param name="total">   The total.</param>
        public AggregateQuantity(
            int positive,
            int negative,
            int total)
        {
            Positive = positive;
            Negative = negative;
            Total = total;
        }

        /// <summary>Gets the positive.</summary>
        /// <value>The positive.</value>
        public int Positive { get; }

        /// <summary>Gets the negative.</summary>
        /// <value>The negative.</value>
        public int Negative { get; }

        /// <summary>Gets the number of. </summary>
        /// <value>The total.</value>
        public int Total { get; }

        /// <summary>Gets the percent positive.</summary>
        /// <value>The percent positive.</value>
        public decimal PercentPositive => Positive / ((decimal)Total);

        /// <summary>Gets the percent negative.</summary>
        /// <value>The percent negative.</value>
        public decimal PercentNegative => Negative / ((decimal)Total);
    }
}
