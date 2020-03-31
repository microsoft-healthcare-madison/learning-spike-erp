// <copyright file="OrgDeviceData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>An organization's aggregate data.</summary>
    public class OrgDeviceData
    {
        private readonly int _totalBeds;
        private readonly int _inpatient;
        private readonly int _icu;
        private readonly int _ventilators;

        /// <summary>Initializes a new instance of the <see cref="OrgDeviceData"/> class.</summary>
        /// <param name="beds">         The beds.</param>
        /// <param name="inpatientBeds">The inpatient beds.</param>
        /// <param name="icuBeds">      The icu beds.</param>
        /// <param name="ventilators">  The ventilators.</param>
        public OrgDeviceData(
            int beds,
            int inpatientBeds,
            int icuBeds,
            int ventilators)
        {
            _totalBeds = beds;
            _inpatient = inpatientBeds;
            _icu = icuBeds;
            _ventilators = ventilators;
        }

        /// <summary>Gets the total bed info.</summary>
        /// <value>The total bed info.</value>
        public int TotalBeds => _totalBeds;

        /// <summary>Gets the inpatient bed info.</summary>
        /// <value>The inpatient bed info.</value>
        public int Inpatient => _inpatient;

        /// <summary>Gets the ICU bed info.</summary>
        /// <value>The ICU bed info.</value>
        public int ICU => _icu;

        /// <summary>Gets the ventilators.</summary>
        /// <value>The ventilators.</value>
        public int Ventilators => _ventilators;
    }
}
