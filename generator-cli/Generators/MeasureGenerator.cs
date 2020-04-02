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

        /// <summary>The canonical URL base.</summary>
        public const string CDCCanonicalUrl = "http://cdcmeasures.example.org/modules/covid19/20200331";

        /// <summary>The canonical URL base.</summary>
        public const string CanonicalUrl = "http://saner.example.org/covid19/20200331";

        /// <summary>The identifier screening rate.</summary>
        public const string IdScreeningRate = "screening-rate";

        /// <summary>Number of identifier tests.</summary>
        public const string IdTestCount = "covid-19-test-count";

        /// <summary>Number of identifier test positives.</summary>
        public const string IdTestPositiveCount = "covid-19-test-positive-count";

        /// <summary>The identifier recovered.</summary>
        public const string IdRecovered = "covid-19-recovered";

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

        /// <summary>List of cdc documents.</summary>
        private static readonly List<string> _cdcDocumentList = new List<string>()
        {
            "https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf",
        };

        /// <summary>Gets the test total.</summary>
        /// <value>The test total.</value>
        public static Measure TestTotal => BuildMeasure(
            IdTestCount,
            CanonicalUrl,
            "COVID-19 Tests Performed",
            "The total number of patients for whom a test for COVID-19 was ordered.",
            new FhirPopulation("Location - facility", "COVID-19 Tests Performed", "Count([???])"),
            null,
            null,
            FhirTriplet.MeasureTypeOutcome);

        /// <summary>Gets the test positive total.</summary>
        /// <value>The test positive total.</value>
        public static Measure TestPositiveTotal => BuildMeasure(
            IdTestPositiveCount,
            CanonicalUrl,
            "COVID-19 Positive Tests",
            "The total number of patients for whom a positive result for a COVID-19 test was documented.",
            new FhirPopulation("Location - facility", "COVID-19 Positive Tests", "Count([???])"),
            null,
            null,
            FhirTriplet.MeasureTypeOutcome);

        /// <summary>Gets the beds total.</summary>
        /// <value>The beds total.</value>
        public static Measure BedsTotal => BuildMeasure(
            CDCTotalBeds,
            CDCCanonicalUrl,
            "All Hospital Beds",
            "Total number of all Inpatient and outpatient beds, " +
                "including all staffed, ICU, licensed, and overflow(surge) beds used for " +
                "inpatients or outpatients.",
            new FhirPopulation("Location - facility", "All Hospital Beds", "Count([Supply: Beds])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the inpatient beds total.</summary>
        /// <value>The inpatient beds total.</value>
        public static Measure InpatientBedsTotal => BuildMeasure(
            CDCInpatientBeds,
            CDCCanonicalUrl,
            "Hospital Inpatient Beds",
            "Inpatient beds, including all staffed, licensed, and overflow(surge) beds used for inpatients.",
            new FhirPopulation("Location - facility", "Hospital Inpatient Beds", "Count([Supply: Beds])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the inpatient beds occupied.</summary>
        /// <value>The inpatient beds occupied.</value>
        public static Measure InpatientBedsOccupied => BuildMeasure(
            CDCInpatientBedOccupancy,
            CDCCanonicalUrl,
            "Hospital Inpatient Bed Occupancy",
            "Total number of staffed inpatient beds that are occupied.",
            new FhirPopulation("Location - facility", "Hospital Inpatient Bed Occupancy", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the icu beds total.</summary>
        /// <value>The icu beds total.</value>
        public static Measure IcuBedsTotal => BuildMeasure(
            CDCIcuBeds,
            CDCCanonicalUrl,
            "Hospital ICU Beds",
            "Total number of staffed inpatient intensive care unit (ICU) beds.",
            new FhirPopulation("Location - facility", "Hospital ICU Beds", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the icu beds occupied.</summary>
        /// <value>The icu beds occupied.</value>
        public static Measure IcuBedsOccupied => BuildMeasure(
            CDCIcuBedOccupancy,
            CDCCanonicalUrl,
            "Hospital ICU Bed Occupancy",
            "Total number of staffed inpatient ICU beds that are occupied.",
            new FhirPopulation("Location - facility", "Hospital ICU Bed Occupancy", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the ventilators total.</summary>
        /// <value>The ventilators total.</value>
        public static Measure VentilatorsTotal => BuildMeasure(
            CDCVentilators,
            CDCCanonicalUrl,
            "Mechanical Ventilators",
            "Total number of ventilators available.",
            new FhirPopulation("Location - facility", "Mechanical Ventilators", "Count([Supply: Ventilators])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the ventilators in use.</summary>
        /// <value>The ventilators in use.</value>
        public static Measure VentilatorsInUse => BuildMeasure(
            CDCVentilatorsInUse,
            CDCCanonicalUrl,
            "Mechanical Ventilators In Use",
            "Total number of ventilators in use.",
            new FhirPopulation("Location - facility", "Mechanical Ventilators In Use", "Count(???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the COVID patients hospitalized.</summary>
        /// <value>The COVID patients hospitalized.</value>
        public static Measure CovidPatientsHospitalized => BuildMeasure(
            CDCHospitalizedPatients,
            CDCCanonicalUrl,
            "COVID-19 Patients Hospitalized",
            "Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.",
            new FhirPopulation("Location - facility", "COVID-19 Patients Hospitalized", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the covid patients ventilated.</summary>
        /// <value>The covid patients ventilated.</value>
        public static Measure CovidPatientsVentilated => BuildMeasure(
            CDCVentilatedPatients,
            CDCCanonicalUrl,
            "COVID-19 Patients Hospitalized and Ventilated",
            "Patients hospitalized in an NHSN inpatient care location who have suspected or confirmed " +
                "COVID - 19 and are on a mechanical ventilator.",
            new FhirPopulation("Location - facility", "COVID-19 Patients Hospitalized and Ventilated", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the covid hospital onset.</summary>
        /// <value>The covid hospital onset.</value>
        public static Measure CovidHospitalOnset => BuildMeasure(
            CDCHospitalOnset,
            CDCCanonicalUrl,
            "COVID-19 Hospital Onset",
            "Patients hospitalized in an NHSN inpatient care location with onset of suspected " +
                "or confirmed COVID - 19 14 or more days after hospitalization.",
            new FhirPopulation("Location - facility", "COVID-19 Hospital Onset", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeOutcome);

        /// <summary>Gets the covid awaiting inpatient.</summary>
        /// <value>The covid awaiting inpatient.</value>
        public static Measure CovidAwaitingBed => BuildMeasure(
            CDCAwaitingBeds,
            CDCCanonicalUrl,
            "ED/Overflow",
            "Patients with suspected or confirmed COVID-19 who are in " +
                "the ED or any overflow location awaiting an inpatient bed.",
            new FhirPopulation("Location - facility", "ED/Overflow", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the covid awaiting ventilator.</summary>
        /// <value>The covid awaiting ventilator.</value>
        public static Measure CovidAwaitingVentilator => BuildMeasure(
            CDCAwaitingVentilators,
            CDCCanonicalUrl,
            "ED/Overflow and ventilated",
            "Patients with suspected or confirmed COVID - 19 who are in the ED or any overflow location " +
                "awaiting an inpatient bed and on a mechanical ventilator.",
            new FhirPopulation("Location - facility", "ED/Overflow and ventilated", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeStructure);

        /// <summary>Gets the covid recovered.</summary>
        /// <value>The covid recovered.</value>
        public static Measure CovidRecovered => BuildMeasure(
            IdRecovered,
            CanonicalUrl,
            "COVID-19 Patients Recovered",
            "Patients with suspected or confirmed COVID-19 who have recovered and been discharged.",
            new FhirPopulation("Location - facility", "COVID-19 Patients Recovered", "Count([???])"),
            null,
            null,
            FhirTriplet.MeasureTypeOutcome);

        /// <summary>Gets the covid died.</summary>
        /// <value>The covid died.</value>
        public static Measure CovidDied => BuildMeasure(
            CDCDied,
            CDCCanonicalUrl,
            "COVID-19 Patients Died",
            "Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location.",
            new FhirPopulation("Location - facility", "COVID-19 Patients Died", "Count([???])"),
            null,
            _cdcDocumentList,
            FhirTriplet.MeasureTypeOutcome);

        /// <summary>Builds a measure.</summary>
        /// <param name="id">                 The identifier.</param>
        /// <param name="urlBase">            The URL base.</param>
        /// <param name="title">              The title.</param>
        /// <param name="description">        The description.</param>
        /// <param name="population">         (Optional) The population.</param>
        /// <param name="topics">             (Optional) The topics.</param>
        /// <param name="relatedDocumentUrls">(Optional) The related document urls.</param>
        /// <param name="measureTypes">       (Optional) List of types of the measures.</param>
        /// <returns>A Measure.</returns>
        private static Measure BuildMeasure(
            string id,
            string urlBase,
            string title,
            string description,
            FhirPopulation population = null,
            List<FhirTriplet> topics = null,
            List<string> relatedDocumentUrls = null,
            FhirTriplet measureType = null)
        {
            Measure measure = new Measure()
            {
                Id = id,
                Name = id,
                Url = $"{urlBase}/{id}",
                Version = MeasureVersion,
                Title = title,
                Status = PublicationStatus.Draft,
                Subject = new CodeableConcept("Location", "Location"),
                Date = PublicationDate,
                Publisher = Publisher,
                Description = new Markdown(description),
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

            if (measureType != null)
            {
                measure.Type = new List<CodeableConcept>()
                {
                    measureType.Concept,
                };
            }

            if ((relatedDocumentUrls != null) && (relatedDocumentUrls.Count > 0))
            {
                measure.RelatedArtifact = new List<RelatedArtifact>();

                foreach (string relatedDocumentUrl in relatedDocumentUrls)
                {
                    measure.RelatedArtifact.Add(
                        new RelatedArtifact()
                        {
                            Type = RelatedArtifact.RelatedArtifactType.Documentation,
                            Url = relatedDocumentUrl,
                        });
                }
            }

            if ((topics != null) && (topics.Count > 0))
            {
                measure.Topic = new List<CodeableConcept>();

                foreach (FhirTriplet topic in topics)
                {
                    measure.Topic.Add(topic.Concept);
                }
            }

            if (population != null)
            {
                measure.Group = new List<Measure.GroupComponent>()
                {
                    new Measure.GroupComponent()
                    {
                        Population = population.ComponentList,
                    },
                };
            }

            measure.Scoring = FhirTriplet.ScoringContinuousVariable.Concept;

            return measure;
        }

        /// <summary>Gets report bundle.</summary>
        /// <returns>The report bundle.</returns>
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

            bundle.AddResourceEntry(
                TestTotal,
                $"{SystemLiterals.Internal}Measure/{TestTotal.Id}");

            bundle.AddResourceEntry(
                TestPositiveTotal,
                $"{SystemLiterals.Internal}Measure/{TestPositiveTotal.Id}");

            bundle.AddResourceEntry(
                BedsTotal,
                $"{SystemLiterals.Internal}Measure/{BedsTotal.Id}");

            bundle.AddResourceEntry(
                InpatientBedsTotal,
                $"{SystemLiterals.Internal}Measure/{InpatientBedsTotal.Id}");

            bundle.AddResourceEntry(
                InpatientBedsOccupied,
                $"{SystemLiterals.Internal}Measure/{InpatientBedsOccupied.Id}");

            bundle.AddResourceEntry(
                IcuBedsTotal,
                $"{SystemLiterals.Internal}Measure/{IcuBedsTotal.Id}");

            bundle.AddResourceEntry(
                IcuBedsOccupied,
                $"{SystemLiterals.Internal}Measure/{IcuBedsOccupied.Id}");

            bundle.AddResourceEntry(
                VentilatorsTotal,
                $"{SystemLiterals.Internal}Measure/{VentilatorsTotal.Id}");

            bundle.AddResourceEntry(
                VentilatorsInUse,
                $"{SystemLiterals.Internal}Measure/{VentilatorsInUse.Id}");

            bundle.AddResourceEntry(
                CovidPatientsHospitalized,
                $"{SystemLiterals.Internal}Measure/{CovidPatientsHospitalized.Id}");

            bundle.AddResourceEntry(
                CovidPatientsVentilated,
                $"{SystemLiterals.Internal}Measure/{CovidPatientsVentilated.Id}");

            bundle.AddResourceEntry(
                CovidHospitalOnset,
                $"{SystemLiterals.Internal}Measure/{CovidHospitalOnset.Id}");

            bundle.AddResourceEntry(
                CovidAwaitingBed,
                $"{SystemLiterals.Internal}Measure/{CovidAwaitingBed.Id}");

            bundle.AddResourceEntry(
                CovidAwaitingVentilator,
                $"{SystemLiterals.Internal}Measure/{CovidAwaitingVentilator.Id}");

            bundle.AddResourceEntry(
                CovidRecovered,
                $"{SystemLiterals.Internal}Measure/{CovidRecovered.Id}");

            bundle.AddResourceEntry(
                CovidDied,
                $"{SystemLiterals.Internal}Measure/{CovidDied.Id}");

            return bundle;
        }
    }
}
