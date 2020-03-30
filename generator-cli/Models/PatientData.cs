// <copyright file="PatientData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>A patient data.</summary>
    public class PatientData : AggregateQuantity
    {
        /// <summary>Initializes a new instance of the <see cref="PatientData"/> class.</summary>
        /// <param name="positive">The positive.</param>
        /// <param name="negative">The negative.</param>
        /// <param name="total">   The total.</param>
        public PatientData(
            int positive,
            int negative,
            int total)
            : base(positive, negative, total)
        {
        }
    }
}
