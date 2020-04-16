// <copyright file="SanerMeasure.cs" company="Microsoft Corporation">
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
    /// <summary>A SANER measure generator.</summary>
    public static class SanerMeasure
    {
        /// <summary>The measure version.</summary>
        private const string MeasureVersion = "20200408.03";

        /// <summary>The publication date.</summary>
        private const string PublicationDate = "2020-04-08T00:00:00Z";

        /// <summary>The publisher.</summary>
        private const string Publisher = "HL7 SANER-IG";

        /// <summary>The cdc grouped measures.</summary>
        private static Measure _cdcCompleteMeasure = null;

        /// <summary>The cdc measures.</summary>
        private static Dictionary<string, Measure> _cdcMeasures = new Dictionary<string, Measure>();

        /// <summary>The cdc measures.</summary>
        private static readonly Dictionary<string, SanerMeasureInfo> _cdcMeasureInfoByName = new Dictionary<string, SanerMeasureInfo>()
        {
            {
                CdcLiterals.TotalBeds,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.TotalBeds,
                    CdcLiterals.TotalBedsTitle,
                    CdcLiterals.TotalBedsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.InpatientBeds,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.InpatientBeds,
                    CdcLiterals.InpatientBedsTitle,
                    CdcLiterals.InpatientBedsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.InpatientBedOccupancy,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.InpatientBedOccupancy,
                    CdcLiterals.InpatientBedOccupancyTitle,
                    CdcLiterals.InpatientBedOccupancyDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.IcuBeds,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.IcuBeds,
                    CdcLiterals.IcuBedsTitle,
                    CdcLiterals.IcuBedsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.IcuBedOccupancy,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.IcuBedOccupancy,
                    CdcLiterals.IcuBedOccupancyTitle,
                    CdcLiterals.IcuBedOccupancyDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.Ventilators,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.Ventilators,
                    CdcLiterals.VentilatorsTitle,
                    CdcLiterals.VentilatorsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.VentilatorsInUse,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.VentilatorsInUse,
                    CdcLiterals.VentilatorsInUseTitle,
                    CdcLiterals.VentilatorsInUseDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.HospitalizedPatients,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.HospitalizedPatients,
                    CdcLiterals.HospitalizedPatientsTitle,
                    CdcLiterals.HospitalizedPatientsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.VentilatedPatients,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.VentilatedPatients,
                    CdcLiterals.VentilatedPatientsTitle,
                    CdcLiterals.VentilatedPatientsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.HospitalOnset,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.HospitalOnset,
                    CdcLiterals.HospitalOnsetTitle,
                    CdcLiterals.HospitalOnsetDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.AwaitingBeds,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.AwaitingBeds,
                    CdcLiterals.AwaitingBedsTitle,
                    CdcLiterals.AwaitingBedsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.AwaitingVentilators,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.AwaitingVentilators,
                    CdcLiterals.AwaitingVentilatorsTitle,
                    CdcLiterals.AwaitingVentilatorsDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                CdcLiterals.Died,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.CDC,
                    CdcLiterals.Died,
                    CdcLiterals.DiedTitle,
                    CdcLiterals.DiedDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
        };

        /// <summary>The fema grouped measures.</summary>
        private static Measure _femaCompleteMeasure = null;

        /// <summary>The fema measures.</summary>
        private static Dictionary<string, Measure> _femaMeasures = new Dictionary<string, Measure>();

        /// <summary>Information describing the fema measure.</summary>
        private static readonly Dictionary<string, SanerMeasureInfo> _femaMeasureInfoByName = new Dictionary<string, SanerMeasureInfo>()
        {
            {
                FemaLiterals.TestsOrderedToday,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.TestsOrderedToday,
                    FemaLiterals.TestsOrderedTodayTitle,
                    FemaLiterals.TestsOrderedTodayDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.TestsOrderedTotal,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.TestsOrderedTotal,
                    FemaLiterals.TestsOrderedTotalTitle,
                    FemaLiterals.TestsOrderedTotalDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.TestsWithResultsToday,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.TestsWithResultsToday,
                    FemaLiterals.TestsWithResultsTodayTitle,
                    FemaLiterals.TestsWithResultsTodayDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.SpecimensRejectedTotal,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.SpecimensRejectedTotal,
                    FemaLiterals.SpecimensRejectedTotalTitle,
                    FemaLiterals.SpecimensRejectedTotalDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.TestsCompletedTotal,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.TestsCompletedTotal,
                    FemaLiterals.TestsCompletedTotalTitle,
                    FemaLiterals.TestsCompletedTotalDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.PositiveC19Today,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.PositiveC19Today,
                    FemaLiterals.PositiveC19TodayTitle,
                    FemaLiterals.PositiveC19TodayDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.PositiveC19Total,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.PositiveC19Total,
                    FemaLiterals.PositiveC19TotalTitle,
                    FemaLiterals.PositiveC19TotalDesc,
                    FhirTriplet.MeasureTypeStructure,
                    SanerMeasureInfo.MeasureStyle.Count)
            },
            {
                FemaLiterals.PercentC19PositiveToday,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.PercentC19PositiveToday,
                    FemaLiterals.PercentC19PositiveTodayTitle,
                    FemaLiterals.PercentC19PositiveTodayDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Ratio)
            },
            {
                FemaLiterals.PercentC19PositiveTotal,
                new SanerMeasureInfo(
                    SanerMeasureInfo.MeasureSource.FEMA,
                    FemaLiterals.PercentC19PositiveTotal,
                    FemaLiterals.PercentC19PositiveTotalTitle,
                    FemaLiterals.PercentC19PositiveTotalDesc,
                    FhirTriplet.MeasureTypeOutcome,
                    SanerMeasureInfo.MeasureStyle.Ratio)
            },
        };

        /// <summary>The other measures.</summary>
        private static readonly List<SanerMeasureInfo> _otherMeasureInfo = new List<SanerMeasureInfo>()
        {
            new SanerMeasureInfo(
                SanerMeasureInfo.MeasureSource.SANER,
                "covid-19-test-count",
                "COVID-19 Tests Performed",
                "The total number of patients for whom a test for COVID-19 was ordered.",
                FhirTriplet.MeasureTypeOutcome,
                SanerMeasureInfo.MeasureStyle.Count),

            new SanerMeasureInfo(
                SanerMeasureInfo.MeasureSource.SANER,
                "covid-19-test-positive-count",
                "COVID-19 Positive Tests",
                "The total number of patients for whom a positive result for a COVID-19 test was documented.",
                FhirTriplet.MeasureTypeOutcome,
                SanerMeasureInfo.MeasureStyle.Count),

            new SanerMeasureInfo(
                SanerMeasureInfo.MeasureSource.SANER,
                "covid-19-test-positive-count",
                "COVID-19 Positive Tests",
                "The total number of patients for whom a positive result for a COVID-19 test was documented.",
                FhirTriplet.MeasureTypeOutcome,
                SanerMeasureInfo.MeasureStyle.Count),
        };

        /// <summary>Builds cdc complete measure.</summary>
        /// <returns>A Measure.</returns>
        private static Measure BuildCdcCompleteMeasure()
        {
            Measure measure = new Measure()
            {
                Id = "sanerCDC",
                Name = "sanerCDC",
                Url = $"{SanerMeasureInfo.CdcCanonicalUrl}/sanerCDC",
                Version = MeasureVersion,
                Title = "CDC Measurement Group",
                Status = PublicationStatus.Draft,
                Subject = new CodeableConcept("Location", "Location"),
                Date = PublicationDate,
                Publisher = Publisher,
                Description = new Markdown("CDC Measurement Group"),
                Jurisdiction = new List<CodeableConcept>()
                {
                    FhirTriplet.UnitedStates.Concept,
                },
                UseContext = new List<UsageContext>()
                {
                    new UsageContext()
                    {
                        Code = FhirTriplet.GetCode(FhirSystems.UsageContextType, CommonLiterals.ContextFocus),
                        Value = FhirTriplet.SctCovid.Concept,
                    },
                },
                Type = new List<CodeableConcept>()
                {
                    FhirTriplet.MeasureTypeComposite.Concept,
                },
                RelatedArtifact = new List<RelatedArtifact>(),
                Group = new List<Measure.GroupComponent>(),
            };

            measure.RelatedArtifact.AddRange(SanerMeasureInfo.CdcArtifacts);

            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.TotalBeds].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.InpatientBeds].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.InpatientBedOccupancy].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.IcuBeds].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.IcuBedOccupancy].MeasureGroupCohort);

            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.Ventilators].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.VentilatorsInUse].MeasureGroupCohort);

            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.HospitalizedPatients].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.VentilatedPatients].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.HospitalOnset].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.AwaitingBeds].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.AwaitingVentilators].MeasureGroupCohort);
            measure.Group.Add(_cdcMeasureInfoByName[CdcLiterals.Died].MeasureGroupCohort);

            return measure;
        }

        /// <summary>Builds fema complete measure.</summary>
        /// <returns>A Measure.</returns>
        private static Measure BuildFemaCompleteMeasure()
        {
            Measure measure = new Measure()
            {
                Id = "sanerFEMA",
                Name = "sanerFEMA",
                Url = $"{SanerMeasureInfo.CdcCanonicalUrl}/sanerFEMA",
                Version = MeasureVersion,
                Title = "FEMA Measurement Group",
                Status = PublicationStatus.Draft,
                Subject = new CodeableConcept("Location", "Location"),
                Date = PublicationDate,
                Publisher = Publisher,
                Description = new Markdown("FEMA Measurement Group"),
                Jurisdiction = new List<CodeableConcept>()
                {
                    FhirTriplet.UnitedStates.Concept,
                },
                UseContext = new List<UsageContext>()
                {
                    new UsageContext()
                    {
                        Code = FhirTriplet.GetCode(FhirSystems.UsageContextType, CommonLiterals.ContextFocus),
                        Value = FhirTriplet.SctCovid.Concept,
                    },
                },
                RelatedArtifact = new List<RelatedArtifact>(),
                Group = new List<Measure.GroupComponent>(),
                Type = new List<CodeableConcept>()
                {
                    FhirTriplet.MeasureTypeComposite.Concept,
                },
            };

            measure.RelatedArtifact.AddRange(SanerMeasureInfo.FemaArtifacts);

            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.TestsOrderedToday].MeasureGroupCohort);
            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.TestsOrderedTotal].MeasureGroupCohort);
            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.TestsWithResultsToday].MeasureGroupCohort);
            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.SpecimensRejectedTotal].MeasureGroupCohort);
            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.TestsCompletedTotal].MeasureGroupCohort);

            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.PositiveC19Today].MeasureGroupCohort);
            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.PositiveC19Total].MeasureGroupCohort);

            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.PercentC19PositiveToday].MeasureGroupProportion);
            measure.Group.Add(_femaMeasureInfoByName[FemaLiterals.PercentC19PositiveTotal].MeasureGroupProportion);

            return measure;
        }

        /// <summary>Builds a measure.</summary>
        /// <param name="info">The identifier.</param>
        /// <returns>A Measure.</returns>
        private static Measure BuildMeasure(
            SanerMeasureInfo info)
        {
            Measure measure = new Measure()
            {
                Id = info.Name,
                Name = info.Name,
                Url = $"{info.Canonical}/{info.Name}",
                Version = MeasureVersion,
                Title = info.Title,
                Status = PublicationStatus.Draft,
                Subject = new CodeableConcept("Location", "Location"),
                Date = PublicationDate,
                Publisher = Publisher,
                Description = new Markdown(info.Description),
                Jurisdiction = new List<CodeableConcept>()
                {
                    FhirTriplet.UnitedStates.Concept,
                },
                UseContext = new List<UsageContext>()
                {
                    new UsageContext()
                    {
                        Code = FhirTriplet.GetCode(FhirSystems.UsageContextType, CommonLiterals.ContextFocus),
                        Value = FhirTriplet.SctCovid.Concept,
                    },
                },
            };

            if (info.MeasureType != null)
            {
                measure.Type = new List<CodeableConcept>()
                {
                    info.MeasureType.Concept,
                };
            }

            if ((info.Artifacts != null) && (info.Artifacts.Count > 0))
            {
                measure.RelatedArtifact = info.Artifacts;
            }

            measure.Group = new List<Measure.GroupComponent>()
            {
                new Measure.GroupComponent()
                {
                    Population = new List<Measure.PopulationComponent>()
                    {
                        new Measure.PopulationComponent()
                        {
                            Code = FhirTriplet.InitialPopulation.Concept,
                            Criteria = new Expression()
                            {
                                Description = info.CriteriaDescription,
                                Language = "text/plain",
                                Expression_ = info.Description,
                            },
                        },
                    },
                },
            };

            // change from continuous variable to cohort since we are always counting things
            measure.Scoring = FhirTriplet.ScoringCohort.Concept;

            return measure;
        }

        /// <summary>Initializes this object.</summary>
        public static void Init()
        {
            // build CDC measures
            foreach (SanerMeasureInfo info in _cdcMeasureInfoByName.Values)
            {
                _cdcMeasures.Add(info.Name, BuildMeasure(info));
            }

            _cdcCompleteMeasure = BuildCdcCompleteMeasure();

            // build FEMA measures
            foreach (SanerMeasureInfo info in _femaMeasureInfoByName.Values)
            {
                _femaMeasures.Add(info.Name, BuildMeasure(info));
            }

            _femaCompleteMeasure = BuildFemaCompleteMeasure();
        }

        /// <summary>Cdc measure.</summary>
        /// <param name="name">The name.</param>
        /// <returns>A Measure.</returns>
        public static Measure CDCMeasure(string name)
        {
            if (!_cdcMeasures.ContainsKey(name))
            {
                return null;
            }

            return _cdcMeasures[name];
        }

        /// <summary>Cdc grouped measure.</summary>
        /// <returns>A Measure.</returns>
        public static Measure CDCCompleteMeasure()
        {
            if (_cdcCompleteMeasure == null)
            {
                Init();
            }

            return _cdcCompleteMeasure;
        }

        /// <summary>Fema measure.</summary>
        /// <param name="name">The name.</param>
        /// <returns>A Measure.</returns>
        public static Measure FemaMeasure(string name)
        {
            if (!_femaMeasures.ContainsKey(name))
            {
                return null;
            }

            return _femaMeasures[name];
        }

        /// <summary>Fema complete measure.</summary>
        /// <returns>A Measure.</returns>
        public static Measure FemaCompleteMeasure()
        {
            if (_femaCompleteMeasure == null)
            {
                Init();
            }

            return _femaCompleteMeasure;
        }

        /// <summary>Gets report bundle.</summary>
        /// <returns>The report bundle.</returns>
        public static Bundle GetCdcMeasureBundle()
        {
            return GetBundleForMeasure(_cdcCompleteMeasure, "sanerCDC");
        }

        /// <summary>Gets FEMA measure bundle.</summary>
        /// <returns>The FEMA measure bundle.</returns>
        public static Bundle GetFemaMeasureBundle()
        {
            return GetBundleForMeasure(_femaCompleteMeasure, "sanerFEMA");
        }

        /// <summary>Gets bundle for measure.</summary>
        /// <param name="measure">The measure.</param>
        /// <param name="id">     The identifier.</param>
        /// <returns>The bundle for measure.</returns>
        private static Bundle GetBundleForMeasure(
            Measure measure,
            string id)
        {
            string bundleId = FhirIds.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirIds.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                measure,
                $"{FhirSystems.Internal}MeasureReport/{id}");

            return bundle;
        }

        /// <summary>Gets measure bundle.</summary>
        /// <returns>The measure bundle.</returns>
        public static Bundle GetMeasureBundle()
        {
            string bundleId = FhirIds.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirIds.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            foreach (SanerMeasureInfo info in _cdcMeasureInfoByName.Values)
            {
                bundle.AddResourceEntry(
                    BuildMeasure(info),
                    $"{FhirSystems.Internal}Measure/{info.Name}");
            }

            return bundle;
        }
    }
}
