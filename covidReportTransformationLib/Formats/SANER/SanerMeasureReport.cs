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
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="data">               The data.</param>
        /// <param name="includedFormatNames">List of names of the included formats, null or empty includes all.</param>
        /// <returns>The bundle.</returns>
        public static Bundle GetBundle(
            ReportData data,
            List<string> includedFormatNames)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            string bundleId = Utils.FhirIds.NextId;

            Bundle bundle = new Bundle()
            {
                Meta = new Meta()
                {
                    Profile = new string[]
                    {
                        FhirSystems.Bundle,
                    },
                },
                Id = bundleId,
                Identifier = Utils.FhirIds.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
            };

            string id;

            bundle.Entry = new List<Bundle.EntryComponent>();

            List<IReportingFormat> formats = FormatHelper.GetFormatList();

            foreach (IReportingFormat format in formats)
            {
                if ((includedFormatNames != null) && (includedFormatNames.Count > 0))
                {
                    if (includedFormatNames.Contains(format.Name))
                    {
                        continue;
                    }
                }

                if (!format.EnableMeasureReport)
                {
                    continue;
                }

                bundle.AddResourceEntry(
                    ReportForMeasure(
                        out id,
                        SanerMeasure.GetMeasure(format),
                        data),
                    $"{FhirSystems.Internal}MeasureReport/{id}");
            }

            return bundle;
        }

        #if false       // 2020.05.04 - changed primary format to dictionary
        /// <summary>Index cdc scores.</summary>
        /// <param name="scores">The scores.</param>
        /// <param name="data">  The data.</param>
        private static void IndexCdcScores(
            Dictionary<string, FieldValue> scores,
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
            Dictionary<string, FieldValue> scores,
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
        #endif

        /// <summary>Reports for cdc grouped measure.</summary>
        /// <param name="id">     [out] The identifier.</param>
        /// <param name="measure">The measure.</param>
        /// <param name="data">   The data.</param>
        /// <returns>A MeasureReport.</returns>
        private static MeasureReport ReportForMeasure(
            out string id,
            Measure measure,
            ReportData data)
        {
            id = FhirIds.NextId;

            MeasureReport report = new MeasureReport()
            {
                Meta = new Meta()
                {
                    Profile = new string[]
                    {
                        FhirSystems.MeasureReport,
                    },
                    Security = FhirTriplet.SecurityTest.GetCodingList(),
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
                report.Subject = new ResourceReference(
                    $"Location/{data.CoveredLocation.Id}",
                    data.CoveredLocation.Name);

#if false // 2020.05.05 - figure out if we still want contained
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
#endif
            }

            if (data.Reporter != null)
            {
                report.Reporter = new ResourceReference(
                    $"Organization/{data.Reporter.Id}",
                    data.Reporter.Name);

#if false // 2020.05.05 - figure out if we still want contained
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
#endif
            }

            if ((data.Reporter == null) && (data.CoveredOrganization != null))
            {
                report.Reporter = new ResourceReference(
                    $"Organization/{data.CoveredOrganization.Id}",
                    data.CoveredOrganization.Name);

#if false // 2020.05.05 - figure out if we still want contained
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
#endif
            }

            foreach (Measure.GroupComponent measureGroup in measure.Group)
            {
                if ((measureGroup.Code == null) ||
                    (measureGroup.Code.Coding == null) ||
                    (measureGroup.Code.Coding.Count == 0))
                {
                    continue;
                }

                string groupName = measureGroup.Code.Coding[0].Code;

                MeasureReport.GroupComponent reportGroup = new MeasureReport.GroupComponent()
                {
                    Code = measureGroup.Code,
                    Population = new List<MeasureReport.PopulationComponent>(),
                    Stratifier = new List<MeasureReport.StratifierComponent>(),
                };

                if ((measureGroup.Population == null) || (measureGroup.Population.Count == 0))
                {
                    if (data.Values.ContainsKey(groupName))
                    {
                        reportGroup.MeasureScore = new Quantity()
                        {
                            Value = data.Values[groupName].Score,
                        };
                    }

                    AddGroupToReport(report, reportGroup);

                    continue;
                }

                foreach (Measure.PopulationComponent population in measureGroup.Population)
                {
                    if ((population.Code == null) ||
                        (population.Code.Coding == null) ||
                        (population.Code.Coding.Count == 0))
                    {
                        continue;
                    }

                    string populationSystem = null;
                    string populationName = null;
                    string populationCode = null;

                    foreach (Coding popCoding in population.Code.Coding)
                    {
                        if ((popCoding.System == FhirSystems.MeasurePopulation) ||
                            (popCoding.System == FhirSystems.SanerAggregateBool) ||
                            (popCoding.System == FhirSystems.SanerAggregateChoice))
                        {
                            populationSystem = popCoding.System;
                            populationCode = popCoding.Code;
                        }

                        if (data.Values.ContainsKey(popCoding.Code))
                        {
                            populationName = popCoding.Code;
                        }
                    }

                    if (populationSystem == FhirSystems.SanerAggregateBool)
                    {
                        if (populationCode == "true")
                        {
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (data.Values[groupName].BoolValue == true) ? 1 : 0,
                            });
                        }
                        else
                        {
                            {
                                reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                                {
                                    Code = population.Code,
                                    Count = (data.Values[groupName].BoolValue == false) ? 1 : 0,
                                });
                            }
                        }

                        continue;
                    }

                    if ((populationCode == FhirSystems.MeasurePopulation) &&
                        (!string.IsNullOrEmpty(populationName)))
                    {
                        switch (populationCode)
                        {
                            case "initial-population":
                                reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                                {
                                    Code = population.Code,
                                    Count = (int)data.Values[populationName].Score,
                                });
                                break;

                            case "measure-population":
                                reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                                {
                                    Code = population.Code,
                                    Count = (int)data.Values[populationName].Score,
                                });
                                break;

                            case "measure-observation":
                                // ignore
                                break;

                            case "numerator":
                                reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                                {
                                    Code = population.Code,
                                    Count = data.Values[populationName].Numerator ?? (int)data.Values[populationName].Score,
                                });
                                break;

                            case "denominator":
                                reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                                {
                                    Code = population.Code,
                                    Count = data.Values[populationName].Denominator ?? (int?)data.Values[populationName].Score ?? 1,
                                });
                                break;
                        }
                    }
                }

                AddGroupToReport(report, reportGroup);
            }

            return report;
        }

        /// <summary>Adds a group to report to 'reportGroup'.</summary>
        /// <param name="report">     The report.</param>
        /// <param name="reportGroup">Group the report belongs to.</param>
        private static void AddGroupToReport(
            MeasureReport report,
            MeasureReport.GroupComponent reportGroup)
        {
            if (reportGroup.Population.Count == 0)
            {
                reportGroup.Population = null;
            }

            if (reportGroup.Stratifier.Count == 0)
            {
                reportGroup.Stratifier = null;
            }

            report.Group.Add(reportGroup);
        }
    }
}
