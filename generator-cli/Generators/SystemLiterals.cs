// <copyright file="SystemLiterals.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace generator_cli.Generators
{
    /// <summary>A code systems.</summary>
    public abstract class SystemLiterals
    {
        /// <summary>The measure report measurement.</summary>
        public const string MeasureReportMeasurement = "https://audaciousinquiry.github.io/saner-ig/Measure/bed-availability-measure";

        /// <summary>The internal system.</summary>
        public const string Internal = "https://github.com/microsoft-healthcare-madison/learning-spike-erp/";

        /// <summary>The SANER-IG characteristic system.</summary>
        public const string SanerCharacteristic = "http://hl7.org/fhir/R4/StructureDefinition/Location";

        /// <summary>The system saner bed feature.</summary>
        public const string SanerBedFeature = "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType";

        /// <summary>Type of the system location physical.</summary>
        public const string LocationPhysicalType = "http://terminology.hl7.org/CodeSystem/location-physical-type";

        /// <summary>The system measure report.</summary>
        public const string MeasureReport = "http://hl7.org/fhir/R4/StructureDefinition/MeasureReport";

        /// <summary>Type of the organization.</summary>
        public const string OrganizationType = "http://hl7.org/fhir/CodeSystem/organization-type";

        /// <summary>The system availability status.</summary>
        public const string AvailabilityStatus = "http://hl7.org/fhir/location-status";

        /// <summary>The system operational status.</summary>
        public const string OperationalStatus = "http://terminology.hl7.org/CodeSystem/v2-0116";

        /// <summary>Type of the system bed.</summary>
        public const string BedType = "http://terminology.hl7.org/CodeSystem/v3-RoleCode";

        /// <summary>URL of the system.</summary>
        public const string Url = "url";

        /// <summary>Type of the system usage context.</summary>
        public const string UsageContextType = "http://terminology.hl7.org/CodeSystem/usage-context-type";

        /// <summary>The snomed sct.</summary>
        public const string SnomedSct = "http://snomed.info/sct";
    }
}
