// <copyright file="OrgSupplyData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>An organization supply data.</summary>
    public class OrgSupplyData
    {
        private SupplyMetrics _ventilators;
        private SupplyMetrics _n95Masks;
        private SupplyMetrics _otherRespirators;
        private SupplyMetrics _surgicalMasks;
        private SupplyMetrics _faceShields;
        private SupplyMetrics _gowns;
        private SupplyMetrics _gloves;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrgSupplyData"/> class.
        /// </summary>
        /// <param name="ventilators">     The ventilators.</param>
        /// <param name="n95Masks">        The n 95 masks.</param>
        /// <param name="otherRespirators">The other respirators.</param>
        /// <param name="surgicalMasks">   The surgical masks.</param>
        /// <param name="faceShields">     The face shields.</param>
        /// <param name="gowns">           The gowns.</param>
        /// <param name="gloves">          The gloves.</param>
        public OrgSupplyData(
            SupplyMetrics ventilators,
            SupplyMetrics n95Masks,
            SupplyMetrics otherRespirators,
            SupplyMetrics surgicalMasks,
            SupplyMetrics faceShields,
            SupplyMetrics gowns,
            SupplyMetrics gloves)
        {
            _ventilators = ventilators;
            _n95Masks = n95Masks;
            _otherRespirators = otherRespirators;
            _surgicalMasks = surgicalMasks;
            _faceShields = faceShields;
            _gowns = gowns;
            _gloves = gloves;
        }

        /// <summary>Gets the ventilators.</summary>
        /// <value>The ventilators.</value>
        public SupplyMetrics Ventilators => _ventilators;

        /// <summary>Gets the 95 masks.</summary>
        /// <value>The n 95 masks.</value>
        public SupplyMetrics N95Masks => _n95Masks;

        /// <summary>Gets the other respirators.</summary>
        /// <value>The other respirators.</value>
        public SupplyMetrics OtherRespirators => _otherRespirators;

        /// <summary>Gets the surgical masks.</summary>
        /// <value>The surgical masks.</value>
        public SupplyMetrics SurgicalMasks => _surgicalMasks;

        /// <summary>Gets the face shields.</summary>
        /// <value>The face shields.</value>
        public SupplyMetrics FaceShields => _faceShields;

        /// <summary>Gets the gowns.</summary>
        /// <value>The gowns.</value>
        public SupplyMetrics Gowns => _gowns;

        /// <summary>Gets the gloves.</summary>
        /// <value>The gloves.</value>
        public SupplyMetrics Gloves => _gloves;
    }
}
