// <copyright file="AcuteHealthcareWorker.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Utils;

namespace covidReportTransformationLib.Formats.CDC
{
    /// <summary>A healthcare worker.</summary>
    public class AcuteHealthcareWorker : IReportingFormat
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
        private static AcuteHealthcareWorker _current = new AcuteHealthcareWorker();

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
                    "Critical Staffing Shortage Today",
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
                    "Critical Staffing Shortage Within a Week",
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
                    "Select the date for which the recorded data was collected for the following questions.",
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

        // TODO(GinoC): Need to figure out groupings for these
        // TODO(GinoC): Need to figure out what 'specify' field aggregation (HCP*) should look like
        /// <summary>The measure groupings.</summary>
        private static readonly List<MeasureGrouping> _measureGroupings = new List<MeasureGrouping>()
        {
            new MeasureGrouping(EnvironmentalServiceShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(NurseShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(RTShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(PharmShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(PhysicianShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(TempShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherLicensedShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(HCPShortageToday, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(EnvironmentalServiceShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(NurseShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(RTShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(PharmShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(PhysicianShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(TempShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherLicensedShortageWeek, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(HCPShortageWeek, MeasureGroupingExtension.UnspecifiedList),
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
                Label = "NHSN COVID-19 Reporting for Acute Care",
                Display = "CDC/NHSN COVID-19 Acute Care Module Home Page",
                Url = "https://www.cdc.gov/nhsn/acute-care-hospital/covid19/",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "How to import COVID-19 Summary Data",
                Display = "Facility - How to Upload COVID-19 CSV Data Files",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "COVID-19 Module Analysis Reports",
                Display = "NHSN COVID-19 Module Analysis Reports",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "Table of Instructions",
                Display = "Instructions for Completion of the COVID-19 Healthcare Worker Staffing Pathway (CDC 57.131)",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.131-toi-508.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "PDF Form",
                Display = "Healthcare Worker Staffing Pathway Form",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.131-covid19-hwp-blank-p.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "CSV File Template",
                Display = "CDC/NHSN COVID-19 Acute Care Healthcare Supply Reporting CSV File Template",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-hcw.csv",
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
        public static AcuteHealthcareWorker Current => _current;

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name => "CDCHealthcareWorkerStaffingPathway";

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title => "COVID-19 Healthcare Worker Staffing Pathway";

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description => "SANER implementation of the CDC COVID-19 Healthcare Worker Staffing Pathway";

        /// <summary>Gets the definition.</summary>
        /// <value>The definition.</value>
        public List<string> Definition => null;

        /// <summary>Gets the fields.</summary>
        /// <value>The fields.</value>
        public Dictionary<string, FormatField> Fields => _fields;

        /// <summary>Gets the measure groupings.</summary>
        /// <value>The measure groupings.</value>
        public List<MeasureGrouping> MeasureGroupings => _measureGroupings;

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

        /// <summary>Gets a value indicating whether the measure report is enabled.</summary>
        /// <value>True if enable measure report, false if not.</value>
        public bool EnableMeasureReport => false;
    }
}
