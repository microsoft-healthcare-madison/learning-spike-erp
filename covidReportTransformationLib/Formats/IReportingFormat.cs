// <copyright file="IReportingFormat.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>Interface for format.</summary>
    public interface IReportingFormat
    {
        /// <summary>Gets a value indicating whether the measure report is enabled.</summary>
        /// <value>True if enable measure report, false if not.</value>
        bool EnableMeasureReport { get; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        string Name
        {
            get;
        }

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        string Title
        {
            get;
        }

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        string Description
        {
            get;
        }

        /// <summary>Gets the definition.</summary>
        /// <value>The definition.</value>
        List<string> Definition
        {
            get;
        }

        /// <summary>Gets the fields.</summary>
        /// <value>The fields.</value>
        Dictionary<string, FormatField> Fields
        {
            get;
        }

        /// <summary>Gets the measure groupings.</summary>
        /// <value>The measure groupings.</value>
        List<MeasureGrouping> MeasureGroupings
        {
            get;
        }

        /// <summary>Gets the questionnaire sections.</summary>
        /// <value>The questionnaire sections.</value>
        List<QuestionnaireSection> QuestionnaireSections
        {
            get;
        }

        /// <summary>Gets the CSV fields.</summary>
        /// <value>The CSV fields.</value>
        List<string> CsvFields
        {
            get;
        }

        /// <summary>Gets the spreadsheet locations.</summary>
        /// <value>The spreadsheet locations.</value>
        Dictionary<string, SpreadsheetLocation> SpreadsheetLocations
        {
            get;
        }

        /// <summary>Gets the artifacts.</summary>
        /// <value>The artifacts.</value>
        List<Hl7.Fhir.Model.RelatedArtifact> Artifacts
        {
            get;
        }

        /// <summary>Gets the authors.</summary>
        /// <value>The authors.</value>
        List<Hl7.Fhir.Model.ContactDetail> Authors
        {
            get;
        }
    }
}
