// <copyright file="FhirSystems.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Utils
{
    /// <summary>A listing of FHIR system urls.</summary>
    public abstract class FhirSystems
    {
        /// <summary>The activity reason.</summary>
        public const string ActivityReason = "http://terminology.hl7.org/CodeSystem/v3-ActReason";

        /// <summary>The system availability status.</summary>
        public const string AvailabilityStatus = "http://hl7.org/fhir/location-status";

        /// <summary>Type of the system bed.</summary>
        public const string BedType = "http://terminology.hl7.org/CodeSystem/v3-RoleCode";

        /// <summary>The system measure report.</summary>
        public const string Bundle = "http://hl7.org/fhir/R4/StructureDefinition/Bundle";

        /// <summary>The improvement notation.</summary>
        public const string ImprovementNotation = "http://terminology.hl7.org/CodeSystem/measure-improvement-notation";

        /// <summary>The internal system.</summary>
        public const string Internal = "https://github.com/microsoft-healthcare-madison/learning-spike-erp/";

        /// <summary>The ISO country.</summary>
        public const string IsoCountry = "urn:iso:std:iso:3166";

        /// <summary>Type of the system location physical.</summary>
        public const string LocationPhysicalType = "http://terminology.hl7.org/CodeSystem/location-physical-type";

        /// <summary>The measure report measurement.</summary>
        public const string MeasureReportMeasurement = "https://audaciousinquiry.github.io/saner-ig/Measure/bed-availability-measure";

        /// <summary>The measure population.</summary>
        public const string MeasurePopulation = "http://terminology.hl7.org/CodeSystem/measure-population";

        /// <summary>The system measure report.</summary>
        public const string Measure = "http://hl7.org/fhir/R4/StructureDefinition/Measure";

        /// <summary>The system measure report.</summary>
        public const string MeasureReport = "http://hl7.org/fhir/R4/StructureDefinition/MeasureReport";

        /// <summary>The measure scoring.</summary>
        public const string MeasureScoring = "http://terminology.hl7.org/CodeSystem/measure-scoring";

        /// <summary>Type of the measure.</summary>
        public const string MeasureType = "http://terminology.hl7.org/CodeSystem/measure-type";

        /// <summary>The system operational status.</summary>
        public const string OperationalStatus = "http://terminology.hl7.org/CodeSystem/v2-0116";

        /// <summary>Type of the organization.</summary>
        public const string OrganizationType = "http://terminology.hl7.org/CodeSystem/organization-type";

        /// <summary>The system Questionnaire.</summary>
        public const string Questionnaire = "http://hl7.org/fhir/R4/StructureDefinition/Questionnaire";

        /// <summary>The system Questionnaire response.</summary>
        public const string QuestionnaireResponse = "http://hl7.org/fhir/R4/StructureDefinition/QuestionnaireResponse";

        /// <summary>Type of the resource.</summary>
        public const string ResourceType = "http://hl7.org/fhir/resource-types";

        /// <summary>The SANER-IG characteristic system.</summary>
        public const string SanerCharacteristic = "http://hl7.org/fhir/R4/StructureDefinition/Location";

        /// <summary>The system saner bed feature.</summary>
        public const string SanerBedFeature = "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType";

        /// <summary>The saner aggregate bool.</summary>
        public const string SanerAggregateBool = "http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation";

        /// <summary>The saner aggregate choice.</summary>
        public const string SanerAggregateChoice = "http://hl7.org/fhir/us/saner/CodeSystem/ChoiceAggregation";

        /// <summary>Group the saner belongs to.</summary>
        public const string SanerGroup = "http://hl7.org/fhir/us/saner/CodeSystem/GroupSystem";

        /// <summary>The saner population.</summary>
        public const string SanerPopulation = "http://hl7.org/fhir/us/saner/CodeSystem/PopulationSystem";

        /// <summary>The snomed sct.</summary>
        public const string SnomedSct = "http://snomed.info/sct";

        /// <summary>URL of the system.</summary>
        public const string Url = "url";

        /// <summary>Type of the system usage context.</summary>
        public const string UsageContextType = "http://terminology.hl7.org/CodeSystem/usage-context-type";
    }
}
