// <copyright file="MeasureGenerator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using generator_cli.Models;
using Hl7.Fhir.Model;
using static generator_cli.Generators.CommonLiterals;

namespace generator_cli.Generators
{
    /// <summary>A measure generator.</summary>
    public static class MeasureGenerator
    {
        /// <summary>The measure version.</summary>
        private const string MeasureVersion = "20200401.06";

        /// <summary>The publication date.</summary>
        private const string PublicationDate = "2020-03-31T00:00:00Z";

        /// <summary>The publisher.</summary>
        private const string Publisher = "SANER-IG";

        /// <summary>The cdc grouped measures.</summary>
        private static Dictionary<string, Measure> _cdcGroupedMeasures = new Dictionary<string, Measure>();

        /// <summary>The cdc measures.</summary>
        private static Dictionary<string, Measure> _cdcMeasures = new Dictionary<string, Measure>();

        /// <summary>The CDC total beds.</summary>
        public const string CDCTotalBeds = "numTotBeds";

        /// <summary>The CDC inpatient beds.</summary>
        public const string CDCInpatientBeds = "numbeds";

        /// <summary>The CDC inpatient bed occupancy.</summary>
        public const string CDCInpatientBedOccupancy = "numBedsOcc";

        /// <summary>The CDC icu beds.</summary>
        public const string CDCIcuBeds = "numICUBeds";

        /// <summary>The CDC icu bed occupancy.</summary>
        public const string CDCIcuBedOccupancy = "numICUBedsOcc";

        /// <summary>The CDC ventilators.</summary>
        public const string CDCVentilators = "numVent";

        /// <summary>The cdc ventilators in use.</summary>
        public const string CDCVentilatorsInUse = "numVentUse";

        /// <summary>The cdc hospitalized patients.</summary>
        public const string CDCHospitalizedPatients = "numC19HospPats";

        /// <summary>The cdc ventilated patients.</summary>
        public const string CDCVentilatedPatients = "numC19MechVentPats";

        /// <summary>The cdc hospital onset.</summary>
        public const string CDCHospitalOnset = "numC19HOPats";

        /// <summary>The cdc overflow patients.</summary>
        public const string CDCAwaitingBeds = "numC19OverflowPats";

        /// <summary>The cdc awaiting ventilators.</summary>
        public const string CDCAwaitingVentilators = "numC19OFMechVentPats";

        /// <summary>The cdc died.</summary>
        public const string CDCDied = "numC19Died";

        /// <summary>The cdc measures.</summary>
        private static readonly List<MeasureInfo> _cdcMeasureInfo = new List<MeasureInfo>()
        {
            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCTotalBeds,
                "All Hospital Beds",
                "Total number of all Inpatient and outpatient beds, " +
                    "including all staffed, ICU, licensed, and overflow(surge) beds used for " +
                    "inpatients or outpatients.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCInpatientBeds,
                "Hospital Inpatient Beds",
                "Inpatient beds, including all staffed, licensed, and overflow(surge) beds used for inpatients.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCInpatientBedOccupancy,
                "Hospital Inpatient Bed Occupancy",
                "Total number of staffed inpatient beds that are occupied.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCIcuBeds,
                "Hospital ICU Beds",
                "Total number of staffed inpatient intensive care unit (ICU) beds.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCIcuBedOccupancy,
                "Hospital ICU Bed Occupancy",
                "Total number of staffed inpatient ICU beds that are occupied.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCVentilators,
                "Mechanical Ventilators",
                "Total number of ventilators available.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCVentilatorsInUse,
                "Mechanical Ventilators In Use",
                "Total number of ventilators in use.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCHospitalizedPatients,
                "COVID-19 Patients Hospitalized",
                "Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCVentilatedPatients,
                "COVID-19 Patients Hospitalized and Ventilated",
                "Patients hospitalized in an NHSN inpatient care location who have suspected or confirmed " +
                    "COVID - 19 and are on a mechanical ventilator.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCHospitalOnset,
                "COVID-19 Hospital Onset",
                "Patients hospitalized in an NHSN inpatient care location with onset of suspected " +
                    "or confirmed COVID - 19 14 or more days after hospitalization.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCAwaitingBeds,
                "ED/Overflow",
                "Patients with suspected or confirmed COVID-19 who are in " +
                    "the ED or any overflow location awaiting an inpatient bed.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCAwaitingVentilators,
                "ED/Overflow and Ventilated",
                "Patients with suspected or confirmed COVID - 19 who are in the ED or any overflow location " +
                    "awaiting an inpatient bed and on a mechanical ventilator.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.CDC,
                CDCDied,
                "COVID-19 Patients Died",
                "Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),
        };

        /// <summary>The fema measures.</summary>
        private static Dictionary<string, Measure> _femaMeasures = new Dictionary<string, Measure>();

        /// <summary>The fema tests ordered today.</summary>
        public const string FemaTestsOrderedToday = "newDiagnosticTests";

        /// <summary>The fema tests ordered total.</summary>
        public const string FemaTestsOrderedTotal = "cumulativeDiagnosticTests";

        /// <summary>The fema tests with results new.</summary>
        public const string FemaTestsWithResultsToday = "newTestsResulted";

        /// <summary>The fema specimens rejected total.</summary>
        public const string FemaSpecimensRejectedTotal = "cumulativeSpecimensRejected";

        /// <summary>The fema tests performed total.</summary>
        public const string FemaTestsPerformedTotal = "cumulativeTestsPerformed";

        /// <summary>The fema positive c 19 today.</summary>
        public const string FemaPositiveC19Today = "newPositiveC19Tests";

        /// <summary>The fema positive c 19 total.</summary>
        public const string FemaPositiveC19Total = "cumulativePositiveC19Tests";

        /// <summary>The fema percent c 19 postive.</summary>
        public const string FemaPercentC19PostiveToday = "percentPositiveAmongNewlyResultedTests";

        /// <summary>The fema percent c 19 positive total.</summary>
        public const string FemaPercentC19PositiveTotal = "cumulativePercentPositiveAmongResultedTests";

        /// <summary>Information describing the fema measure.</summary>
        private static readonly List<MeasureInfo> _femaMeasureInfo = new List<MeasureInfo>()
        {
            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaTestsOrderedToday,
                "New Diagnostic Tests Ordered/Received",
                "Midnight to midnight cutoff, tests ordered on previous date queried.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaTestsOrderedTotal,
                "Cumulative Diagnostic Tests Ordered/Received",
                "All tests ordered to date.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaTestsWithResultsToday,
                "New Tests Resulted",
                "Midnight to midnight cutoff, test results released on previous date queried.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaSpecimensRejectedTotal,
                "Cumulative Specimens Rejected",
                "All specimens rejected for testing to date.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaTestsPerformedTotal,
                "Cumulative Tests Performed",
                "All tests with results released to date.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaPositiveC19Today,
                "New Positive COVID-19 Tests",
                "Midnight to midnight cutoff, positive test results released on the previous date queried.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaPositiveC19Total,
                "Cumulative Positive COVID-19 Tests",
                "All positivetest results released to date.",
                FhirTriplet.MeasureTypeStructure,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaPercentC19PostiveToday,
                "Percent Positive among Newly Resulted Tests",
                "# of new positive test results / # of total new tests released for previous date queried.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Ratio),

            new MeasureInfo(
                MeasureInfo.MeasureSource.FEMA,
                FemaPercentC19PositiveTotal,
                "Cumulative Percent Positive among Resulted Tests",
                "# of total positive results to released date / # of total test results released to date.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Ratio),
        };

        /// <summary>The other measures.</summary>
        private static readonly List<MeasureInfo> _otherMeasureInfo = new List<MeasureInfo>()
        {
            new MeasureInfo(
                MeasureInfo.MeasureSource.SANER,
                "covid-19-test-count",
                "COVID-19 Tests Performed",
                "The total number of patients for whom a test for COVID-19 was ordered.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.SANER,
                "covid-19-test-positive-count",
                "COVID-19 Positive Tests",
                "The total number of patients for whom a positive result for a COVID-19 test was documented.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),

            new MeasureInfo(
                MeasureInfo.MeasureSource.SANER,
                "covid-19-test-positive-count",
                "COVID-19 Positive Tests",
                "The total number of patients for whom a positive result for a COVID-19 test was documented.",
                FhirTriplet.MeasureTypeOutcome,
                MeasureInfo.MeasureStyle.Count),
        };

        /// <summary>Builds cdc bed measure.</summary>
        /// <returns>A Measure.</returns>
        private static Measure BuildCdcBedMeasure()
        {
            Measure measure = new Measure()
            {
                Id = "cdcBeds",
                Name = "cdcBeds",
                Url = $"{MeasureInfo.CdcCanonicalUrl}/beds",
                Version = MeasureVersion,
                Title = "CDC Bed Measurement Group",
                Status = PublicationStatus.Draft,
                Subject = new CodeableConcept("Location", "Location"),
                Date = PublicationDate,
                Publisher = Publisher,
                Description = new Markdown("CDC Bed Measurement Group"),
                Jurisdiction = new List<CodeableConcept>()
                {
                    FhirTriplet.UnitedStates.Concept,
                },
                UseContext = new List<UsageContext>()
                {
                    new UsageContext()
                    {
                        Code = FhirTriplet.GetCode(SystemLiterals.UsageContextType, ContextFocus),
                        Value = FhirTriplet.SctCovid.Concept,
                    },
                },
                Type = new List<CodeableConcept>()
                {
                    FhirTriplet.MeasureTypeStructure.Concept,
                },
                RelatedArtifact = new List<RelatedArtifact>(),
                Group = new List<Measure.GroupComponent>(),
                Scoring = FhirTriplet.ScoringCohort.Concept,
            };

            foreach (string relatedDocumentUrl in MeasureInfo.CdcDocumentList)
            {
                measure.RelatedArtifact.Add(
                    new RelatedArtifact()
                    {
                        Type = RelatedArtifact.RelatedArtifactType.Documentation,
                        Url = relatedDocumentUrl,
                    });
            }

            measure.Group.Add(GroupComponentFromCdcMeasure(_cdcMeasures[CDCTotalBeds]));
            measure.Group.Add(GroupComponentFromCdcMeasure(_cdcMeasures[CDCInpatientBeds]));
            measure.Group.Add(GroupComponentFromCdcMeasure(_cdcMeasures[CDCInpatientBedOccupancy]));
            measure.Group.Add(GroupComponentFromCdcMeasure(_cdcMeasures[CDCIcuBeds]));
            measure.Group.Add(GroupComponentFromCdcMeasure(_cdcMeasures[CDCIcuBedOccupancy]));

            return measure;
        }

        /// <summary>Group component from measure.</summary>
        /// <param name="measure">The measure.</param>
        /// <returns>A Measure.GroupComponent.</returns>
        private static Measure.GroupComponent GroupComponentFromCdcMeasure(
            Measure measure)
        {
            return new Measure.GroupComponent()
            {
                Code = new CodeableConcept(
                    MeasureInfo.CdcCanonicalUrl,
                    measure.Name),
                Description = measure.Description.ToString(),
                Population = measure.Group[0].Population,
            };
        }

        /// <summary>Builds a measure.</summary>
        /// <param name="info">The identifier.</param>
        /// <returns>A Measure.</returns>
        private static Measure BuildMeasure(
            MeasureInfo info)
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
                        Code = FhirTriplet.GetCode(SystemLiterals.UsageContextType, ContextFocus),
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

            if ((info.DocumentUrls != null) && (info.DocumentUrls.Count > 0))
            {
                measure.RelatedArtifact = new List<RelatedArtifact>();

                foreach (string relatedDocumentUrl in info.DocumentUrls)
                {
                    measure.RelatedArtifact.Add(
                        new RelatedArtifact()
                        {
                            Type = RelatedArtifact.RelatedArtifactType.Documentation,
                            Url = relatedDocumentUrl,
                        });
                }
            }

            measure.Group = new List<Measure.GroupComponent>()
            {
                new Measure.GroupComponent()
                {
                    Population = new List<Measure.PopulationComponent>()
                    {
                        new Measure.PopulationComponent()
                        {
                            Code = FhirTriplet.MeasurePopulation.Concept,
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
            foreach (MeasureInfo info in _cdcMeasureInfo)
            {
                _cdcMeasures.Add(info.Name, BuildMeasure(info));
            }

            _cdcGroupedMeasures.Add("beds", BuildCdcBedMeasure());

            // build FEMA measures
            foreach (MeasureInfo info in _femaMeasureInfo)
            {
                _femaMeasures.Add(info.Name, BuildMeasure(info));
            }
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
        /// <param name="name">The name.</param>
        /// <returns>A Measure.</returns>
        public static Measure CDCGroupedMeasure(string name)
        {
            if (!_cdcGroupedMeasures.ContainsKey(name))
            {
                return null;
            }

            return _cdcGroupedMeasures[name];
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

        /// <summary>Gets report bundle.</summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>The report bundle.</returns>
        public static Bundle GetGroupedMeasureBundle(string groupName)
        {
            if (!_cdcGroupedMeasures.ContainsKey(groupName))
            {
                return null;
            }

            string bundleId = FhirGenerator.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirGenerator.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                _cdcGroupedMeasures[groupName],
                $"{SystemLiterals.Internal}Measure/cdc{groupName}");

            return bundle;
        }

        /// <summary>Gets measure bundle.</summary>
        /// <returns>The measure bundle.</returns>
        public static Bundle GetMeasureBundle()
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

            bundle.Entry = new List<Bundle.EntryComponent>();

            foreach (MeasureInfo info in _cdcMeasureInfo)
            {
                bundle.AddResourceEntry(
                    BuildMeasure(info),
                    $"{SystemLiterals.Internal}Measure/{info.Name}");
            }

            return bundle;
        }
    }
}
