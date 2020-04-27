// <copyright file="DailyReporting.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.FEMA
{
    /// <summary>FEMA Measurement Group info.</summary>
    public class DailyReporting : IReportingFormat
    {
        /// <summary>The collection date.</summary>
        public const string CollectionDate = "collectionDate";

        /// <summary>The state.</summary>
        public const string State = "state";

        /// <summary>The county.</summary>
        public const string County = "county";

        /// <summary>The fema tests ordered today.</summary>
        public const string TestsOrderedToday = "newDiagnosticTests";

        /// <summary>The fema tests ordered total.</summary>
        public const string TestsOrderedTotal = "cumulativeDiagnosticTests";

        /// <summary>The fema tests with results new.</summary>
        public const string TestsWithResultsToday = "newTestsResulted";

        /// <summary>The fema specimens rejected total.</summary>
        public const string SpecimensRejectedTotal = "cumulativeSpecimensRejected";

        /// <summary>The fema tests performed total.</summary>
        public const string TestsCompletedTotal = "cumulativeTestsPerformed";

        /// <summary>The fema positive c 19 today.</summary>
        public const string PositiveC19Today = "newPositiveC19Tests";

        /// <summary>The fema positive c 19 total.</summary>
        public const string PositiveC19Total = "cumulativePositiveC19Tests";

        /// <summary>The fema percent c 19 postive.</summary>
        public const string PercentC19PositiveToday = "percentPositiveAmongNewlyResultedTests";

        /// <summary>The fema percent c 19 positive total.</summary>
        public const string PercentC19PositiveTotal = "cumulativePercentPositiveAmongResultedTests";

        /// <summary>The current.</summary>
        private static DailyReporting _current = new DailyReporting();

        /// <summary>The fields.</summary>
        private static readonly Dictionary<string, FormatField> _fields = new Dictionary<string, FormatField>()
        {
            {
                CollectionDate,
                new FormatField(
                    CollectionDate,
                    "Date",
                    string.Empty,
                    FormatField.FieldType.Date,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    null,
                    null,
                    null)
            },
            {
                State,
                new FormatField(
                    State,
                    "State",
                    string.Empty,
                    FormatField.FieldType.ShortString,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                County,
                new FormatField(
                    County,
                    "County",
                    string.Empty,
                    FormatField.FieldType.ShortString,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                TestsOrderedToday,
                new FormatField(
                    TestsOrderedToday,
                    "New Diagnostic Tests Ordered/Received",
                    "Midnight to midnight cutoff, tests ordered on previous date queried.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                TestsOrderedTotal,
                new FormatField(
                    TestsOrderedTotal,
                    "Cumulative Diagnostic Tests Ordered/Received",
                    "All tests ordered to date.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                TestsWithResultsToday,
                new FormatField(
                    TestsWithResultsToday,
                    "New Tests Resulted",
                    "Midnight to midnight cutoff, test results released on previous date queried.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                SpecimensRejectedTotal,
                new FormatField(
                    SpecimensRejectedTotal,
                    "Cumulative Specimens Rejected",
                    "All specimens rejected for testing to date.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                TestsCompletedTotal,
                new FormatField(
                    TestsCompletedTotal,
                    "Cumulative Tests Performed",
                    "All tests with results released to date.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PositiveC19Today,
                new FormatField(
                    PositiveC19Today,
                    "New Positive COVID-19 Tests",
                    "Midnight to midnight cutoff, positive test results released on the previous date queried.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PositiveC19Total,
                new FormatField(
                    PositiveC19Total,
                    "Cumulative Positive COVID-19 Tests",
                    "All positive test results released to date.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PercentC19PositiveToday,
                new FormatField(
                    PercentC19PositiveToday,
                    "Percent Positive among Newly Resulted Tests",
                    "# of new positive test results / # of total new tests released for previous date queried.",
                    FormatField.FieldType.Percentage,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PercentC19PositiveTotal,
                new FormatField(
                    PercentC19PositiveTotal,
                    "Cumulative Percent Positive among Resulted Tests",
                    "# of total positive results to released date / # of total test results released to date.",
                    FormatField.FieldType.Percentage,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    null,
                    null,
                    null)
            },
        };

        /// <summary>The measure report fields.</summary>
        private static readonly List<string> _measureReportFields = new List<string>
        {
            TestsOrderedToday,
            TestsOrderedTotal,
            TestsWithResultsToday,
            SpecimensRejectedTotal,
            TestsCompletedTotal,
            PositiveC19Today,
            PositiveC19Total,
            PercentC19PositiveToday,
            PercentC19PositiveTotal,
        };

        /// <summary>The questionnaire sections.</summary>
        private static readonly List<QuestionnaireSection> _questionnaireSections = new List<QuestionnaireSection>
        {
            new QuestionnaireSection(
                "Daily Hospital COVID-19 Reporting",
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(CollectionDate),
                    new QuestionnaireQuestion(State),
                    new QuestionnaireQuestion(County),
                    new QuestionnaireQuestion(TestsOrderedToday),
                    new QuestionnaireQuestion(TestsOrderedTotal),
                    new QuestionnaireQuestion(TestsWithResultsToday),
                    new QuestionnaireQuestion(SpecimensRejectedTotal),
                    new QuestionnaireQuestion(TestsCompletedTotal),
                    new QuestionnaireQuestion(PositiveC19Today),
                    new QuestionnaireQuestion(PositiveC19Total),
                }),
        };

        /// <summary>The spreadsheet locations.</summary>
        private static readonly Dictionary<string, SpreadsheetLocation> _spreadsheetLocations = new Dictionary<string, SpreadsheetLocation>()
        {
            {
                CollectionDate,
                new SpreadsheetLocation("A", 1)
            },
            {
                TestsOrderedToday,
                new SpreadsheetLocation("B", 1)
            },
            {
                TestsOrderedTotal,
                new SpreadsheetLocation("C", 1)
            },
            {
                TestsWithResultsToday,
                new SpreadsheetLocation("D", 1)
            },
            {
                SpecimensRejectedTotal,
                new SpreadsheetLocation("E", 1)
            },
            {
                TestsCompletedTotal,
                new SpreadsheetLocation("F", 1)
            },
            {
                PositiveC19Today,
                new SpreadsheetLocation("G", 1)
            },
            {
                PositiveC19Total,
                new SpreadsheetLocation("H", 1)
            },
            {
                PercentC19PositiveToday,
                new SpreadsheetLocation("I", 1)
            },
            {
                PercentC19PositiveTotal,
                new SpreadsheetLocation("J", 1)
            },
            {
                State,
                new SpreadsheetLocation("B", 4)
            },
            {
                County,
                new SpreadsheetLocation("B", 5)
            },
        };

        /// <summary>The fema citation.</summary>
        private static readonly Hl7.Fhir.Model.Markdown _femaCitation = new Hl7.Fhir.Model.Markdown(
            "Federal Emergency Management Agency (FEMA)");

        /// <summary>List of fema documents.</summary>
        private static readonly List<Hl7.Fhir.Model.RelatedArtifact> _artifacts = new List<Hl7.Fhir.Model.RelatedArtifact>()
        {
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "Template for daily Hospital COVID-19 Reporting",
                Citation = _femaCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://github.com/AudaciousInquiry/saner-ig/blob/master/resources/Template%20for%20Daily%20Hospital%20COVID-19%20Reporting.xlsx",
                    Creation = "2020-03-29",
                },
            },
        };

        /// <summary>The current.</summary>
        public static DailyReporting Current = _current;

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name => "sanerFEMA";

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title => "SANER FEMA Template for daily Hospital COVID-19 Reporting";

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description => "SANER implementation of the FEMA Template for daily Hospital COVID-19 Reporting";

        /// <summary>Gets the fields.</summary>
        /// <value>The fields.</value>
        public Dictionary<string, FormatField> Fields => _fields;

        /// <summary>Gets the measure report fields.</summary>
        /// <value>The measure report fields.</value>
        public List<string> MeasureReportFields => _measureReportFields;

        /// <summary>Gets the questionnaire sections.</summary>
        /// <value>The questionnaire sections.</value>
        public List<QuestionnaireSection> QuestionnaireSections => _questionnaireSections;

        /// <summary>Gets the CSV fields.</summary>
        /// <value>The CSV fields.</value>
        public List<string> CsvFields => null;

        /// <summary>Gets the spreadsheet locations.</summary>
        /// <value>The spreadsheet locations.</value>
        public Dictionary<string, SpreadsheetLocation> SpreadsheetLocations => _spreadsheetLocations;

        /// <summary>Gets the artifacts.</summary>
        /// <value>The artifacts.</value>
        public List<Hl7.Fhir.Model.RelatedArtifact> Artifacts => _artifacts;
    }
}
