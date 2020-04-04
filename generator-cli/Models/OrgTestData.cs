// <copyright file="OrgTestData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Models
{
    /// <summary>An organization test data.</summary>
    public class OrgTestData
    {
        private int _performedToday;
        private int _positiveToday;
        private int _negativeToday;
        private int _rejected;
        private int _rejectedToday;
        private int _performed;
        private int _positive;
        private int _negative;
        private int _pending;

        /// <summary>Initializes a new instance of the <see cref="OrgTestData"/> class.</summary>
        /// <param name="performed">The tests performed.</param>
        /// <param name="positive"> The tests positive.</param>
        /// <param name="negative"> The tests negative.</param>
        /// <param name="pending">  The test pending.</param>
        public OrgTestData(
            int performed,
            int positive,
            int negative,
            int pending)
        {
            _performed = performed;
            _positive = positive;
            _negative = negative;
            _pending = pending;

            _performedToday = performed;
            _positiveToday = positive;
            _negativeToday = negative;
            _rejected = (int)(performed * 0.05);
            _rejectedToday = _rejected;
        }

        /// <summary>Gets the tests performed.</summary>
        /// <value>The tests performed.</value>
        public int Performed => _performed;

        /// <summary>Gets the tests positive.</summary>
        /// <value>The tests positive.</value>
        public int Positive => _positive;

        /// <summary>Gets the tests negative.</summary>
        /// <value>The tests negative.</value>
        public int Negative => _negative;

        /// <summary>Gets the test pending.</summary>
        /// <value>The test pending.</value>
        public int Pending => _pending;

        /// <summary>Gets the performed today.</summary>
        /// <value>The performed today.</value>
        public int PerformedToday => _performedToday;

        /// <summary>Gets the positive today.</summary>
        /// <value>The positive today.</value>
        public int PositiveToday => _positiveToday;

        /// <summary>Gets the negative today.</summary>
        /// <value>The negative today.</value>
        public int NegativeToday => _negativeToday;

        /// <summary>Gets the rejected.</summary>
        /// <value>The rejected.</value>
        public int Rejected => _rejected;

        /// <summary>Gets the rejected today.</summary>
        /// <value>The rejected today.</value>
        public int RejectedToday => _rejectedToday;

        /// <summary>Updates this object.</summary>
        /// <param name="performedDelta">The performed delta.</param>
        /// <param name="positiveDelta"> The positive delta.</param>
        /// <param name="negativeDelta"> The negative delta.</param>
        /// <param name="pending">       The tests pending.</param>
        public void Update(
            int performedDelta,
            int positiveDelta,
            int negativeDelta,
            int pending)
        {
            _performed += performedDelta;
            _positive += positiveDelta;
            _negative += negativeDelta;
            _pending = pending;

            _performedToday = performedDelta;
            _positiveToday = positiveDelta;
            _negativeToday = negativeDelta;
            _rejected = (int)((double)_pending * 0.05);
            _rejectedToday = (int)((double)performedDelta * 0.05);
        }
    }
}
