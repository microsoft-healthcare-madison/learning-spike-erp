// <copyright file="FemaModel.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace measureReportTransformer.FEMA
{
    /// <summary>A data Model for the fema.</summary>
    public class FemaModel
    {
        /// <summary>Initializes a new instance of the <see cref="FemaModel"/> class.</summary>
        /// <param name="testsOrderedToday">      The tests ordered today.</param>
        /// <param name="testsOrderedTotal">      The tests ordered total.</param>
        /// <param name="testResultsToday">       The test results today.</param>
        /// <param name="specimensRejectedTotal"> The specimens rejected total.</param>
        /// <param name="testsPerformedTotal">    The tests performed total.</param>
        /// <param name="c19TestsPositiveToday">  The c 19 tests positive today.</param>
        /// <param name="percentC19PositiveToday">The percent c 19 positive today.</param>
        /// <param name="percentC19PositiveTotal">The percent c 19 positive total.</param>
        public FemaModel(
            int testsOrderedToday,
            long testsOrderedTotal,
            int testResultsToday,
            int specimensRejectedTotal,
            long testsPerformedTotal,
            int c19TestsPositiveToday,
            decimal percentC19PositiveToday,
            decimal percentC19PositiveTotal)
        {
            TestsOrderedToday = testsOrderedToday;
            TestsOrderedTotal = testsOrderedTotal;
            TestResultsToday = testResultsToday;
            SpecimensRejectedTotal = specimensRejectedTotal;
            TestsPerformedTotal = testsPerformedTotal;
            C19TestsPositiveToday = c19TestsPositiveToday;
            PercentC19PositiveToday = percentC19PositiveToday;
            PercentC19PositiveTotal = percentC19PositiveTotal;
        }

        /// <summary>Gets the tests ordered today.</summary>
        /// <value>The tests ordered today.</value>
        public int? TestsOrderedToday { get; }

        /// <summary>Gets the tests ordered total.</summary>
        /// <value>The tests ordered total.</value>
        public long? TestsOrderedTotal { get; }

        /// <summary>Gets the test results today.</summary>
        /// <value>The test results today.</value>
        public int? TestResultsToday { get; }

        /// <summary>Gets the specimens rejected total.</summary>
        /// <value>The specimens rejected total.</value>
        public int? SpecimensRejectedTotal { get; }

        /// <summary>Gets the tests performed total.</summary>
        /// <value>The tests performed total.</value>
        public long? TestsPerformedTotal { get; }

        /// <summary>Gets the 19 tests positive today.</summary>
        /// <value>The c 19 tests positive today.</value>
        public int? C19TestsPositiveToday { get; }

        /// <summary>Gets the percent c 19 positive today.</summary>
        /// <value>The percent c 19 positive today.</value>
        public decimal? PercentC19PositiveToday { get; }

        /// <summary>Gets the percent c 19 positive total.</summary>
        /// <value>The percent c 19 positive total.</value>
        public decimal? PercentC19PositiveTotal { get; }
    }
}
