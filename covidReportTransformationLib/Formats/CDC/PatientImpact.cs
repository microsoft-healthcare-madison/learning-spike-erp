// <copyright file="PatientImpact.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.CDC
{
    /// <summary>A cdc literals.</summary>
    public class PatientImpact : IReportingFormat
    {
        /// <summary>Identifier for the facility.</summary>
        public const string FacilityId = "facilityId";

        /// <summary>Identifier for the summary census.</summary>
        public const string SummaryCensusId = "summaryCensusId";

        /// <summary>The collection date.</summary>
        public const string CollectionDate = "collectionDate";

        /// <summary>The total beds.</summary>
        public const string TotalBeds = "numTotBeds";

        /// <summary>The inpatient beds.</summary>
        public const string InpatientBeds = "numbeds";

        /// <summary>The inpatient bed occupancy.</summary>
        public const string InpatientBedOccupancy = "numBedsOcc";

        /// <summary>The icu beds.</summary>
        public const string IcuBeds = "numICUBeds";

        /// <summary>The icu bed occupancy.</summary>
        public const string IcuBedOccupancy = "numICUBedsOcc";

        /// <summary>The ventilators.</summary>
        public const string Ventilators = "numVent";

        /// <summary>The cdc ventilators in use.</summary>
        public const string VentilatorsInUse = "numVentUse";

        /// <summary>The cdc hospitalized patients.</summary>
        public const string HospitalizedPatients = "numC19HospPats";

        /// <summary>The cdc ventilated patients.</summary>
        public const string VentilatedPatients = "numC19MechVentPats";

        /// <summary>The cdc hospital onset.</summary>
        public const string HospitalOnset = "numC19HOPats";

        /// <summary>The cdc overflow patients.</summary>
        public const string AwaitingBeds = "numC19OverflowPats";

        /// <summary>The cdc awaiting ventilators.</summary>
        public const string AwaitingVentilators = "numC19OFMechVentPats";

        /// <summary>The cdc died.</summary>
        public const string Died = "numC19Died";

        /// <summary>The current.</summary>
        private static PatientImpact _current = new PatientImpact();

        /// <summary>The fields.</summary>
        private static readonly Dictionary<string, FormatField> _fields = new Dictionary<string, FormatField>()
        {
            {
                FacilityId,
                new FormatField(
                    FacilityId,
                    "Facility ID #",
                    string.Empty,
                    FormatField.FieldType.ShortString,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    null,
                    null,
                    null)
            },
            {
                SummaryCensusId,
                new FormatField(
                    SummaryCensusId,
                    "Summary Census ID #",
                    string.Empty,
                    FormatField.FieldType.ShortString,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    null,
                    null,
                    null)
            },
            {
                CollectionDate,
                new FormatField(
                    CollectionDate,
                    "Collection Date",
                    "Date for which patient impact and hospital capacity counts are reported",
                    FormatField.FieldType.Date,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    null,
                    null,
                    null)
            },
            {
                HospitalizedPatients,
                new FormatField(
                    HospitalizedPatients,
                    "HOSPITALIZED",
                    "Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                VentilatedPatients,
                new FormatField(
                    VentilatedPatients,
                    "HOSPITALIZED and VENTILATED",
                    "Patients hospitalized in an NHSN inpatient care location who have suspected or confirmed COVID - 19 and are on a mechanical ventilator.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                HospitalOnset,
                new FormatField(
                    HospitalOnset,
                    "HOSPITAL ONSET",
                    "Patients hospitalized in an NHSN inpatient care location with onset of suspected or confirmed COVID - 19 14 or more days after hospitalization.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                AwaitingBeds,
                new FormatField(
                    AwaitingBeds,
                    "ED/OVERFLOW",
                    "Patients with suspected or confirmed COVID-19 who are in the ED or any overflow location awaiting an inpatient bed.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                AwaitingVentilators,
                new FormatField(
                    AwaitingVentilators,
                    "ED/OVERFLOW and VENTILATED",
                    "Patients with suspected or confirmed COVID - 19 who are in the ED or any overflow location awaiting an inpatient bed and on a mechanical ventilator.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                Died,
                new FormatField(
                    Died,
                    "DEATHS",
                    "Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    1500,
                    null)
            },
            {
                TotalBeds,
                new FormatField(
                    TotalBeds,
                    "ALL HOSPTIAL BEDS",
                    "Total number of all Inpatient and outpatient beds, including all staffed, ICU, licensed, and overflow(surge) beds used for inpatients or outpatients.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                InpatientBeds,
                new FormatField(
                    InpatientBeds,
                    "HOSPITAL INPATIENT BEDS",
                    "Inpatient beds, including all staffed, licensed, and overflow(surge) beds used for inpatients.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    0,
                    10000,
                    null)
            },
            {
                InpatientBedOccupancy,
                new FormatField(
                    InpatientBedOccupancy,
                    "HOSPITAL INPATIENT BED OCCUPANCY",
                    "Total number of staffed inpatient beds that are occupied.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                IcuBeds,
                new FormatField(
                    IcuBeds,
                    "ICU BEDS",
                    "Total number of staffed inpatient intensive care unit (ICU) beds.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                IcuBedOccupancy,
                new FormatField(
                    IcuBedOccupancy,
                    "ICU BED OCCUPANCY",
                    "Total number of staffed inpatient ICU beds that are occupied.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                Ventilators,
                new FormatField(
                    Ventilators,
                    "MECHANICAL VENTILATORS",
                    "Total number of ventilators available.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                VentilatorsInUse,
                new FormatField(
                    VentilatorsInUse,
                    "MECHANICAL VENTILATORS IN USE",
                    "Total number of ventilators in use.",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    0,
                    10000,
                    null)
            },
        };

        /// <summary>The measure report fields.</summary>
        private static readonly List<string> _measureReportFields = new List<string>()
        {
            TotalBeds,
            InpatientBeds,
            InpatientBedOccupancy,
            IcuBeds,
            IcuBedOccupancy,
            Ventilators,
            VentilatorsInUse,
            HospitalizedPatients,
            VentilatedPatients,
            HospitalOnset,
            AwaitingBeds,
            AwaitingVentilators,
            Died,
        };

        /// <summary>The questionnaire sections.</summary>
        private static readonly List<QuestionnaireSection> _questionnaireSections = new List<QuestionnaireSection>()
        {
            new QuestionnaireSection(
                "COVID-19 Module: Patient Impact and Hospital Capacity Pathway",
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(FacilityId),
                    new QuestionnaireQuestion(SummaryCensusId),
                    new QuestionnaireQuestion(CollectionDate),
                }),
            new QuestionnaireSection(
                "Patient Impact Data Elements",
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(HospitalizedPatients),
                    new QuestionnaireQuestion(VentilatedPatients),
                    new QuestionnaireQuestion(HospitalOnset),
                    new QuestionnaireQuestion(AwaitingBeds),
                    new QuestionnaireQuestion(AwaitingVentilators),
                    new QuestionnaireQuestion(Died),
                }),
            new QuestionnaireSection(
                "Hospital Bed/ Intensive Care Unit (ICU)/ Ventilator Capacity Data Elements",
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(TotalBeds),
                    new QuestionnaireQuestion(InpatientBeds),
                    new QuestionnaireQuestion(InpatientBedOccupancy),
                    new QuestionnaireQuestion(IcuBeds),
                    new QuestionnaireQuestion(IcuBedOccupancy),
                    new QuestionnaireQuestion(Ventilators),
                    new QuestionnaireQuestion(VentilatorsInUse),
                }),
        };

        /// <summary>The CSV fields.</summary>
        private static readonly List<string> _csvFields = new List<string>()
        {
            CollectionDate,
            TotalBeds,
            InpatientBeds,
            InpatientBedOccupancy,
            IcuBeds,
            IcuBedOccupancy,
            Ventilators,
            VentilatorsInUse,
            HospitalizedPatients,
            VentilatedPatients,
            HospitalOnset,
            AwaitingBeds,
            AwaitingVentilators,
            Died,
        };

        /// <summary>The cdc citation.</summary>
        private static readonly Hl7.Fhir.Model.Markdown _cdcCitation = new Hl7.Fhir.Model.Markdown(
            "Centers for Disease Control and Prevention (CDC), National Healthcare Safety Network (NHSN)");

        /// <summary>List of cdc documents.</summary>
        private static readonly List<Hl7.Fhir.Model.RelatedArtifact> _artifacts = new List<Hl7.Fhir.Model.RelatedArtifact>()
        {
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "COVID-19 Module",
                Display = "CDC’s NHSN is supporting the nation’s COVID-19 response by introducing a new COVID-19 Module.",
                Citation = _cdcCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://www.cdc.gov/nhsn/acute-care-hospital/covid19/",
                    Creation = "2020-03-27",
                },
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "Importing COVID-19 Patient Module Denominator data for Patient Safety Component",
                Citation = _cdcCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf",
                    Creation = "2020-03-27",
                },
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "Instructions for Completion of the COVID-19 Patient Impact and Hospital Capacity Module Form (CDC 57.130)",
                Citation = _cdcCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf",
                    Creation = "2020-03-27",
                },
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "covi19-test-csv-import",
                Citation = _cdcCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://www.cdc.gov/nhsn/pdfs/covid19/covid19-test-csv-import.csv",
                    Creation = "2020-03-27",
                },
            },
        };

        /// <summary>Gets the current.</summary>
        /// <value>The current.</value>
        public static PatientImpact Current => _current;

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name => "sanerCDC";

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title => "SANER CDC COVID-19 Patient Impact & Hospital Capacity Module";

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description => "SANER implementation of the CDC COVID-19 Patient Impact & Hospital Capacity Module";

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
        public List<string> CsvFields => _csvFields;

        /// <summary>Gets the spreadsheet locations.</summary>
        /// <value>The spreadsheet locations.</value>
        public Dictionary<string, SpreadsheetLocation> SpreadsheetLocations => null;

        /// <summary>Gets the artifacts.</summary>
        /// <value>The artifacts.</value>
        public List<Hl7.Fhir.Model.RelatedArtifact> Artifacts => _artifacts;
    }
}
