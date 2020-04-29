// <copyright file="PatientImpact.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Utils;

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

        /// <summary>NOT OFFICIAL: The patients.</summary>
        public const string Patients = "numC19Pats";

        /// <summary>The cdc hospitalized patients.</summary>
        public const string HospitalizedPatients = "numC19HospPats";

        /// <summary>NOT OFFICIAL: The ventilated not hospitalized.</summary>
        public const string VentilatedNotHospitalized = "numC19VentPats";

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
                    string.Empty,
                    FormatField.FieldType.Date,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    null,
                    null,
                    null)
            },
            {
                Patients,
                new FormatField(
                    Patients,
                    "COVID-19 Patients",
                    "Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.",
                    "encounter.where(clinicalStatus = 'active' and diagnosis.condition.ofType(Condition).code in %ValueSet-SuspectedOrDiagnosedCOVID19)\n| Encounter.where(clinicalStatus = \"active\" and Condition.where(code in %ValueSet-SuspectedOrDiagnosedCOVID19).encounter = $this)",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                HospitalizedPatients,
                new FormatField(
                    HospitalizedPatients,
                    "Hospitalized COVID-19 Patients",
                    "Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.",
                    "%numC19Pats.where(location.where(status='active' and type in %ValueSet-InpatientLocations))",
                    FormatField.FieldType.Count,
                    FormatField.FhirMeasureType.Outcome,
                    false,
                    0,
                    10000,
                    null)
            },
            {
                VentilatedNotHospitalized,
                new FormatField(
                    VentilatedNotHospitalized,
                    "Ventilated COVID-19 Patients",
                    "Patients in any location who have suspected or confirmed COVID-19 and are currently on a ventilator.",
                    "%numC19Pats.where(Device.where(type in %ValueSet-VentilatorDevices and status = active).patient = $this.patient)",
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
                    "Hospitalized and Ventilated COVID-19 Patients",
                    "Patients hospitalized in an NHSN inpatient care location who have suspected or confirmed COVID-19 and are on a mechanical ventilator.",
                    "%numC19HospPats.intersect(%numC19VentPats)",
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
                    "Hospital Onset COVID-19 Patients",
                    "Patients hospitalized in an NHSN inpatient care location with onset of suspected or confirmed COVID - 19 14 or more days after hospitalization.",
                    "condition.where(\ncode in %ValueSet-SuspectedOrDiagnosedCOVID19\nand encounter in %numC19HospPats\nand onset + 14 days > encounter.period.start).encounter",
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
                    "ED/Overflow COVID-19 Patients",
                    "Patients with suspected or confirmed COVID-19 who are in the ED or any overflow location awaiting an inpatient bed.",
                    "%numC19Pats.where(location.where(status='active' and type in %ValueSet-EDorOverflowLocations))",
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
                    "ED/Overflow and Ventilated COVID-19 Patients",
                    "Patients with suspected or confirmed COVID - 19 who are in the ED or any overflow location awaiting an inpatient bed and on a mechanical ventilator.",
                    "%numC19OverflowPats.intersect(%numC19VentPats)",
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
                    "COVID-19 Patient Deaths",
                    "Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location.",
                    "%numC19Pats.hospitalization.dispostion in %ValueSet-PatientDied",
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
                    "All Hospital Beds",
                    "Total number of all Inpatient and outpatient beds, including all staffed, ICU, licensed, and overflow(surge) beds used for inpatients or outpatients.",
                    "Device.where(type in %ValueSet-BedDeviceTypes and location.physicalType in %ValueSet-BedLocationTypes)",
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
                    "Hospital Inpatient Beds",
                    "Inpatient beds, including all staffed, licensed, and overflow(surge) beds used for inpatients.",
                    "%numTotBeds.where(location.type in %ValueSet-InpatientLocations)",
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
                    "Hospital Inpatient Bed Occupancy",
                    "Total number of staffed inpatient beds that are occupied.",
                    "%numBeds.where(location.operationalStatus = %ValueSet-OccupiedBed)",
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
                    "ICU Beds",
                    "Total number of staffed inpatient intensive care unit (ICU) beds.",
                    "%numBeds.where(location.type in %ValueSet-ICULocations)",
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
                    "ICU Bed Occupancy",
                    "Total number of staffed inpatient ICU beds that are occupied.",
                    "%numICUBeds.where(location.operationalStatus = %ValueSet-OccupiedBed)",
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
                    "Mechanical Ventilators",
                    "Total number of ventilators available.",
                    "Device.where(type in %ValueSet-VentilatorDevices and status = active)",
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
                    "Mechanical Ventilators in Use",
                    "Total number of ventilators in use.",
                    "%numVent.where(patient!={})",
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
                Label = "NHSN COVID-19 Reporting",
                Display = "CDC/NHSN COVID-19 Patient Impact & Hospital Capacity Module Home Page",
                Url = "https://www.cdc.gov/nhsn/acute-care-hospital/covid19/",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "How to import COVID-19 Summary Data",
                Display = "Importing COVID-19 Patient Module Denominator data for Patient Safety Component",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "Table of Instructions",
                Display = "Instructions for Completion of the COVID-19 Patient Impact and Hospital Capacity Module Form (CDC 57.130)",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "CSV File Template",
                Display = "CDC/NHSN COVID-19 Reporting CSV File Template",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/covid19-test-csv-import.csv",
                Citation = _cdcCitation,
            },
        };

        /// <summary>The authors.</summary>
        private static readonly List<Hl7.Fhir.Model.ContactDetail> _authors = new List<Hl7.Fhir.Model.ContactDetail>()
        {
            new Hl7.Fhir.Model.ContactDetail()
            {
                Name = "Centers for Disease Control/National Healthcare Safety Network (CDC/NHSN)",
                Telecom = new List<Hl7.Fhir.Model.ContactPoint>()
                {
                    new Hl7.Fhir.Model.ContactPoint(
                        Hl7.Fhir.Model.ContactPoint.ContactPointSystem.Email,
                        null,
                        "mailto:nhsn@cdc.gov"),
                },
            },
        };

        /// <summary>Gets the current.</summary>
        /// <value>The current.</value>
        public static PatientImpact Current => _current;

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name => "CDCPatientImpactAndHospitalCapacity";

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title => "Patient Impact and Hospital Capacity";

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description => "SANER implementation of the CDC COVID-19 Patient Impact & Hospital Capacity Module";

        /// <summary>Gets the definition.</summary>
        /// <value>The definition.</value>
        public List<string> Definition => new List<string>()
        {
            "Ventilator\n: Any device used to support, assist or control respiration (inclusive of the weaning period) through the application of positive\npressure to the airway when delivered via an artificial airway, specifically an oral/nasal endotracheal or tracheostomy tube.\nNote: Ventilation and lung expansion devices that deliver positive pressure to the airway (for example: CPAP, BiPAP, bi-level, IPPB and\nPEEP) via non-invasive means (for example: nasal prongs, nasal mask, full face mask, total mask, etc.) are not considered ventilators\nunless positive pressure is delivered via an artificial airway (oral/nasal endotracheal or tracheostomy tube).",
            "Beds\n: Baby beds in mom's room count as 1 bed, even if there are multiple baby beds\nFollow-up in progress if staffed is less than licensed.\nTotal includes all beds, even if with surge beds it exceeds licensed beds.",
            "ICU beds\n: Include NICU (from CDC Webinar 31-Mar-2020) (outstanding question on burn unit)",
        };

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

        /// <summary>Gets the authors.</summary>
        /// <value>The authors.</value>
        public List<Hl7.Fhir.Model.ContactDetail> Authors => _authors;
    }
}
