// <copyright file="BedData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>A bed category data.</summary>
    public class BedData : AggregateQuantity
    {
        /// <summary>Initializes a new instance of the <see cref="BedData"/> class.</summary>
        /// <param name="available">The available.</param>
        /// <param name="inUse">    The in use.</param>
        /// <param name="total">    The total.</param>
        public BedData(
            int available,
            int inUse,
            int total)
            : base(available, inUse, total)
        {
        }

        /// <summary>Gets the available.</summary>
        /// <value>The available.</value>
        public int Available => Positive;

        /// <summary>Gets the in use.</summary>
        /// <value>The in use.</value>
        public int InUse => Negative;

        /// <summary>Gets the percent available.</summary>
        /// <value>The percent available.</value>
        public decimal PercentAvailable => PercentPositive;

        /// <summary>Gets the percent in use.</summary>
        /// <value>The percent in use.</value>
        public decimal PercentInUse => PercentNegative;
    }
}
