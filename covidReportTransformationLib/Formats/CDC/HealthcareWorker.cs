// <copyright file="HealthcareWorker.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.CDC
{
    /// <summary>A healthcare worker.</summary>
    public class HealthcareWorker : IReportingFormat
    {
        /// <summary>Identifier for the facility.</summary>
        public const string FacilityId = "facilityId";

        /// <summary>Identifier for the summary census.</summary>
        public const string SummaryCensusId = "summaryCensusId";

        /// <summary>The collection date.</summary>
        public const string CollectionDate = "collectiondate";

        /// <summary>The environmental service shortage today.</summary>
        public const string EnvironmentalServiceShortageToday = "shortenvsvc";

        /// <summary>The nurse shortage today.</summary>
        public const string NurseShortageToday = "shortnurse";

        /// <summary>The right shortage today.</summary>
        public const string RTShortageToday = "shortrt";

        /// <summary>The pharmacy shortage today.</summary>
        public const string PharmShortageToday = "shortphar";

        /// <summary>The phyisician shortage today.</summary>
        public const string PhysicianShortageToday = "shortphys";

        /// <summary>The temporary shortage today.</summary>
        public const string TempShortageToday = "shorttemp";

        /// <summary>The other shortage today.</summary>
        public const string OtherShortageToday = "shortoth";

        /// <summary>The other licensed shortage today.</summary>
        public const string OtherLicensedShortageToday = "shortothlic";

        /// <summary>The hcp shortage today.</summary>
        public const string HCPShortageToday = "shortothsfy";

        /// <summary>The environmental service shortage week.</summary>
        public const string EnvironmentalServiceShortageWeek = "posshortenvsvc";

        /// <summary>The nurse shortage week.</summary>
        public const string NurseShortageWeek = "posshortnurse";

        /// <summary>The right shortage week.</summary>
        public const string RTShortageWeek = "posshortrt";

        /// <summary>The pharm shortage week.</summary>
        public const string PharmShortageWeek = "posshortphar";

        /// <summary>The physician shortage week.</summary>
        public const string PhysicianShortageWeek = "posshortphys";

        /// <summary>The temporary shortage week.</summary>
        public const string TempShortageWeek = "posshorttemp";

        /// <summary>The other shortage week.</summary>
        public const string OtherShortageWeek = "posshortoth";

        /// <summary>The other licensed shortage week.</summary>
        public const string OtherLicensedShortageWeek = "posshortothlic";

        /// <summary>The hcp shortage week.</summary>
        public const string HCPShortageWeek = "posshortothsfy";

        /// <summary>The group today.</summary>
        public const string GroupToday = "groupToday";

        /// <summary>The group this week.</summary>
        public const string GroupThisWeek = "groupThisWeek";

        /// <summary>The information.</summary>
        public const string Information = "information";

        /// <summary>The current.</summary>
        private static HealthcareWorker _current = new HealthcareWorker();

        /// <summary>The fields.</summary>
        private static readonly Dictionary<string, FormatField> _fields = new Dictionary<string, FormatField>()
        {
            {
                Information,
                new FormatField(
                    Information,
                    "Information",
                    string.Empty,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.None,
                    true,
                    null,
                    null,
                    null)
            },
            {
                GroupToday,
                new FormatField(
                    GroupToday,
                    "CRITICAL STAFFING SHORTAGE TODAY",
                    "Does your organization consider that it has a critical staffing shortage in this group today?",
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.None,
                    true,
                    null,
                    null,
                    null)
            },
            {
                GroupThisWeek,
                new FormatField(
                    GroupThisWeek,
                    "CRITICAL STAFFING SHORTAGE WITHIN A WEEK",
                    "Does your organization anticipate that it will have a critical staffing shortage in this group within one week?",
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.None,
                    true,
                    null,
                    null,
                    null)
            },
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
                    "Date for which Healthcare Worker Impact Module responses are reported",
                    FormatField.FieldType.Date,
                    FormatField.FhirMeasureType.Structure,
                    true,
                    null,
                    null,
                    null)
            },
            {
                EnvironmentalServiceShortageToday,
                new FormatField(
                    EnvironmentalServiceShortageToday,
                    "Environmental services",
                    "Front-line persons who clean patient rooms and all areas in a healthcare facility",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                NurseShortageToday,
                new FormatField(
                    NurseShortageToday,
                    "Nurses",
                    "Registered nurses and licensed practical nurses",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                RTShortageToday,
                new FormatField(
                    RTShortageToday,
                    "Respiratory therapists",
                    "Certified medical professionals who specialize in knowledge and use of mechanical ventilation as well as other programs for respiratory care",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PharmShortageToday,
                new FormatField(
                    PharmShortageToday,
                    "Pharmacists and pharmacy techs",
                    string.Empty,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PhysicianShortageToday,
                new FormatField(
                    PhysicianShortageToday,
                    "Physicians",
                    "Attending physicians, fellows",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherLicensedShortageToday,
                new FormatField(
                    OtherLicensedShortageToday,
                    "Other licensed independent practitioners",
                    "Advanced practice nurses, physician assistants",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                TempShortageToday,
                new FormatField(
                    TempShortageToday,
                    "Temporary physicians, nurses, respiratory therapists, and pharmacists",
                    "'per diems', 'travelers', retired, or other seasonal or intermittently contracted persons",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherShortageToday,
                new FormatField(
                    OtherShortageToday,
                    "Other HCP",
                    "Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                HCPShortageToday,
                new FormatField(
                    HCPShortageToday,
                    "Other HCP - Specify the groups",
                    "Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above",
                    FormatField.FieldType.ShortString,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                EnvironmentalServiceShortageWeek,
                new FormatField(
                    EnvironmentalServiceShortageWeek,
                    "Environmental services",
                    "Front-line persons who clean patient rooms and all areas in a healthcare facility",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                NurseShortageWeek,
                new FormatField(
                    NurseShortageWeek,
                    "Nurses",
                    "Registered nurses and licensed practical nurses",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                RTShortageWeek,
                new FormatField(
                    RTShortageWeek,
                    "Respiratory therapists",
                    "Certified medical professionals who specialize in knowledge and use of mechanical ventilation as well as other programs for respiratory care",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PharmShortageWeek,
                new FormatField(
                    PharmShortageWeek,
                    "Pharmacists and pharmacy techs",
                    string.Empty,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                PhysicianShortageWeek,
                new FormatField(
                    PhysicianShortageWeek,
                    "Physicians",
                    "Attending physicians, fellows",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherLicensedShortageWeek,
                new FormatField(
                    OtherLicensedShortageWeek,
                    "Other licensed independent practitioners",
                    "Advanced practice nurses, physician assistants",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                TempShortageWeek,
                new FormatField(
                    TempShortageWeek,
                    "Temporary physicians, nurses, respiratory therapists, and pharmacists",
                    "'per diems', 'travelers', retired, or other seasonal or intermittently contracted persons",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherShortageWeek,
                new FormatField(
                    OtherShortageWeek,
                    "Other HCP",
                    "Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above.",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                HCPShortageWeek,
                new FormatField(
                    HCPShortageWeek,
                    "Other HCP - Specify the groups",
                    "Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above.",
                    FormatField.FieldType.ShortString,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
        };

        /// <summary>The measure report fields.</summary>
        private static readonly List<string> _measureReportFields = new List<string>
        {
            EnvironmentalServiceShortageToday,
            NurseShortageToday,
            RTShortageToday,
            PharmShortageToday,
            PhysicianShortageToday,
            TempShortageToday,
            OtherShortageToday,
            OtherLicensedShortageToday,
            HCPShortageToday,
            EnvironmentalServiceShortageWeek,
            NurseShortageWeek,
            RTShortageWeek,
            PharmShortageWeek,
            PhysicianShortageWeek,
            TempShortageWeek,
            OtherShortageWeek,
            OtherLicensedShortageWeek,
            HCPShortageWeek,
        };

        /// <summary>The questionnaire sections.</summary>
        private static readonly List<QuestionnaireSection> _questionnaireSections = new List<QuestionnaireSection>()
        {
            new QuestionnaireSection(
                "COVID-19 Module: Healthcare Worker Staffing Pathway",
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(FacilityId),
                    new QuestionnaireQuestion(SummaryCensusId),
                    new QuestionnaireQuestion(CollectionDate),
                }),
            new QuestionnaireSection(
                Information,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(GroupToday),
                    new QuestionnaireQuestion(GroupThisWeek),
                }),
            new QuestionnaireSection(
                EnvironmentalServiceShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        EnvironmentalServiceShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        EnvironmentalServiceShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                NurseShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        NurseShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        NurseShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                RTShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        RTShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        RTShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                PharmShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        PharmShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        PharmShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                PhysicianShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        PhysicianShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        PhysicianShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                TempShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        TempShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        TempShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                OtherLicensedShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        OtherLicensedShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        OtherLicensedShortageWeek,
                        true,
                        GroupThisWeek),
                }),
            new QuestionnaireSection(
                OtherShortageToday,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        OtherShortageToday,
                        true,
                        GroupToday),
                    new QuestionnaireQuestion(
                        HCPShortageToday,
                        true),
                    new QuestionnaireQuestion(
                        OtherShortageWeek,
                        true,
                        GroupThisWeek),
                    new QuestionnaireQuestion(
                        HCPShortageWeek,
                        true),
                }),
        };

        /// <summary>The CSV fields.</summary>
        private static readonly List<string> _csvFields = new List<string>()
        {
            CollectionDate,
            EnvironmentalServiceShortageToday,
            NurseShortageToday,
            RTShortageToday,
            PharmShortageToday,
            PhysicianShortageToday,
            TempShortageToday,
            OtherShortageToday,
            OtherLicensedShortageToday,
            HCPShortageToday,
            EnvironmentalServiceShortageWeek,
            NurseShortageWeek,
            RTShortageWeek,
            PharmShortageWeek,
            PhysicianShortageWeek,
            TempShortageWeek,
            OtherShortageWeek,
            OtherLicensedShortageWeek,
            HCPShortageWeek,
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
                Label = "Instructions for Completion of the COVID-19 Healthcare Worker Staffing Pathway (CDC 57.131)",
                Citation = _cdcCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.131-toi-508.pdf",
                    Creation = "2020-03-27",
                },
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "facility-import-hcw",
                Citation = _cdcCitation,
                Document = new Hl7.Fhir.Model.Attachment()
                {
                    Url = "https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-hcw.csv",
                    Creation = "2020-03-27",
                },
            },
        };

        /// <summary>Gets the current.</summary>
        /// <value>The current.</value>
        public static HealthcareWorker Current => _current;

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name => "sanerCDCHCW";

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title => "SANER CDC COVID-19 Healthcare Worker Staffing Pathway";

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description => "SANER implementation of the CDC COVID-19 Healthcare Worker Staffing Pathway";

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
