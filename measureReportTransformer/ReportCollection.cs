// <copyright file="ReportCollection.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace measureReportTransformer
{
    /// <summary>Collection of reports.</summary>
    public class ReportCollection
    {
        private readonly string _orgRef;

        private Dictionary<string, MeasureReport> _reportById = new Dictionary<string, MeasureReport>();
        private Dictionary<string, List<MeasureReport>> _reportsByPeriodEnding = new Dictionary<string, List<MeasureReport>>();
        private Dictionary<string, List<MeasureReport>> _reportsByMeasure = new Dictionary<string, List<MeasureReport>>();

        /// <summary>Initializes a new instance of the <see cref="ReportCollection"/> class.</summary>
        /// <param name="orgRef">Identifier for the organization.</param>
        public ReportCollection(string orgRef)
        {
            _orgRef = orgRef;
        }

        /// <summary>Adds a report.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///  illegal values.</exception>
        /// <param name="report">The report.</param>
        public void AddReport(MeasureReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            if (report.Reporter.Reference != _orgRef)
            {
                throw new ArgumentException($"Invalid Reporter: {report.Reporter} != collection org: {_orgRef}");
            }

            if (_reportById.ContainsKey(report.Id))
            {
                return;
            }

            _reportById.Add(report.Id, report);

            string periodEnds = report.Period.End;
            if (!_reportsByPeriodEnding.ContainsKey(periodEnds))
            {
                _reportsByPeriodEnding.Add(periodEnds, new List<MeasureReport>());
            }

            _reportsByPeriodEnding[periodEnds].Add(report);

            string measure = report.Measure;
            if (!_reportsByMeasure.ContainsKey(measure))
            {
                _reportsByMeasure.Add(measure, new List<MeasureReport>());
            }

            _reportsByMeasure[measure].Add(report);
        }
    }
}
