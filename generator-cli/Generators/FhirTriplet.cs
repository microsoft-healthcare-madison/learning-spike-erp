// <copyright file="FhirTriplet.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;
using static generator_cli.Generators.FhirGenerator;

namespace generator_cli.Generators
{
    /// <summary>A fhir triplet.</summary>
    public class FhirTriplet
    {
        /// <summary>The fake code text.</summary>
        private const string _fakeCodeText = "This code is not used, but is required.";

        /// <summary>
        /// Initializes a new instance of the <see cref="FhirTriplet"/> class.
        /// </summary>
        /// <param name="system"> The system.</param>
        /// <param name="code">   The code.</param>
        /// <param name="display">The display.</param>
        public FhirTriplet(
            string system,
            string code,
            string display)
        {
            if (string.IsNullOrEmpty(system))
            {
                System = string.Empty;
            }
            else
            {
                System = system;
            }

            if (string.IsNullOrEmpty(code))
            {
                Code = string.Empty;
            }
            else
            {
                Code = code;
            }

            if (string.IsNullOrEmpty(display))
            {
                Display = string.Empty;
            }
            else
            {
                Display = display;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="FhirTriplet"/> class.</summary>
        /// <param name="system">The system.</param>
        /// <param name="code">  The code.</param>
        public FhirTriplet(
            string system,
            string code)
            : this(system, code, string.Empty)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FhirTriplet"/> class.</summary>
        private FhirTriplet()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>Gets the system.</summary>
        /// <value>The system.</value>
        public string System { get; }

        /// <summary>Gets the code.</summary>
        /// <value>The code.</value>
        public string Code { get; }

        /// <summary>Gets the display.</summary>
        /// <value>The display.</value>
        public string Display { get; }

        /// <summary>Gets the concept.</summary>
        /// <value>The concept.</value>
        public CodeableConcept Concept => new CodeableConcept(System, Code, Display);

        /// <summary>Gets the coding.</summary>
        /// <value>The coding.</value>
        public Coding Coding => new Coding(System, Code, Display);

        /// <summary>Gets the physical type bed.</summary>
        /// <value>The physical type bed.</value>
        public static FhirTriplet PhysicalTypeBed => new FhirTriplet(SystemLocationPhysicalType, "bd");

        /// <summary>Gets the physical type site.</summary>
        /// <value>The physical type site.</value>
        public static FhirTriplet PhysicalTypeSite => new FhirTriplet(SystemLocationPhysicalType, "si");

        /// <summary>The measure report period.</summary>
        public static FhirTriplet MeasureReportPeriod = new FhirTriplet(SystemMeasureReport, "MeasureReport.period");

        /// <summary>The empty required.</summary>
        public static FhirTriplet EmptyRequired = new FhirTriplet(string.Empty, string.Empty, _fakeCodeText);

        /// <summary>Gets the saner status.</summary>
        /// <value>The saner status.</value>
        public static FhirTriplet SanerAvailabilityStatus => Saner(SanerCharacteristic.Status);

        /// <summary>Gets the saner operational status.</summary>
        /// <value>The saner operational status.</value>
        public static FhirTriplet SanerOperationalStatus => Saner(SanerCharacteristic.OperationalStatus);

        /// <summary>Gets the type of the saner.</summary>
        /// <value>The type of the saner.</value>
        public static FhirTriplet SanerType => Saner(SanerCharacteristic.Type);

        /// <summary>Gets the saner feature.</summary>
        /// <value>The saner feature.</value>
        public static FhirTriplet SanerFeature => Saner(SanerCharacteristic.Feature);

        /// <summary>Gets the saner location.</summary>
        /// <value>The saner location.</value>
        public static FhirTriplet SanerLocation => Saner(SanerCharacteristic.Location);

        /// <summary>Gets the saner period.</summary>
        /// <value>The saner period.</value>
        public static FhirTriplet SanerPeriod => Saner(SanerCharacteristic.Period);

        /// <summary>For saner.</summary>
        /// <param name="characteristic">The characteristic.</param>
        /// <returns>A FhirTriplet.</returns>
        public static FhirTriplet Saner(SanerCharacteristic characteristic)
        {
            switch (characteristic)
            {
                case SanerCharacteristic.Status:
                    return new FhirTriplet(
                        SystemSanerCharacteristic,
                        "Location.status");

                case SanerCharacteristic.OperationalStatus:
                    return new FhirTriplet(
                        SystemSanerCharacteristic,
                        "Location.operationalStatus");

                case SanerCharacteristic.Type:
                    return new FhirTriplet(
                        SystemSanerCharacteristic,
                        "Location.type");

                case SanerCharacteristic.Feature:
                    return new FhirTriplet(
                        SystemSanerCharacteristic,
                        "Location.Feature");

                case SanerCharacteristic.Location:
                    return new FhirTriplet(
                        SystemSanerCharacteristic,
                        "Location.partOf");

                case SanerCharacteristic.Period:
                    return new FhirTriplet(
                        SystemSanerCharacteristic,
                        "Period");
            }

            return new FhirTriplet();
        }

        /// <summary>Availability status.</summary>
        /// <param name="status">The status.</param>
        /// <returns>A FhirTriplet.</returns>
        public static FhirTriplet AvailabilityStatus(string status)
        {
            switch (status)
            {
                case AvailabilityStatusActive:
                    return new FhirTriplet(
                        SystemAvailabilityStatus,
                        "active",
                        "The location is operational.");

                case AvailabilityStatusSuspended:
                    return new FhirTriplet(
                        SystemAvailabilityStatus,
                        "suspended",
                        "The location is temporarily closed.");

                case AvailabilityStatusInactive:
                    return new FhirTriplet(
                        SystemAvailabilityStatus,
                        "inactive",
                        "The location is no longer used.");
            }

            return new FhirTriplet();
        }

        /// <summary>Operational status.</summary>
        /// <param name="status">The status.</param>
        /// <returns>A FhirTriplet.</returns>
        public static FhirTriplet OperationalStatus(string status)
        {
            switch (status)
            {
                case OperationalStatusContaminated:
                    return new FhirTriplet(
                        SystemOperationalStatus,
                        "K",
                        "Contaminated");

                case OperationalStatusClosed:
                    return new FhirTriplet(
                        SystemOperationalStatus,
                        "C",
                        "Closed");

                case OperationalStatusHousekeeping:
                    return new FhirTriplet(
                        SystemOperationalStatus,
                        "H",
                        "Housekeeping");

                case OperationalStatusOccupied:
                    return new FhirTriplet(
                        SystemOperationalStatus,
                        "O",
                        "Occupied");

                case OperationalStatusUnoccupied:
                    return new FhirTriplet(
                        SystemOperationalStatus,
                        "U",
                        "Unoccupied");
            }

            return new FhirTriplet();
        }

        /// <summary>Bed type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>A FhirTriplet.</returns>
        public static FhirTriplet BedType(string type)
        {
            switch (type)
            {
                case BedTypeAdultICU:
                    return new FhirTriplet(
                        SystemBedType,
                        "ICU",
                        "Adult ICU bed type.");

                case BedTypePediatricICU:
                    return new FhirTriplet(
                        SystemBedType,
                        "PEDICU",
                        "Pediatric ICU beds.");

                case BedTypeNeonatalICU:
                    return new FhirTriplet(
                        SystemBedType,
                        "PEDNICU",
                        "Neonatal ICU beds.");

                case BedTypeEmergencyRoom:
                    return new FhirTriplet(
                        SystemBedType,
                        "ER",
                        "Emergency Department beds.");

                case BedTypeHospitalUnit:
                    return new FhirTriplet(
                        SystemBedType,
                        "HU",
                        "Hospital unit.");

                case BedTypeRehabLongTermCare:
                    return new FhirTriplet(
                        SystemBedType,
                        "RHU",
                        "Rehabilitation - long term care beds.");

                case BedTypePediatric:
                    return new FhirTriplet(
                        SystemBedType,
                        "PEDU",
                        "Pediatric beds.");

                case BedTypePsychiatric:
                    return new FhirTriplet(
                        SystemBedType,
                        "PHU",
                        "Psyciatric beds.");

                case BedTypeOperatingRoom:
                    return new FhirTriplet(
                        SystemBedType,
                        "OR",
                        "Operating Rooms");
            }

            return new FhirTriplet();
        }

        /// <summary>Bed feature.</summary>
        /// <param name="feature">The feature.</param>
        /// <returns>A FhirTriplet.</returns>
        public static FhirTriplet BedFeature(string feature)
        {
            switch (feature)
            {
                case BedFeatureNegativeFlowIsolation:
                    return new FhirTriplet(
                        SystemSanerBedFeature,
                        "NEGISO",
                        "Negative airflow isolation beds.");

                case BedFeatureOtherIsolation:
                    return new FhirTriplet(
                        SystemSanerBedFeature,
                        "OTHISO",
                        "Isolation beds (airflow is not a concern).");

                case BedFeatureNonIsolating:
                    return new FhirTriplet(
                        SystemSanerBedFeature,
                        "NONISO",
                        "Non-isolating unit.");
            }

            return new FhirTriplet();
        }
    }
}
