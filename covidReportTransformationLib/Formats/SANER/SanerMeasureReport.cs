// <copyright file="SanerMeasureReport.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Formats.CDC;
using covidReportTransformationLib.Formats.FEMA;
using covidReportTransformationLib.Models;
using covidReportTransformationLib.Utils;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Formats.SANER
{
    /// <summary>A measure report generator.</summary>
    public abstract class SanerMeasureReport
    {
        /// <summary>Gets a bundle.</summary>
        /// <param name="data">       The data.</param>
        /// <param name="includeCdc"> True to include, false to exclude the cdc.</param>
        /// <param name="includeFema">True to include, false to exclude the fema.</param>
        /// <returns>The bundle.</returns>
        public static Bundle GetBundle(
            ReportData data,
            bool includeCdc,
            bool includeFema)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Dictionary<string, Score> scores = new Dictionary<string, Score>();

            IndexCdcScores(scores, data);
            IndexFemaScores(scores, data);

            string bundleId = Utils.FhirIds.NextId;

            Bundle bundle = new Bundle()
            {
                Meta = new Meta()
                {
                    Profile = new string[]
                    {
                        "http://hl7.org/fhir/4.0/StructureDefinition/Bundle",
                    },
                },
                Id = bundleId,
                Identifier = Utils.FhirIds.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
            };

            string id;

            bundle.Entry = new List<Bundle.EntryComponent>();

            if (includeCdc)
            {
                bundle.AddResourceEntry(
                    ReportForMeasure(
                        out id,
                        SanerMeasure.CDCPatientImpactMeasure(),
                        data,
                        scores),
                    $"{FhirSystems.Internal}MeasureReport/{id}");
            }

            if (includeFema)
            {
                bundle.AddResourceEntry(
                    ReportForMeasure(
                        out id,
                        SanerMeasure.FEMADailyMeasure(),
                        data,
                        scores),
                    $"{FhirSystems.Internal}MeasureReport/{id}");
            }

            return bundle;
        }

        /// <summary>Adds a score.</summary>
        /// <param name="scores">The scores.</param>
        /// <param name="field"> The field.</param>
        /// <param name="value"> The value.</param>
        private static void AddScore(Dictionary<string, Score> scores, string field, decimal? value)
        {
            if (value == null)
            {
                return;
            }

            if (scores.ContainsKey(field))
            {
                scores[field] = new Score((decimal)value);
                return;
            }

            scores.Add(field, new Score((decimal)value));
        }

        /// <summary>Adds a score.</summary>
        /// <param name="scores">     The scores.</param>
        /// <param name="field">      The field.</param>
        /// <param name="numerator">  The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        private static void AddScore(Dictionary<string, Score> scores, string field, int? numerator, int? denominator)
        {
            if ((numerator == null) || (denominator == null))
            {
                return;
            }

            if (scores.ContainsKey(field))
            {
                scores[field] = new Score((int)numerator, (int)denominator);
                return;
            }

            scores.Add(field, new Score((int)numerator, (int)denominator));
        }

        /// <summary>Index cdc scores.</summary>
        /// <param name="scores">The scores.</param>
        /// <param name="data">  The data.</param>
        private static void IndexCdcScores(
            Dictionary<string, Score> scores,
            ReportData data)
        {
            AddScore(scores, AcutePatientImpact.TotalBeds, data.BedsTotal);
            AddScore(scores, AcutePatientImpact.InpatientBeds, data.BedsInpatientTotal);
            AddScore(scores, AcutePatientImpact.InpatientBedOccupancy, data.BedsInpatientInUse);
            AddScore(scores, AcutePatientImpact.IcuBeds, data.BedsIcuTotal);
            AddScore(scores, AcutePatientImpact.IcuBedOccupancy, data.BedsIcuInUse);
            AddScore(scores, AcutePatientImpact.Ventilators, data.VentilatorsTotal);
            AddScore(scores, AcutePatientImpact.VentilatorsInUse, data.VentilatorsInUse);
            AddScore(scores, AcutePatientImpact.HospitalizedPatients, data.PatientsC19Hospitalized);
            AddScore(scores, AcutePatientImpact.VentilatedPatients, data.PatientsC19Ventilated);
            AddScore(scores, AcutePatientImpact.HospitalOnset, data.PatientsC19HospitalOnsetTotal);
            AddScore(scores, AcutePatientImpact.AwaitingBeds, data.PatientsC19NeedBed);
            AddScore(scores, AcutePatientImpact.AwaitingVentilators, data.PatientsC19NeedVent);
            AddScore(scores, AcutePatientImpact.Died, data.PatientsC19DiedTotal);
        }

        /// <summary>Index fema scores.</summary>
        /// <param name="scores">The scores.</param>
        /// <param name="data">  The data.</param>
        private static void IndexFemaScores(
            Dictionary<string, Score> scores,
            ReportData data)
        {
            AddScore(scores, DailyReporting.TestsOrderedToday, data.TestsPerformedToday);
            AddScore(scores, DailyReporting.TestsOrderedTotal, data.TestsPerformedTotal);
            AddScore(scores, DailyReporting.TestsWithResultsToday, data.TestsResultedToday);
            AddScore(scores, DailyReporting.SpecimensRejectedTotal, data.TestsRejectedTotal);
            AddScore(scores, DailyReporting.TestsCompletedTotal, data.TestsResultedTotal);
            AddScore(scores, DailyReporting.PositiveC19Today, data.C19TestsPositiveToday);
            AddScore(scores, DailyReporting.PositiveC19Total, data.C19TestsPositiveTotal);

            AddScore(
                scores,
                DailyReporting.PercentC19PositiveToday,
                data.C19TestsPositiveToday,
                data.TestsResultedToday);

            AddScore(
                scores,
                DailyReporting.PercentC19PositiveTotal,
                data.C19TestsPositiveTotal,
                data.TestsResultedTotal);
        }

        /// <summary>Reports for cdc grouped measure.</summary>
        /// <param name="id">     [out] The identifier.</param>
        /// <param name="measure">The measure.</param>
        /// <param name="data">   The data.</param>
        /// <returns>A MeasureReport.</returns>
        private static MeasureReport ReportForMeasure(
            out string id,
            Measure measure,
            ReportData data,
            Dictionary<string, Score> scores)
        {
            id = FhirIds.NextId;

            MeasureReport report = new MeasureReport()
            {
                Meta = new Meta()
                {
                    Profile = new string[]
                    {
                        "http://hl7.org/fhir/4.0/StructureDefinition/MeasureReport",
                    },
                },
                Id = id,
                Status = MeasureReport.MeasureReportStatus.Complete,
                Type = MeasureReport.MeasureReportType.Summary,
                Date = new FhirDateTime(new DateTimeOffset(DateTime.Now)).ToString(),
                Period = data.FhirPeriod(),
                Measure = measure.Url,
                Group = new List<MeasureReport.GroupComponent>(),
            };

            if (data.ReportDate != null)
            {
                report.Date = new FhirDateTime(new DateTimeOffset((DateTime)data.ReportDate!)).ToString();
            }

            if (data.CoveredLocation != null)
            {
                data.CoveredLocation.ToFhir(
                    out ResourceReference resourceReference,
                    out Resource contained);

                report.Subject = resourceReference;

                if (contained != null)
                {
                    if (report.Contained == null)
                    {
                        report.Contained = new List<Resource>();
                    }

                    report.Contained.Add(contained);
                }
            }

            if (data.Reporter != null)
            {
                data.Reporter.ToFhir(
                    out ResourceReference resourceReference,
                    out Resource contained);

                report.Reporter = resourceReference;

                if (contained != null)
                {
                    if (report.Contained == null)
                    {
                        report.Contained = new List<Resource>();
                    }

                    report.Contained.Add(contained);
                }
            }

            if ((data.Reporter == null) && (data.CoveredOrganization != null))
            {
                data.CoveredOrganization.ToFhir(
                    out ResourceReference resourceReference,
                    out Resource contained);

                report.Reporter = resourceReference;

                if (contained != null)
                {
                    if (report.Contained == null)
                    {
                        report.Contained = new List<Resource>();
                    }

                    report.Contained.Add(contained);
                }
            }

            foreach (Measure.GroupComponent measureGroup in measure.Group)
            {
                string groupName = measureGroup.Code.Coding[0].Code;

                if (!scores.ContainsKey(groupName))
                {
                    report.Group.Add(new MeasureReport.GroupComponent()
                        {
                            Code = measureGroup.Code,
                        });

                    continue;
                }

                MeasureReport.GroupComponent reportGroup = new MeasureReport.GroupComponent()
                {
                    Code = measureGroup.Code,
                    MeasureScore = new Quantity() { Value = scores[groupName].MeasureScore, },
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
                                Count = (int)scores[groupName].MeasureScore,
                            });
                            break;

                        case "measure-population":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (int)scores[groupName].MeasureScore,
                            });
                            break;

                        case "measure-observation":
                            // ignore
                            break;

                        case "numerator":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = scores[groupName].Numerator ?? (int)scores[groupName].MeasureScore,
                            });
                            break;

                        case "denominator":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = scores[groupName].Denominator ?? 1,
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
    }
}
