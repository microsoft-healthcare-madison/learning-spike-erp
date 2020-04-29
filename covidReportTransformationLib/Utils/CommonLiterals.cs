// <copyright file="CommonLiterals.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Utils
{
    /// <summary>A common literals.</summary>
    public abstract class CommonLiterals
    {
        /// <summary>The FHIR identifier prefix.</summary>
        public const string FhirIdPrefix = "FHIR-";

        /// <summary>The hospital prefix.</summary>
        public const string OrgPrefix = "Org-";

        /// <summary>The root location prefix.</summary>
        public const string RootLocationPrefix = "Loc-";

        /// <summary>The organization type provider.</summary>
        public const string OrganizationTypeProvider = "prov";

        /// <summary>The location physical type bed.</summary>
        public const string LocationPhysicalTypeBed = "bd";

        /// <summary>The location physical type site.</summary>
        public const string LocationPhysicalTypeSite = "si";

        /// <summary>The sct covid code.</summary>
        public const string SctCovidCode = "840539006";

        /// <summary>The sct covid display.</summary>
        public const string SctCovidDisplay = "COVID-19";

        /// <summary>The fake code text.</summary>
        public const string FakeCodeText = "This code is not used, but is required.";

        /// <summary>The context focus.</summary>
        public const string ContextFocus = "focus";

        /// <summary>This location is operational (but may be in use).</summary>
        public const string AvailabilityStatusActive = "active";

        /// <summary>This location is temporarily out of service.</summary>
        public const string AvailabilityStatusSuspended = "suspended";

        /// <summary>This location is no longer operational.</summary>
        public const string AvailabilityStatusInactive = "inactive";

        /// <summary>The availability statuses.</summary>
        public static readonly string[] AvailabilityStatuses =
        {
            AvailabilityStatusActive,
            AvailabilityStatusInactive,
            AvailabilityStatusSuspended,
        };

        /// <summary>This bed needs decontamination before it can be readied for use.</summary>
        public const string OperationalStatusContaminated = "K";

        /// <summary>This bed is no longer in service.</summary>
        public const string OperationalStatusClosed = "C";

        /// <summary>This bed is not in use, but is presently ready for use.</summary>
        public const string OperationalStatusHousekeeping = "H";

        /// <summary>This bed is presently in use.</summary>
        public const string OperationalStatusOccupied = "O";

        /// <summary>This bed is presently ready for use.</summary>
        public const string OperationalStatusUnoccupied = "U";

        /// <summary>The operational statuses.</summary>
        public static readonly string[] OperationalStatuses =
        {
            OperationalStatusContaminated,
            OperationalStatusClosed,
            OperationalStatusHousekeeping,
            OperationalStatusOccupied,
            OperationalStatusUnoccupied,
        };

        /// <summary>The bed type adult icu.</summary>
        public const string BedTypeAdultICU = "ICU";

        /// <summary>The bed type pediactric icu.</summary>
        public const string BedTypePediatricICU = "PEDICU";

        /// <summary>The bed type neonatal icu.</summary>
        public const string BedTypeNeonatalICU = "PEDNICU";

        /// <summary>The bed type emergency room.</summary>
        public const string BedTypeEmergencyRoom = "ER";

        /// <summary>The bed type hospital unit.</summary>
        public const string BedTypeHospitalUnit = "HU";

        /// <summary>The bed type rehab long term care.</summary>
        public const string BedTypeRehabLongTermCare = "RHU";

        /// <summary>The bed type pediatric.</summary>
        public const string BedTypePediatric = "PEDU";

        /// <summary>The bed type psychiatric.</summary>
        public const string BedTypePsychiatric = "PHU";

        /// <summary>The bed type operating room.</summary>
        public const string BedTypeOperatingRoom = "OR";

        /// <summary>List of types of the bed.</summary>
        public static readonly string[] BedTypes =
        {
            BedTypeAdultICU,
            BedTypePediatricICU,
            BedTypeNeonatalICU,
            BedTypeEmergencyRoom,
            BedTypeHospitalUnit,
            BedTypeRehabLongTermCare,
            BedTypePediatric,
            BedTypePsychiatric,
            BedTypeOperatingRoom,
        };

        /// <summary>Negative airflow isolation beds.</summary>
        public const string BedFeatureNegativeFlowIsolation = "NEGISO";

        /// <summary>Isolation beds (airflow is not a concern).</summary>
        public const string BedFeatureOtherIsolation = "OTHISO";

        /// <summary>Bed in a unit that does not support isolation.</summary>
        public const string BedFeatureNonIsolating = "NONISO";

        /// <summary>The bed features.</summary>
        public static readonly string[] BedFeatures =
        {
            BedFeatureNegativeFlowIsolation,
            BedFeatureOtherIsolation,
            BedFeatureNonIsolating,
        };

        /// <summary>The scoring.</summary>
        public const string Scoring = "scoring";

        /// <summary>The subject.</summary>
        public const string Subject = "subject";

        /// <summary>The type.</summary>
        public const string Type = "type";

        /// <summary>The improvement notation.</summary>
        public const string ImprovementNotation = "improvementNotation";

        /// <summary>The rate aggregation.</summary>
        public const string RateAggregation = "rateAggregation";

        /// <summary>The aggregable by period.</summary>
        public const string AggregableByPeriod = "aggregable-by-period";

        /// <summary>Values that represent SANER-IG characteristics.</summary>
        public enum SanerCharacteristic
        {
            /// <summary>Location Status.</summary>
            Status,

            /// <summary>Location Operational Status.</summary>
            OperationalStatus,

            /// <summary>Location Role Type.</summary>
            Type,

            /// <summary>Bed Feature.</summary>
            Feature,

            /// <summary>Parent location.</summary>
            Location,

            /// <summary>Time this group represents.</summary>
            Period,
        }
    }
}
