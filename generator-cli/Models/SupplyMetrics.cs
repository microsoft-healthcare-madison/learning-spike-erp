// <copyright file="SupplyMetrics.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>A supply metrics.</summary>
    public class SupplyMetrics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupplyMetrics"/> class.
        /// </summary>
        /// <param name="supplyChoiceIndex">The supply choice index.</param>
        /// <param name="reusing">          True if reusing, false if not.</param>
        /// <param name="canObtainMore">    True if we can obtain more, false if not.</param>
        public SupplyMetrics(int supplyChoiceIndex, bool reusing, bool canObtainMore)
        {
            SupplyChoiceIndex = supplyChoiceIndex;
            Reusing = reusing;
            CanObtainMore = canObtainMore;
        }

        /// <summary>Gets the zero-based index of the supply choice.</summary>
        /// <value>The supply choice index.</value>
        public int SupplyChoiceIndex { get; }

        /// <summary>Gets a value indicating whether the reusing.</summary>
        /// <value>True if reusing, false if not.</value>
        public bool Reusing { get; }

        /// <summary>Gets a value indicating whether we can obtain more.</summary>
        /// <value>True if we can obtain more, false if not.</value>
        public bool CanObtainMore { get; }
    }
}
