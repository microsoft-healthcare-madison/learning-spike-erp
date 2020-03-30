// <copyright file="OrganizationData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>An organization's aggregate data.</summary>
    public class OrganizationData
    {
        /// <summary>Gets the beds total.</summary>
        /// <value>The beds total.</value>
        public BedData BedsTotal { get; }

        /// <summary>Gets the beds ICU.</summary>
        /// <value>The beds ICU.</value>
        public BedData BedsICU { get; }

        /// <summary>Gets the beds inpatient.</summary>
        /// <value>The beds inpatient.</value>
        public BedData BedsInpatient { get; }

        /// <summary>Gets the patients COVID known or suspected.</summary>
        /// <value>The patients COVID known or suspected.</value>
        public int PatientsCovidKnownOrSuspected { get; }

        /// <summary>Gets the patients COVID.</summary>
        /// <value>The patients COVID.</value>
        public int PatientsCovidTested { get; }

        /// <summary>Gets the COVID tests ordered, completed vs pending.</summary>
        /// <value>The COVID tests ordered, complete vs pending.</value>
        public AggregateQuantity CovidTestCompletion { get; }

        /// <summary>Gets the patients ventilated.</summary>
        /// <value>The patients ventilated.</value>
        public PatientData PatientsVentilated { get; }

        /// <summary>Gets the ventilators.</summary>
        /// <value>The ventilators.</value>
        public VentilatorData Ventilators { get; }
    }
}
