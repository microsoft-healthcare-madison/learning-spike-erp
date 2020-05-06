// <copyright file="AcuteHealthcareSupply.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Utils;

namespace covidReportTransformationLib.Formats.CDC
{
    /// <summary>An acute healthcare supply.</summary>
    public class AcuteHealthcareSupply : IReportingFormat
    {
        /// <summary>Identifier for the facility.</summary>
        public const string FacilityId = "facilityId";

        /// <summary>Identifier for the summary census.</summary>
        public const string SummaryCensusId = "summaryCensusId";

        /// <summary>The collection date.</summary>
        public const string CollectionDate = "collectiondate";

        /// <summary>The ventilator base.</summary>
        public const string VentilatorBase = "vent";

        /// <summary>Information describing the ventilator.</summary>
        public const string VentilatorDesc = "Ventilator supplies (any, including tubing)";

        /// <summary>The ventilator supplies.</summary>
        public const string VentilatorSupply = "ventsupply";

        /// <summary>The ventilator reuse.</summary>
        public const string VentilatorReuse = "ventreuse";

        /// <summary>The ventilator obtain.</summary>
        public const string VentilatorObtain = "ventobtain";

        /// <summary>The 95 mask base.</summary>
        public const string N95MaskBase = "n95mask";

        /// <summary>Information describing the 95 mask.</summary>
        public const string N95MaskDesc = "N95 masks";

        /// <summary>The 95 mask supplies.</summary>
        public const string N95MaskSupply = "n95masksupply";

        /// <summary>The 95 mask reuse.</summary>
        public const string N95MaskReuse = "n95maskreuse";

        /// <summary>The 95 mask obtain.</summary>
        public const string N95MaskObtain = "n95maskobtain";

        /// <summary>The other resp supplies.</summary>
        public const string OtherRespBase = "otherresp";

        /// <summary>Information describing the other resp.</summary>
        public const string OtherRespDesc = "Other respirators including PAPRs";

        /// <summary>The other resp supplies.</summary>
        public const string OtherRespSupply = "othrespsupply";

        /// <summary>The other resp reuse.</summary>
        public const string OtherRespReuse = "othrespreuse";

        /// <summary>The other resp obtain.</summary>
        public const string OtherRespObtain = "othrespobtain";

        /// <summary>The surgical mask base.</summary>
        public const string SurgicalMaskBase = "surgmask";

        /// <summary>Information describing the surgical mask.</summary>
        public const string SurgicalMaskDesc = "Surgical masks";

        /// <summary>The surgical mask supply.</summary>
        public const string SurgicalMaskSupply = "surgmasksupply";

        /// <summary>The surgical mask reuse.</summary>
        public const string SurgicalMaskReuse = "surgmaskreuse";

        /// <summary>The surgical mask obtain.</summary>
        public const string SurgicalMaskObtain = "surgmaskobtain";

        /// <summary>The face shield base.</summary>
        public const string FaceShieldBase = "shield";

        /// <summary>Information describing the face shield.</summary>
        public const string FaceShieldDesc = "Eye protection including face shields or goggles";

        /// <summary>The face shield supply.</summary>
        public const string FaceShieldSupply = "shieldsupply";

        /// <summary>The face shield reuse.</summary>
        public const string FaceShieldReuse = "shieldreuse";

        /// <summary>The face shield obtain.</summary>
        public const string FaceShieldObtain = "shieldobtain";

        /// <summary>The gown base.</summary>
        public const string GownBase = "gown";

        /// <summary>Information describing the gown.</summary>
        public const string GownDesc = "Gowns (single use)";

        /// <summary>The gown supply.</summary>
        public const string GownSupply = "gownsupply";

        /// <summary>The gown reuse.</summary>
        public const string GownReuse = "gownreuse";

        /// <summary>The gown obtain.</summary>
        public const string GownObtain = "gownobtain";

        /// <summary>The glove base.</summary>
        public const string GloveBase = "glove";

        /// <summary>Information describing the glove.</summary>
        public const string GloveDesc = "Gloves";

        /// <summary>The glove supply.</summary>
        public const string GloveSupply = "glovesupply";

        /// <summary>The glove reuse.</summary>
        public const string GloveReuse = "glovereuse";

        /// <summary>The glove obtain.</summary>
        public const string GloveObtain = "gloveobtain";

        /// <summary>The information.</summary>
        public const string Information = "information";

        /// <summary>The group on hand supply.</summary>
        public const string GroupOnHandSupply = "onHandSupply";

        /// <summary>The group reuse or extended use.</summary>
        public const string GroupReuseOrExtendedUse = "reuseOrExtendedUse";

        /// <summary>The group able to obtain.</summary>
        public const string GroupAbleToObtain = "ableToObtain";

        /// <summary>The description hand supply.</summary>
        private const string DescOnHandSupply = "Please answer based on your organization’s best estimation for the supply of this item.";

        /// <summary>The description reuse.</summary>
        private const string DescReuse = "Are you currently reusing the item or implementing extended use?";

        /// <summary>The description able to obtain.</summary>
        private const string DescAbleToObtain = "Are you able to obtain this item? If you have placed an order but  are not able to have that order filled, please answer NO.";

        /// <summary>The current.</summary>
        private static AcuteHealthcareSupply _current = new AcuteHealthcareSupply();

        /// <summary>The supply choices.</summary>
        private static readonly List<FormatFieldOption> _supplyChoices = new List<FormatFieldOption>()
        {
            new FormatFieldOption("Zero days", true),
            new FormatFieldOption("1-3 days", true),
            new FormatFieldOption("4-14 days", true),
            new FormatFieldOption("15 or more days", true),
        };

        /// <summary>The fields.</summary>
        private static readonly Dictionary<string, FormatField> _fields = new Dictionary<string, FormatField>()
        {
            {
                Information,
                new FormatField(
                    Information,
                    "Information",
                    null,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.None,
                    true,
                    null,
                    null,
                    null)
            },
            {
                GroupOnHandSupply,
                new FormatField(
                    GroupOnHandSupply,
                    "On-hand Supply",
                    DescOnHandSupply,
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.None,
                    true,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                GroupReuseOrExtendedUse,
                new FormatField(
                    GroupReuseOrExtendedUse,
                    "Reusing or Extending Use",
                    DescReuse,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.None,
                    true,
                    null,
                    null,
                    null)
            },
            {
                GroupAbleToObtain,
                new FormatField(
                    GroupAbleToObtain,
                    "Able to Obtain",
                    DescAbleToObtain,
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
                    null,
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
                    null,
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
                VentilatorBase,
                new FormatField(
                    VentilatorBase,
                    VentilatorDesc,
                    "any supplies, including flow sensors, tubing, connectors, valves, filters, etc",
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                VentilatorSupply,
                new FormatField(
                    VentilatorSupply,
                    VentilatorDesc,
                    "any supplies, including flow sensors, tubing, connectors, valves, filters, etc",
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                VentilatorReuse,
                new FormatField(
                    VentilatorReuse,
                    VentilatorDesc,
                    "any supplies, including flow sensors, tubing, connectors, valves, filters, etc",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                VentilatorObtain,
                new FormatField(
                    VentilatorObtain,
                    VentilatorDesc,
                    "any supplies, including flow sensors, tubing, connectors, valves, filters, etc",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                N95MaskBase,
                new FormatField(
                    N95MaskBase,
                    N95MaskDesc,
                    null,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                N95MaskSupply,
                new FormatField(
                    N95MaskSupply,
                    N95MaskDesc,
                    null,
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                N95MaskReuse,
                new FormatField(
                    N95MaskReuse,
                    N95MaskDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                N95MaskObtain,
                new FormatField(
                    N95MaskObtain,
                    N95MaskDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherRespBase,
                new FormatField(
                    OtherRespBase,
                    OtherRespDesc,
                    "Other respirators such as PAPRs or elastomerics",
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherRespSupply,
                new FormatField(
                    OtherRespSupply,
                    OtherRespDesc,
                    "Other respirators such as PAPRs or elastomerics",
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                OtherRespReuse,
                new FormatField(
                    OtherRespReuse,
                    OtherRespDesc,
                    "Other respirators such as PAPRs or elastomerics",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                OtherRespObtain,
                new FormatField(
                    OtherRespObtain,
                    OtherRespDesc,
                    "Other respirators such as PAPRs or elastomerics",
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                SurgicalMaskBase,
                new FormatField(
                    SurgicalMaskBase,
                    SurgicalMaskDesc,
                    null,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                SurgicalMaskSupply,
                new FormatField(
                    SurgicalMaskSupply,
                    SurgicalMaskDesc,
                    null,
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                SurgicalMaskReuse,
                new FormatField(
                    SurgicalMaskReuse,
                    SurgicalMaskDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                SurgicalMaskObtain,
                new FormatField(
                    SurgicalMaskObtain,
                    SurgicalMaskDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                FaceShieldBase,
                new FormatField(
                    FaceShieldBase,
                    FaceShieldDesc,
                    null,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                FaceShieldSupply,
                new FormatField(
                    FaceShieldSupply,
                    FaceShieldDesc,
                    null,
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                FaceShieldReuse,
                new FormatField(
                    FaceShieldReuse,
                    FaceShieldDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                FaceShieldObtain,
                new FormatField(
                    FaceShieldObtain,
                    FaceShieldDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                GownBase,
                new FormatField(
                    GownBase,
                    GownDesc,
                    null,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                GownSupply,
                new FormatField(
                    GownSupply,
                    GownDesc,
                    null,
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                GownReuse,
                new FormatField(
                    GownReuse,
                    GownDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                GownObtain,
                new FormatField(
                    GownObtain,
                    GownDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                GloveBase,
                new FormatField(
                    GloveBase,
                    GloveDesc,
                    null,
                    FormatField.FieldType.Display,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                GloveSupply,
                new FormatField(
                    GloveSupply,
                    GloveDesc,
                    null,
                    FormatField.FieldType.Choice,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    _supplyChoices)
            },
            {
                GloveReuse,
                new FormatField(
                    GloveReuse,
                    GloveDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
            {
                GloveObtain,
                new FormatField(
                    GloveObtain,
                    GloveDesc,
                    null,
                    FormatField.FieldType.Boolean,
                    FormatField.FhirMeasureType.Structure,
                    false,
                    null,
                    null,
                    null)
            },
        };

#if true
        /// <summary>The measure groupings.</summary>
        private static readonly List<MeasureGrouping> _measureGroupings = new List<MeasureGrouping>()
        {
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "Ventilators",
                    VentilatorDesc),
                "Ventilator Supply Reporting",
                MeasureGroupingExtension.VentilatorList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(VentilatorSupply, null),
                    new MeasureGroupingPopulation(VentilatorReuse, null),
                    new MeasureGroupingPopulation(VentilatorObtain, null),
                }),
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "N95Masks",
                    N95MaskDesc),
                "N95 Mask Supply Reporting",
                MeasureGroupingExtension.UnspecifiedList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(N95MaskSupply, null),
                    new MeasureGroupingPopulation(N95MaskReuse, null),
                    new MeasureGroupingPopulation(N95MaskObtain, null),
                }),
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "OtherRespirators",
                    OtherRespDesc),
                "Other Respirator Supply Reporting",
                MeasureGroupingExtension.UnspecifiedList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(OtherRespSupply, null),
                    new MeasureGroupingPopulation(OtherRespReuse, null),
                    new MeasureGroupingPopulation(OtherRespObtain, null),
                }),
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "SurgicalMasks",
                    SurgicalMaskDesc),
                "Surgical Mask Supply Reporting",
                MeasureGroupingExtension.UnspecifiedList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(SurgicalMaskSupply, null),
                    new MeasureGroupingPopulation(SurgicalMaskReuse, null),
                    new MeasureGroupingPopulation(SurgicalMaskObtain, null),
                }),
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "EyeProtection",
                    FaceShieldDesc),
                "Eye Protection Supply Reporting",
                MeasureGroupingExtension.UnspecifiedList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(FaceShieldSupply, null),
                    new MeasureGroupingPopulation(FaceShieldReuse, null),
                    new MeasureGroupingPopulation(FaceShieldObtain, null),
                }),
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "Gown",
                    GownDesc),
                "Gown Supply Reporting",
                MeasureGroupingExtension.UnspecifiedList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(GownSupply, null),
                    new MeasureGroupingPopulation(GownReuse, null),
                    new MeasureGroupingPopulation(GownObtain, null),
                }),
            new MeasureGrouping(
                new FhirTriplet(
                    FhirSystems.SanerGroup,
                    "Glove",
                    GloveDesc),
                "Glove Supply Reporting",
                MeasureGroupingExtension.UnspecifiedList,
                new List<MeasureGroupingPopulation>()
                {
                    new MeasureGroupingPopulation(GloveSupply, null),
                    new MeasureGroupingPopulation(GloveReuse, null),
                    new MeasureGroupingPopulation(GloveObtain, null),
                }),
        };
#else
        /// <summary>The measure groupings.</summary>
        private static readonly List<MeasureGrouping> _measureGroupings = new List<MeasureGrouping>()
        {
            new MeasureGrouping(VentilatorSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(VentilatorReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(VentilatorObtain, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(N95MaskSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(N95MaskReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(N95MaskObtain, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherRespSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherRespReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(OtherRespObtain, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(SurgicalMaskSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(SurgicalMaskReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(SurgicalMaskObtain, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(FaceShieldSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(FaceShieldReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(FaceShieldObtain, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(GownSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(GownReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(GownObtain, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(GloveSupply, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(GloveReuse, MeasureGroupingExtension.UnspecifiedList),
            new MeasureGrouping(GloveObtain, MeasureGroupingExtension.UnspecifiedList),
        };
#endif

        /// <summary>The questionnaire sections.</summary>
        private static readonly List<QuestionnaireSection> _questionnaireSections = new List<QuestionnaireSection>()
        {
            new QuestionnaireSection(
                "COVID-19 Module: Healthcare Supply Pathway",
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
                    new QuestionnaireQuestion(GroupOnHandSupply),
                    new QuestionnaireQuestion(GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                VentilatorBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        VentilatorSupply,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        VentilatorReuse,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        VentilatorObtain,
                        true,
                        GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                N95MaskBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        N95MaskSupply,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        N95MaskReuse,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        N95MaskObtain,
                        true,
                        GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                OtherRespBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        OtherRespSupply,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        OtherRespReuse,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        OtherRespObtain,
                        true,
                        GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                SurgicalMaskBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        SurgicalMaskDesc,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        SurgicalMaskSupply,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        SurgicalMaskReuse,
                        true,
                        GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                FaceShieldBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        FaceShieldSupply,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        FaceShieldReuse,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        FaceShieldObtain,
                        true,
                        GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                GownBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        GownSupply,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        GownReuse,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        GownObtain,
                        true,
                        GroupAbleToObtain),
                }),
            new QuestionnaireSection(
                GloveBase,
                new List<QuestionnaireQuestion>()
                {
                    new QuestionnaireQuestion(
                        GloveSupply,
                        true,
                        GroupOnHandSupply),
                    new QuestionnaireQuestion(
                        GloveReuse,
                        true,
                        GroupReuseOrExtendedUse),
                    new QuestionnaireQuestion(
                        GloveObtain,
                        true,
                        GroupAbleToObtain),
                }),
        };

        /// <summary>The CSV fields.</summary>
        private static readonly List<string> _csvFields = new List<string>()
        {
            CollectionDate,
            VentilatorSupply,
            VentilatorReuse,
            VentilatorObtain,
            N95MaskSupply,
            N95MaskReuse,
            N95MaskObtain,
            OtherRespSupply,
            OtherRespReuse,
            OtherRespObtain,
            SurgicalMaskSupply,
            SurgicalMaskReuse,
            SurgicalMaskObtain,
            FaceShieldSupply,
            FaceShieldReuse,
            FaceShieldObtain,
            GownSupply,
            GownReuse,
            GownObtain,
            GloveSupply,
            GloveReuse,
            GloveObtain,
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
                Display = "Instructions for Completion of the COVID-19 Healthcare Supply Pathway (CDC 57.132)",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.132-toi-508.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "PDF Form",
                Display = "Healthcare Supply Pathway Form",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.132-covid19-sup-blank-p.pdf",
                Citation = _cdcCitation,
            },
            new Hl7.Fhir.Model.RelatedArtifact()
            {
                Type = Hl7.Fhir.Model.RelatedArtifact.RelatedArtifactType.Documentation,
                Label = "CSV File Template",
                Display = "CDC/NHSN COVID-19 Acute Care Healthcare Supply Reporting CSV File Template",
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-supplies.csv",
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
        public static AcuteHealthcareSupply Current => _current;

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name => "CDCHealthcareSupplyPathway";

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title => "COVID-19 Healthcare Supply Pathway";

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description => "SANER implementation of the CDC COVID-19 Healthcare Supply Pathway";

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
