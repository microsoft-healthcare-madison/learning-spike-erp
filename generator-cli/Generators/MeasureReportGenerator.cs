// <copyright file="MeasureReportGenerator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Formats.CDC;
using covidReportTransformationLib.Formats.FEMA;
using covidReportTransformationLib.Formats.SANER;
using generator_cli.Models;
using Hl7.Fhir.Model;

namespace generator_cli.Generators
{
    /// <summary>A measure report generator.</summary>
    public class MeasureReportGenerator
    {
        private Organization _org;
        private Location _location;
        private OrgDeviceData _deviceData;
        private OrgPatientData _patientData;
        private OrgTestData _testData;
        private Period _period;
        private Dictionary<string, Score> _scoresByGroupCode = new Dictionary<string, Score>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureReportGenerator"/> class.
        /// </summary>
        /// <param name="org">        The organization.</param>
        /// <param name="location">   The location.</param>
        /// <param name="deviceData"> Information describing the device.</param>
        /// <param name="patientData">Information describing the patient.</param>
        /// <param name="testData">   Information describing the test.</param>
        /// <param name="period">     The period.</param>
        public MeasureReportGenerator(
            Organization org,
            Location location,
            OrgDeviceData deviceData,
            OrgPatientData patientData,
            OrgTestData testData,
            Period period)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            if (deviceData == null)
            {
                throw new ArgumentNullException(nameof(deviceData));
            }

            if (patientData == null)
            {
                throw new ArgumentNullException(nameof(patientData));
            }

            if (testData == null)
            {
                throw new ArgumentNullException(nameof(testData));
            }

            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            _org = org;
            _location = location;
            _deviceData = deviceData;
            _patientData = patientData;
            _testData = testData;
            _period = period;

            ComputeCdcScores();
            ComputeFemaScores();
        }

        /// <summary>Builds score dictionaries.</summary>
        private void ComputeCdcScores()
        {
            int val;

            _scoresByGroupCode.Add(AcutePatientImpact.TotalBeds, new Score(_deviceData.TotalBeds));

            _scoresByGroupCode.Add(AcutePatientImpact.InpatientBeds, new Score(_deviceData.Inpatient));

            val = Math.Min(_deviceData.Inpatient, _patientData.Total);
            _scoresByGroupCode.Add(AcutePatientImpact.InpatientBedOccupancy, new Score(val));

            _scoresByGroupCode.Add(AcutePatientImpact.IcuBeds, new Score(_deviceData.ICU));

            val = Math.Min(_deviceData.ICU, _patientData.NegativeNeedIcu + _patientData.PositiveNeedIcu);
            _scoresByGroupCode.Add(AcutePatientImpact.IcuBedOccupancy, new Score(val));

            _scoresByGroupCode.Add(AcutePatientImpact.Ventilators, new Score(_deviceData.Ventilators));

            val = Math.Min(_deviceData.Ventilators, _patientData.NegativeNeedVent + _patientData.PositiveNeedVent);
            _scoresByGroupCode.Add(AcutePatientImpact.VentilatorsInUse, new Score(val));

            val = Math.Min(_deviceData.TotalBeds - _patientData.Negative, _patientData.Positive);
            _scoresByGroupCode.Add(AcutePatientImpact.HospitalizedPatients, new Score(val));

            val = Math.Min(_deviceData.Ventilators - _patientData.NegativeNeedVent, _patientData.PositiveNeedVent);
            _scoresByGroupCode.Add(AcutePatientImpact.VentilatedPatients, new Score(val));

            _scoresByGroupCode.Add(AcutePatientImpact.HospitalOnset, new Score(_patientData.OnsetInCare));

            val = Math.Max(0, _patientData.Positive - (_deviceData.TotalBeds - _patientData.Negative));
            _scoresByGroupCode.Add(AcutePatientImpact.AwaitingBeds, new Score(val));

            val = Math.Max(0, _patientData.PositiveNeedVent - (_deviceData.Ventilators - _patientData.NegativeNeedVent));
            _scoresByGroupCode.Add(AcutePatientImpact.AwaitingVentilators, new Score(val));

            _scoresByGroupCode.Add(AcutePatientImpact.Died, new Score(_patientData.Died));
        }

        /// <summary>Calculates the fema scores.</summary>
        private void ComputeFemaScores()
        {
            decimal val;

            val = _testData.PerformedToday * 2;
            _scoresByGroupCode.Add(DailyReporting.TestsOrderedToday, new Score(val));

            val = _testData.Performed * 2;
            _scoresByGroupCode.Add(DailyReporting.TestsOrderedTotal, new Score(val));

            val = (_testData.Positive + _testData.Negative) * 2;
            _scoresByGroupCode.Add(DailyReporting.TestsWithResultsToday, new Score(val));

            _scoresByGroupCode.Add(DailyReporting.SpecimensRejectedTotal, new Score(_testData.Rejected));

            val = (_testData.Performed * 2) - _testData.Rejected - _testData.Pending;
            _scoresByGroupCode.Add(DailyReporting.TestsCompletedTotal, new Score(val));

            _scoresByGroupCode.Add(DailyReporting.PositiveC19Today, new Score(_testData.PositiveToday));

            _scoresByGroupCode.Add(DailyReporting.PositiveC19Total, new Score(_testData.Positive));

            _scoresByGroupCode.Add(
                DailyReporting.PercentC19PositiveToday,
                new Score(_testData.PositiveToday, _testData.PositiveToday * 2));

            _scoresByGroupCode.Add(
                DailyReporting.PercentC19PositiveTotal,
                new Score(_testData.Positive, _testData.Positive * 2));
        }

        /// <summary>Gets group report bundle.</summary>
        /// <returns>The group report bundle.</returns>
        public Bundle GetCdcCompleteReportBundle()
        {
            return GetReportBundleForMeasure(SanerMeasure.CDCPatientImpactMeasure());
        }

        /// <summary>Gets group report bundle.</summary>
        /// <returns>The group report bundle.</returns>
        public Bundle GetFemaCompleteReportBundle()
        {
            return GetReportBundleForMeasure(SanerMeasure.FEMADailyMeasure());
        }

        /// <summary>Gets bundle for measure.</summary>
        /// <param name="measure">The measure.</param>
        /// <returns>The bundle for measure.</returns>
        private Bundle GetReportBundleForMeasure(Measure measure)
        {
            string bundleId = FhirGenerator.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirGenerator.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            string id;

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                ReportForCompleteMeasure(
                    out id,
                    measure),
                $"{SystemLiterals.Internal}MeasureReport/{id}");

            return bundle;
        }

        /// <summary>Reports for cdc grouped measure.</summary>
        /// <param name="id">     [out] The identifier.</param>
        /// <param name="measure">The measure.</param>
        /// <returns>A MeasureReport.</returns>
        private MeasureReport ReportForCompleteMeasure(
            out string id,
            Measure measure)
        {
            id = FhirGenerator.NextId;

            MeasureReport report = new MeasureReport()
            {
                Id = id,
                Subject = new ResourceReference(
                    $"{_location.ResourceType}/{_location.Id}",
                    _org.Name),
                Status = MeasureReport.MeasureReportStatus.Complete,
                Type = MeasureReport.MeasureReportType.Summary,
                Date = new FhirDateTime(new DateTimeOffset(DateTime.Now)).ToString(),
                Period = _period,
                Measure = measure.Url,
                Reporter = new ResourceReference(
                    $"{_org.ResourceType}/{_org.Id}",
                    _org.Name),
                Group = new List<MeasureReport.GroupComponent>(),
            };

            foreach (Measure.GroupComponent measureGroup in measure.Group)
            {
                string groupName = measureGroup.Code.Coding[0].Code;

                if (!_scoresByGroupCode.ContainsKey(groupName))
                {
                    continue;
                }

                MeasureReport.GroupComponent reportGroup = new MeasureReport.GroupComponent()
                {
                    Code = measureGroup.Code,
                    MeasureScore = new Quantity() { Value = _scoresByGroupCode[groupName].MeasureScore, },
                    Population = new List<MeasureReport.PopulationComponent>(),
                    Stratifier = new List<MeasureReport.StratifierComponent>(),
                };

                foreach (Measure.PopulationComponent population in measureGroup.Population)
                {
                    if ((population.Code == null) ||
                        (population.Code.Coding == null) ||
                        (population.Code.Coding.Count == 0))
                    {
                        continue;
                    }

                    switch (population.Code.Coding[0].Code)
                    {
                        case "initial-population":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (int)_scoresByGroupCode[groupName].MeasureScore,
                            });
                            break;

                        case "measure-population":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (int)_scoresByGroupCode[groupName].MeasureScore,
                            });
                            break;

                        case "measure-observation":
                            // ignore
                            break;

                        case "numerator":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = _scoresByGroupCode[groupName].Numerator ?? (int)_scoresByGroupCode[groupName].MeasureScore,
                            });
                            break;

                        case "denominator":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = _scoresByGroupCode[groupName].Denominator ?? 1,
                            });
                            break;
                    }
                }

                if (reportGroup.Population.Count == 0)
                {
                    reportGroup.Population = null;
                }

                report.Group.Add(reportGroup);
            }

            return report;
        }

        /// <summary>Reports for measure.</summary>
        /// <param name="id">     [out] The identifier.</param>
        /// <param name="measure">The measure.</param>
        /// <param name="score">  The score.</param>
        /// <returns>A MeasureReport.</returns>
        private MeasureReport ReportForMeasure(
            out string id,
            Measure measure,
            Score score)
        {
            id = FhirGenerator.NextId;

            MeasureReport report = new MeasureReport()
            {
                Id = id,
                Subject = new ResourceReference(
                    $"{_location.ResourceType}/{_location.Id}",
                    _org.Name),
                Status = MeasureReport.MeasureReportStatus.Complete,
                Type = MeasureReport.MeasureReportType.Summary,
                Date = new FhirDateTime(new DateTimeOffset(DateTime.Now)).ToString(),
                Period = _period,
                Measure = measure.Url,
                Reporter = new ResourceReference(
                    $"{_org.ResourceType}/{_org.Id}",
                    _org.Name),
                Group = new List<MeasureReport.GroupComponent>()
                {
                    new MeasureReport.GroupComponent(),
                },
            };

            report.Group[0].MeasureScore = new Quantity() { Value = score.MeasureScore };
            report.Group[0].Population = new List<MeasureReport.PopulationComponent>();

            if (measure.Group[0].Population != null)
            {
                foreach (Measure.PopulationComponent population in measure.Group[0].Population)
                {
                    if ((population.Code == null) ||
                        (population.Code.Coding == null) ||
                        (population.Code.Coding.Count == 0))
                    {
                        continue;
                    }

                    switch (population.Code.Coding[0].Code)
                    {
                        case "measure-population":
                            report.Group[0].Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (int)score.MeasureScore,
                            });
                            break;

                        case "measure-observation":
                            // ignore
                            break;

                        case "numerator":
                            report.Group[0].Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = score.Numerator,
                            });
                            break;

                        case "denominator":
                            report.Group[0].Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = score.Denominator,
                            });
                            break;
                    }
                }
            }

            if (report.Group[0].Population.Count == 0)
            {
                report.Group[0].Population = null;
            }

            return report;
        }
    }
}
