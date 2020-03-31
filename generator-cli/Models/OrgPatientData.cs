// <copyright file="OrgPatientData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>A patient data.</summary>
    public class OrgPatientData
    {
        private int _onsetInCare;
        private int _recovered;
        private int _died;
        private int _total;
        private int _positive;
        private int _positiveNeedIcu;
        private int _positiveNeedVent;
        private int _negative;
        private int _negativeNeedIcu;
        private int _negativeNeedVent;

        /// <summary>Initializes a new instance of the <see cref="OrgPatientData"/> class.</summary>
        /// <param name="total">           Number of patients.</param>
        /// <param name="positive">        The positive.</param>
        /// <param name="positiveNeedIcu"> The positive need icu.</param>
        /// <param name="positiveNeedVent">The positive need vent.</param>
        /// <param name="negative">        The negative.</param>
        /// <param name="negativeNeedIcu"> The negative need icu.</param>
        /// <param name="negativeNeedVent">The negative need vent.</param>
        /// <param name="onsetInCare">     The patients that became positive while under care.</param>
        /// <param name="recovered">       The patients recovered.</param>
        /// <param name="died">            The patients died.</param>
        public OrgPatientData(
            int total,
            int positive,
            int positiveNeedIcu,
            int positiveNeedVent,
            int negative,
            int negativeNeedIcu,
            int negativeNeedVent,
            int onsetInCare,
            int recovered,
            int died)
        {
            _total = total;
            _positive = positive;
            _positiveNeedIcu = positiveNeedIcu;
            _positiveNeedVent = positiveNeedVent;
            _negative = negative;
            _negativeNeedIcu = negativeNeedIcu;
            _negativeNeedVent = negativeNeedVent;
            _onsetInCare = onsetInCare;
            _recovered = recovered;
            _died = died;
        }

        /// <summary>Gets the total number of patients. </summary>
        /// <value>The total.</value>
        public int Total => _total;

        /// <summary>Gets the number of COVID positive patients.</summary>
        /// <value>The positive.</value>
        public int Positive => _positive;

        /// <summary>Gets the number of COVID patients in the hospital that do not require ICU.</summary>
        /// <value>The positive non icu.</value>
        public int PositiveNonIcu => _positive - _positiveNeedIcu;

        /// <summary>Gets the number of COVID patients that need ICU.</summary>
        /// <value>The positive need icu.</value>
        public int PositiveNeedIcu => _positiveNeedIcu;

        /// <summary>Gets the number of COVID patients that need ventilators.</summary>
        /// <value>The positive need vent.</value>
        public int PositiveNeedVent => _positiveNeedVent;

        /// <summary>Gets the number of COVID negative / untested patients.</summary>
        /// <value>The negative.</value>
        public int Negative => _negative;

        /// <summary>Gets the number of non-COVID patients that need ICU.</summary>
        /// <value>The negative need icu.</value>
        public int NegativeNeedIcu => _negativeNeedIcu;

        /// <summary>Gets the number of non-COVID patients that need ventilators.</summary>
        /// <value>The negative need vent.</value>
        public int NegativeNeedVent => _negativeNeedVent;

        /// <summary>Gets the number of patients who developed COVID while under care.</summary>
        /// <value>The patients positive onset.</value>
        public int OnsetInCare => _onsetInCare;

        /// <summary>Gets the patients recovered.</summary>
        /// <value>The patients recovered.</value>
        public int Recovered => _recovered;

        /// <summary>Gets the patients died.</summary>
        /// <value>The patients died.</value>
        public int Died => _died;

        /// <summary>Updates this object.</summary>
        /// <param name="total">           Number of patients.</param>
        /// <param name="positive">        The positive.</param>
        /// <param name="positiveNeedIcu"> The positive need icu.</param>
        /// <param name="positiveNeedVent">The positive need vent.</param>
        /// <param name="negative">        The negative.</param>
        /// <param name="negativeNeedIcu"> The negative need icu.</param>
        /// <param name="negativeNeedVent">The negative need vent.</param>
        /// <param name="onsetInCare">     The patients that became positive while under care.</param>
        /// <param name="recovered">       The patients recovered.</param>
        /// <param name="died">            The patients died.</param>
        public void Update(
            int total,
            int positive,
            int positiveNeedIcu,
            int positiveNeedVent,
            int negative,
            int negativeNeedIcu,
            int negativeNeedVent,
            int onsetInCare,
            int recovered,
            int died)
        {
            _total = total;
            _positive = positive;
            _positiveNeedIcu = positiveNeedIcu;
            _positiveNeedVent = positiveNeedVent;
            _negative = negative;
            _negativeNeedIcu = negativeNeedIcu;
            _negativeNeedVent = negativeNeedVent;
            _onsetInCare = onsetInCare;
            _recovered = recovered;
            _died = died;
        }
    }
}
