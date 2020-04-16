// <copyright file="FemaLiterals.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.FEMA
{
    /// <summary>A fema literals.</summary>
    public abstract class FemaLiterals
    {
        /// <summary>The fema tests ordered today.</summary>
        public const string TestsOrderedToday = "newDiagnosticTests";

        /// <summary>The tests ordered today title.</summary>
        public const string TestsOrderedTodayTitle = "New Diagnostic Tests Ordered/Received";

        /// <summary>Information describing the tests ordered today.</summary>
        public const string TestsOrderedTodayDesc = "Midnight to midnight cutoff, tests ordered on previous date queried.";

        /// <summary>The fema tests ordered total.</summary>
        public const string TestsOrderedTotal = "cumulativeDiagnosticTests";

        /// <summary>The tests ordered total title.</summary>
        public const string TestsOrderedTotalTitle = "Cumulative Diagnostic Tests Ordered/Received";

        /// <summary>Information describing the tests ordered total.</summary>
        public const string TestsOrderedTotalDesc = "All tests ordered to date.";

        /// <summary>The fema tests with results new.</summary>
        public const string TestsWithResultsToday = "newTestsResulted";

        /// <summary>The tests with results today title.</summary>
        public const string TestsWithResultsTodayTitle = "New Tests Resulted";

        /// <summary>Information describing the tests with results today.</summary>
        public const string TestsWithResultsTodayDesc = "Midnight to midnight cutoff, test results released on previous date queried.";

        /// <summary>The fema specimens rejected total.</summary>
        public const string SpecimensRejectedTotal = "cumulativeSpecimensRejected";

        /// <summary>The specimens rejected total title.</summary>
        public const string SpecimensRejectedTotalTitle = "Cumulative Specimens Rejected";

        /// <summary>Information describing the specimens rejected total.</summary>
        public const string SpecimensRejectedTotalDesc = "All specimens rejected for testing to date.";

        /// <summary>The fema tests performed total.</summary>
        public const string TestsCompletedTotal = "cumulativeTestsPerformed";

        /// <summary>The tests completed total title.</summary>
        public const string TestsCompletedTotalTitle = "Cumulative Tests Performed";

        /// <summary>Information describing the tests completed total.</summary>
        public const string TestsCompletedTotalDesc = "All tests with results released to date.";

        /// <summary>The fema positive c 19 today.</summary>
        public const string PositiveC19Today = "newPositiveC19Tests";

        /// <summary>The positive c 19 today title.</summary>
        public const string PositiveC19TodayTitle = "New Positive COVID-19 Tests";

        /// <summary>Information describing the positive c 19 today.</summary>
        public const string PositiveC19TodayDesc = "Midnight to midnight cutoff, positive test results released on the previous date queried.";

        /// <summary>The fema positive c 19 total.</summary>
        public const string PositiveC19Total = "cumulativePositiveC19Tests";

        /// <summary>The positive c 19 total title.</summary>
        public const string PositiveC19TotalTitle = "Cumulative Positive COVID-19 Tests";

        /// <summary>Information describing the positive c 19 total.</summary>
        public const string PositiveC19TotalDesc = "All positivetest results released to date.";

        /// <summary>The fema percent c 19 postive.</summary>
        public const string PercentC19PositiveToday = "percentPositiveAmongNewlyResultedTests";

        /// <summary>The percent c 19 positive today title.</summary>
        public const string PercentC19PositiveTodayTitle = "Percent Positive among Newly Resulted Tests";

        /// <summary>Information describing the percent c 19 positive today.</summary>
        public const string PercentC19PositiveTodayDesc = "# of new positive test results / # of total new tests released for previous date queried.";

        /// <summary>The fema percent c 19 positive total.</summary>
        public const string PercentC19PositiveTotal = "cumulativePercentPositiveAmongResultedTests";

        /// <summary>The percent c 19 positive total title.</summary>
        public const string PercentC19PositiveTotalTitle = "Cumulative Percent Positive among Resulted Tests";

        /// <summary>Information describing the percent c 19 positive total.</summary>
        public const string PercentC19PositiveTotalDesc = "# of total positive results to released date / # of total test results released to date.";
    }
}
