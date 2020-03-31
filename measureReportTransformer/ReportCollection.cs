// <copyright file="ReportCollection.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;
using measureReportTransformer.CDC;

namespace measureReportTransformer
{
    /// <summary>Collection of reports.</summary>
    public class ReportCollection
    {
        private readonly string _orgRef;

        private Dictionary<string, MeasureReport> _reportById = new Dictionary<string, MeasureReport>();
        private Dictionary<string, MeasureReport> _reportByKey = new Dictionary<string, MeasureReport>();

        private HashSet<string> _periods = new HashSet<string>();

        /// <summary>Initializes a new instance of the <see cref="ReportCollection"/> class.</summary>
        /// <param name="orgRef">Identifier for the organization.</param>
        public ReportCollection(string orgRef)
        {
            _orgRef = orgRef;
        }

        /// <summary>Gets information describing the cdc.</summary>
        /// <value>Information describing the cdc.</value>
        public List<CdcModel> CdcData
        {
            get
            {
                List<CdcModel> cdcList = new List<CdcModel>();

                List<string> periods = _periods.ToList();
                periods.Sort();

                foreach (string period in periods)
                {
                    cdcList.Add(GetCdcModel(period));
                }

                return cdcList;
            }
        }

        /// <summary>Gets cdc model.</summary>
        /// <param name="period">The date.</param>
        /// <returns>The cdc model.</returns>
        private CdcModel GetCdcModel(string period)
        {
            DateTime periodDateTime = DateTime.ParseExact(period, "yyyy-MM-dd", null);

            return new CdcModel(
                periodDateTime.ToString("MM/dd/yyyy", null),
                GetIntCdc(period, CdcLiterals.CDCTotalBeds),
                (int)GetIntCdc(period, CdcLiterals.CDCInpatientBeds),
                GetIntCdc(period, CdcLiterals.CDCInpatientBedOccupancy),
                GetIntCdc(period, CdcLiterals.CDCIcuBeds),
                GetIntCdc(period, CdcLiterals.CDCIcuBedOccupancy),
                GetIntCdc(period, CdcLiterals.CDCVentilators),
                GetIntCdc(period, CdcLiterals.CDCVentilatorsInUse),
                GetIntCdc(period, CdcLiterals.CDCHospitalizedPatients),
                GetIntCdc(period, CdcLiterals.CDCVentilatedPatients),
                GetIntCdc(period, CdcLiterals.CDCHospitalOnset),
                GetIntCdc(period, CdcLiterals.CDCAwaitingBeds),
                GetIntCdc(period, CdcLiterals.CDCAwaitingVentilators),
                GetIntCdc(period, CdcLiterals.CDCDied));
        }

        /// <summary>Value for identifier.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="id">The identifier.</param>
        /// <returns>A decimal?</returns>
        private decimal? GetValue(string id)
        {
            if ((!_reportById.ContainsKey(id)) ||
                (_reportById[id].Group == null) ||
                (_reportById[id].Group.Count == 0) ||
                (_reportById[id].Group[0] == null))
            {
                return null;
            }

            return _reportById[id].Group[0].MeasureScore.Value;
        }

        /// <summary>Value for identifier.</summary>
        /// <param name="period">    The date.</param>
        /// <param name="cdcField">The cdc field.</param>
        /// <returns>A decimal?</returns>
        private int? GetIntCdc(string period, string cdcField)
        {
            string key = $"{period}{CdcLiterals.Canonical(cdcField)}";

            if ((!_reportByKey.ContainsKey(key)) ||
                (_reportByKey[key].Group == null) ||
                (_reportByKey[key].Group.Count == 0) ||
                (_reportByKey[key].Group[0] == null))
            {
                return null;
            }

            return (int?)_reportByKey[key].Group[0].MeasureScore.Value;
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

            if (!_periods.Contains(report.Period.End))
            {
                _periods.Add(report.Period.End);
            }

            _reportById.Add(report.Id, report);
            _reportByKey.Add(GetKey(report), report);
        }

        /// <summary>Gets a key.</summary>
        /// <param name="report">The report.</param>
        /// <returns>The key.</returns>
        private static string GetKey(MeasureReport report)
        {
            return $"{report.Period.End}{report.Measure}";
        }
    }
}
