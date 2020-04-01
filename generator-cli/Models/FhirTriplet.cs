// <copyright file="FhirTriplet.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using generator_cli.Generators;
using Hl7.Fhir.Model;
using static generator_cli.Generators.CommonLiterals;
using static generator_cli.Generators.FhirGenerator;

namespace generator_cli.Models
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

        /// <summary>The empty required.</summary>
        public static FhirTriplet EmptyRequired = new FhirTriplet(string.Empty, string.Empty, _fakeCodeText);

        /// <summary>The measure report period.</summary>
        public static FhirTriplet MeasureReportPeriod = new FhirTriplet(SystemLiterals.MeasureReport, "MeasureReport.period");

        /// <summary>Gets the concept.</summary>
        /// <value>The concept.</value>
        public CodeableConcept Concept => new CodeableConcept(System, Code, Display);

        /// <summary>Gets the coding.</summary>
        /// <value>The coding.</value>
        public Coding Coding => new Coding(System, Code, Display);

        /// <summary>Gets the sct covid.</summary>
        /// <value>The sct covid.</value>
        public static FhirTriplet SctCovid => new FhirTriplet(
            SystemLiterals.SnomedSct,
            SctCovidCode,
            SctCovidDisplay);

        /// <summary>Gets the numerator.</summary>
        /// <value>The numerator.</value>
        public static FhirTriplet MeasurePopulation => new FhirTriplet(
            string.Empty,
            "measure-population",
            "Measure Population");

        /// <summary>Gets the numerator.</summary>
        /// <value>The numerator.</value>
        public static FhirTriplet Numerator => new FhirTriplet(
            string.Empty,
            "numerator",
            "Numerator");

        /// <summary>Gets the denominator.</summary>
        /// <value>The denominator.</value>
        public static FhirTriplet Denominator => new FhirTriplet(
            string.Empty,
            "denominator",
            "Denominator");

        /// <summary>Gets the physical type bed.</summary>
        /// <value>The physical type bed.</value>
        public static FhirTriplet PhysicalTypeBed => new FhirTriplet(SystemLiterals.LocationPhysicalType, LocationPhysicalTypeBed);

        /// <summary>Gets the physical type site.</summary>
        /// <value>The physical type site.</value>
        public static FhirTriplet PhysicalTypeSite => new FhirTriplet(SystemLiterals.LocationPhysicalType, LocationPhysicalTypeSite);

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

        /// <summary>Concepts.</summary>
        /// <param name="system"> The system.</param>
        /// <param name="code">   The code.</param>
        /// <param name="display">(Optional) The display.</param>
        /// <returns>A CodeableConcept.</returns>
        public static CodeableConcept GetConcept(
            string system,
            string code,
            string display = "")
        {
            return new FhirTriplet(system, code, display).Concept;
        }

        /// <summary>Gets a coding.</summary>
        /// <param name="system"> The system.</param>
        /// <param name="code">   The code.</param>
        /// <param name="display">(Optional) The display.</param>
        /// <returns>The coding.</returns>
        public static Coding GetCode(
            string system,
            string code,
            string display = "")
        {
            return new FhirTriplet(system, code, display).Coding;
        }

        /// <summary>For saner.</summary>
        /// <param name="characteristic">The characteristic.</param>
        /// <returns>A FhirTriplet.</returns>
        public static FhirTriplet Saner(SanerCharacteristic characteristic)
        {
            switch (characteristic)
            {
                case SanerCharacteristic.Status:
                    return new FhirTriplet(
                        SystemLiterals.SanerCharacteristic,
                        "Location.status");

                case SanerCharacteristic.OperationalStatus:
                    return new FhirTriplet(
                        SystemLiterals.SanerCharacteristic,
                        "Location.operationalStatus");

                case SanerCharacteristic.Type:
                    return new FhirTriplet(
                        SystemLiterals.SanerCharacteristic,
                        "Location.type");

                case SanerCharacteristic.Feature:
                    return new FhirTriplet(
                        SystemLiterals.SanerCharacteristic,
                        "Location.Feature");

                case SanerCharacteristic.Location:
                    return new FhirTriplet(
                        SystemLiterals.SanerCharacteristic,
                        "Location.partOf");

                case SanerCharacteristic.Period:
                    return new FhirTriplet(
                        SystemLiterals.SanerCharacteristic,
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
                        SystemLiterals.AvailabilityStatus,
                        AvailabilityStatusActive,
                        "The location is operational.");

                case AvailabilityStatusSuspended:
                    return new FhirTriplet(
                        SystemLiterals.AvailabilityStatus,
                        AvailabilityStatusSuspended,
                        "The location is temporarily closed.");

                case AvailabilityStatusInactive:
                    return new FhirTriplet(
                        SystemLiterals.AvailabilityStatus,
                        AvailabilityStatusInactive,
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
                        SystemLiterals.OperationalStatus,
                        OperationalStatusContaminated,
                        "Contaminated");

                case OperationalStatusClosed:
                    return new FhirTriplet(
                        SystemLiterals.OperationalStatus,
                        OperationalStatusClosed,
                        "Closed");

                case OperationalStatusHousekeeping:
                    return new FhirTriplet(
                        SystemLiterals.OperationalStatus,
                        OperationalStatusHousekeeping,
                        "Housekeeping");

                case OperationalStatusOccupied:
                    return new FhirTriplet(
                        SystemLiterals.OperationalStatus,
                        OperationalStatusOccupied,
                        "Occupied");

                case OperationalStatusUnoccupied:
                    return new FhirTriplet(
                        SystemLiterals.OperationalStatus,
                        OperationalStatusUnoccupied,
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
                        SystemLiterals.BedType,
                        BedTypeAdultICU,
                        "Adult ICU bed type.");

                case BedTypePediatricICU:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypePediatricICU,
                        "Pediatric ICU beds.");

                case BedTypeNeonatalICU:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypeNeonatalICU,
                        "Neonatal ICU beds.");

                case BedTypeEmergencyRoom:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypeEmergencyRoom,
                        "Emergency Department beds.");

                case BedTypeHospitalUnit:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypeHospitalUnit,
                        "Hospital unit.");

                case BedTypeRehabLongTermCare:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypeRehabLongTermCare,
                        "Rehabilitation - long term care beds.");

                case BedTypePediatric:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypePediatric,
                        "Pediatric beds.");

                case BedTypePsychiatric:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypePsychiatric,
                        "Psyciatric beds.");

                case BedTypeOperatingRoom:
                    return new FhirTriplet(
                        SystemLiterals.BedType,
                        BedTypeOperatingRoom,
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
                        SystemLiterals.SanerBedFeature,
                        BedFeatureNegativeFlowIsolation,
                        "Negative airflow isolation beds.");

                case BedFeatureOtherIsolation:
                    return new FhirTriplet(
                        SystemLiterals.SanerBedFeature,
                        BedFeatureOtherIsolation,
                        "Isolation beds (airflow is not a concern).");

                case BedFeatureNonIsolating:
                    return new FhirTriplet(
                        SystemLiterals.SanerBedFeature,
                        BedFeatureNonIsolating,
                        "Non-isolating unit.");
            }

            return new FhirTriplet();
        }
    }
}
