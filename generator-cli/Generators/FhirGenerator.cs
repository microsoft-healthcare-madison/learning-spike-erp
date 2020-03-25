// <copyright file="BenGenerator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using generator_cli.Geographic;
using Hl7.Fhir.Model;

namespace generator_cli.Generators
{
    /// <summary>A ben generator.</summary>
    public abstract class FhirGenerator
    {
        /// <summary>The internal system.</summary>
        public const string InternalSystem = "https://github.com/microsoft-healthcare-madison/learning-spike-erp/";

        /// <summary>The fhir identifier prefix.</summary>
        public const string FhirIdPrefix = "FHIR-";

        /// <summary>The hospital prefix.</summary>
        public const string OrgPrefix = "Org-";

        /// <summary>The root location prefix.</summary>
        public const string RootLocationPrefix = "Loc-";

        /// <summary>The SANER-IG characteristic system.</summary>
        private const string _sanerCharacteristicSystem = "http://hl7.org/fhir/location-definitions";

        /// <summary>The random.</summary>
        private static Random _rand = new Random();

        /// <summary>The identifier.</summary>
        private static long _id = 1;

        /// <summary>This location is operational (but may be in use).</summary>
        public const string AvailabilityStatusActive = "active";
        /// <summary>This location is temporarily out of service.</summary>
        public const string AvailabilityStatusSuspended = "suspended";
        /// <summary>This location is no longer operational.</summary>
        public const string AvailabilityStatusInactive = "inactive";

        /// <summary>The availability statuses.</summary>
        public static readonly string[] AvailabilityStatuses = {
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
        public static readonly string[] OperationalStatuses = {
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
        public static readonly string[] BedTypes = {
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
        public static readonly string[] BedFeatures = {
            BedFeatureNegativeFlowIsolation,
            BedFeatureOtherIsolation,
            BedFeatureNonIsolating,
        };

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
        }

        /// <summary>Gets the identifier of the next.</summary>
        /// <value>The identifier of the next.</value>
        public static string NextId
        {
            get
            {
                return $"{FhirIdPrefix}{Interlocked.Increment(ref _id)}";
            }
        }

        /// <summary>Gets the identifier of the next organisation.</summary>
        /// <value>The identifier of the next organisation.</value>
        public static string NextOrgId
        {
            get
            {
                return $"{OrgPrefix}{Interlocked.Increment(ref _id)}";
            }
        }

        /// <summary>Identifier for identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;</returns>
        public static Hl7.Fhir.Model.Identifier IdentifierForId(string id)
        {
            return new Hl7.Fhir.Model.Identifier(
                    InternalSystem,
                    id);
        }

        /// <summary>Identifier list for identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;</returns>
        public static List<Hl7.Fhir.Model.Identifier> IdentifierListForId(string id)
        {
            return new List<Identifier>()
            {
                new Hl7.Fhir.Model.Identifier(
                        InternalSystem,
                        id),
            };
        }

        /// <summary>Concept for organization type.</summary>
        /// <returns>A List&lt;Hl7.Fhir.Model.CodeableConcept&gt;</returns>
        public static List<Hl7.Fhir.Model.CodeableConcept> ConceptListForOrganizationType() =>
            new List<Hl7.Fhir.Model.CodeableConcept>()
            {
                new Hl7.Fhir.Model.CodeableConcept(
                    "http://hl7.org/fhir/ValueSet/organization-type",
                    "prov"),
            };

        /// <summary>Identifier for organization root location.</summary>
        /// <param name="orgId">Identifier for the organization.</param>
        /// <returns>A string.</returns>
        private static string IdForOrgRootLocation(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
            {
                return NextId;
            }

            return orgId.Replace(
                OrgPrefix,
                $"{RootLocationPrefix}{OrgPrefix}",
                StringComparison.Ordinal);
        }

        /// <summary>Generates a bed with random properties.</summary>
        /// <param name="managing">      The managing organization.</param>
        /// <param name="parentLocation">The parent location.</param>
        /// <returns>The bed.</returns>
        public static Location GenerateBed(
            Organization managing,
            Location parentLocation)
        {
            // note: Random.Next max is exclusive
            string availability = AvailabilityStatuses[_rand.Next(0, AvailabilityStatuses.Length)];

            string operational = OperationalStatuses[_rand.Next(0, OperationalStatuses.Length)];

            List<string> bedTypes = new List<string>()
            {
                BedTypes[_rand.Next(0, BedTypes.Length)],
            };

            return GenerateBed(
                availability,
                operational,
                bedTypes,
                managing,
                parentLocation);
        }

        /// <summary>Root location for organization.</summary>
        /// <param name="org">The organization.</param>
        /// <returns>A Location.</returns>
        public static Location RootLocationForOrg(
            Organization org)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if ((org.Address == null) || (org.Address.Count == 0))
            {
                throw new ArgumentException($"Organization Address is required!");
            }

            Location.PositionComponent position = null;

            if (HospitalManager.IsHospitalKnown(org.Id))
            {
                position = HospitalManager.PositionForHospital(org.Id);
            }
            else if (GeoManager.IsPostalCodeKnown(org.Address[0].PostalCode))
            {
                position = GeoManager.PositionForPostalCode(org.Address[0].PostalCode);
            }

            return new Location()
            {
                Id = IdForOrgRootLocation(org.Id),
                Address = org.Address[0],
                Status = GetLocationStatus(AvailabilityStatusActive),
                Mode = Location.LocationMode.Instance,
                PhysicalType = ConceptForPhysicalTypeSite(),
                Position = position,
            };
        }

        /// <summary>Generates a bed with random properties.</summary>
        /// <param name="availabilityStatus">The availability status.</param>
        /// <param name="operationalStatus"> The operational status.</param>
        /// <param name="bedTypes">          List of types of the bed.</param>
        /// <param name="managing">          The managing organization.</param>
        /// <param name="parentLocation">    The parent location.</param>
        /// <returns>The bed.</returns>
        public static Location GenerateBed(
            string availabilityStatus,
            string operationalStatus,
            List<string> bedTypes,
            Organization managing,
            Location parentLocation)
        {
            Location loc = new Location()
            {
                Id = NextId,
                Status = GetLocationStatus(availabilityStatus),
                OperationalStatus = CodingForOperationalStatus(operationalStatus),
                Type = (bedTypes == null) ? null : ConceptsForBedTypes(bedTypes),
                Mode = Location.LocationMode.Instance,
                PhysicalType = ConceptForPhysicalTypeBed(),
            };

            if (managing != null)
            {
                loc.ManagingOrganization = new ResourceReference(managing.Id);
            }

            if (parentLocation != null)
            {
                loc.PartOf = new ResourceReference(parentLocation.Id);
            }

            return loc;
        }

        /// <summary>Generates a group.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="name">                The name.</param>
        /// <param name="quantity">            The quantity.</param>
        /// <param name="managingOrganization">The managing organization.</param>
        /// <param name="parentLocation">      The parent location.</param>
        /// <param name="availabilityStatus">  The availability status.</param>
        /// <param name="operationalStatus">   The operational status.</param>
        /// <param name="bedType">             Type of the bed.</param>
        /// <param name="bedFeature">          The bed feature.</param>
        /// <returns>The group.</returns>
        public static Group GenerateGroup(
            string name,
            int quantity,
            Organization managingOrganization,
            Location parentLocation,
            string availabilityStatus,
            string operationalStatus,
            string bedType,
            string bedFeature)
        {
            if (managingOrganization == null)
            {
                throw new ArgumentNullException(nameof(managingOrganization));
            }

            if (parentLocation == null)
            {
                throw new ArgumentNullException(nameof(parentLocation));
            }

            List<Group.CharacteristicComponent> characteristics = new List<Group.CharacteristicComponent>()
            {
                new Group.CharacteristicComponent()
                {
                    Code = ConceptForSaner(SanerCharacteristic.Status),
                    Value = CodingForAvailabilityStatus(availabilityStatus),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = ConceptForSaner(SanerCharacteristic.OperationalStatus),
                    Value = CodingForOperationalStatus(operationalStatus),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = ConceptForSaner(SanerCharacteristic.Type),
                    Value = ConceptForBedType(bedType),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = ConceptForSaner(SanerCharacteristic.Feature),
                    Value = ConceptForBedFeature(bedFeature),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = ConceptForSaner(SanerCharacteristic.Location),
                    Value = new ResourceReference(parentLocation.Id),
                    Exclude = false,
                },
            };

            return new Group()
            {
                Type = Group.GroupType.Device,
                Actual = true,
                Code = ConceptForPhysicalTypeBed(),
                Name = name,
                Quantity = quantity,
                ManagingEntity = new ResourceReference(managingOrganization.Id),
                Characteristic = characteristics,
            };
        }

        /// <summary>Concept for saner.</summary>
        /// <param name="saner">The saner.</param>
        /// <returns>A CodeableConcept.</returns>
        private static CodeableConcept ConceptForSaner(SanerCharacteristic saner)
        {
            switch (saner)
            {
                case SanerCharacteristic.Status:
                    return new CodeableConcept(
                        _sanerCharacteristicSystem,
                        "Location.status");

                case SanerCharacteristic.OperationalStatus:
                    return new CodeableConcept(
                        _sanerCharacteristicSystem,
                        "Location.operationalStatus");

                case SanerCharacteristic.Type:
                    return new CodeableConcept(
                        _sanerCharacteristicSystem,
                        "Location.type");

                case SanerCharacteristic.Feature:
                    return new CodeableConcept(
                        _sanerCharacteristicSystem,
                        "Location.Feature");

                case SanerCharacteristic.Location:
                    return new CodeableConcept(
                        _sanerCharacteristicSystem,
                        "Location.partOf");
            }

            return null;
        }

        /// <summary>Concept for physical type bed.</summary>
        /// <returns>A CodeableConcept.</returns>
        private static CodeableConcept ConceptForPhysicalTypeBed()
        {
            return new CodeableConcept(
                "http://terminology.hl7.org/ValueSet/location-physical-type",
                "bd");
        }

        /// <summary>Concept for physical type site.</summary>
        /// <returns>A CodeableConcept.</returns>
        private static CodeableConcept ConceptForPhysicalTypeSite()
        {
            return new CodeableConcept(
                "http://terminology.hl7.org/ValueSet/location-physical-type",
                "si");
        }

        /// <summary>Gets location status.</summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="status">The status.</param>
        /// <returns>The location status.</returns>
        private static Location.LocationStatus GetLocationStatus(string status)
        {
            switch (status)
            {
                case AvailabilityStatusActive:
                    return Location.LocationStatus.Active;

                case AvailabilityStatusSuspended:
                    return Location.LocationStatus.Suspended;

                case AvailabilityStatusInactive:
                    return Location.LocationStatus.Inactive;
            }

            throw new ArgumentOutOfRangeException(nameof(status));
        }

        /// <summary>Coding for availability status.</summary>
        /// <param name="status">The status.</param>
        /// <returns>A Coding.</returns>
        private static Coding CodingForAvailabilityStatus(string status)
        {
            switch (status)
            {
                case AvailabilityStatusActive:
                    return new Coding(
                        "http://hl7.org/fhir/location-status",
                        "active",
                        "The location is operational.");

                case AvailabilityStatusSuspended:
                    return new Coding(
                        "http://hl7.org/fhir/location-status",
                        "suspended",
                        "The location is temporarily closed.");

                case AvailabilityStatusInactive:
                    return new Coding(
                        "http://hl7.org/fhir/location-status",
                        "inactive",
                        "The location is no longer used.");
            }

            return null;
        }

        /// <summary>Coding for status.</summary>
        /// <param name="status">The status.</param>
        /// <returns>A Coding.</returns>
        private static Coding CodingForOperationalStatus(string status)
        {
            switch (status)
            {
                case OperationalStatusContaminated:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "K",
                        "Contaminated");

                case OperationalStatusClosed:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "C",
                        "Closed");

                case OperationalStatusHousekeeping:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "H",
                        "Housekeeping");

                case OperationalStatusOccupied:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "O",
                        "Occupied");

                case OperationalStatusUnoccupied:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "U",
                        "Unoccupied");
            }

            return null;
        }

        /// <summary>Codings for bed types.</summary>
        /// <param name="bedTypes">List of types of the bed.</param>
        /// <returns>A List&lt;Coding&gt;</returns>
        private static List<Coding> CodingsForBedTypes(List<string> bedTypes)
        {
            List<Coding> codings = new List<Coding>();

            foreach (string bedType in bedTypes)
            {
                codings.Add(CodingForBedType(bedType));
            }

            return codings;
        }

        /// <summary>Coding for bed type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>A Coding.</returns>
        private static Coding CodingForBedType(string type)
        {
            switch (type)
            {
                case BedTypeAdultICU:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ICU",
                        "Adult ICU bed type.");

                case BedTypePediatricICU:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDICU",
                        "Pediatric ICU beds.");

                case BedTypeNeonatalICU:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDNICU",
                        "Neonatal ICU beds.");

                case BedTypeEmergencyRoom:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ER",
                        "Emergency Department beds.");

                case BedTypeHospitalUnit:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "HU",
                        "Hospital unit.");

                case BedTypeRehabLongTermCare:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "RHU",
                        "Rehabilitation - long term care beds.");

                case BedTypePediatric:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDU",
                        "Pediatric beds.");

                case BedTypePsychiatric:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PHU",
                        "Psyciatric beds.");

                case BedTypeOperatingRoom:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "OR",
                        "Operating Rooms");
            }

            return null;
        }

        /// <summary>Concepts for bed types.</summary>
        /// <param name="bedTypes">List of types of the bed.</param>
        /// <returns>A List&lt;CodeableConcept&gt;</returns>
        private static List<CodeableConcept> ConceptsForBedTypes(List<string> bedTypes)
        {
            List<CodeableConcept> concepts = new List<CodeableConcept>();

            foreach (string bedType in bedTypes)
            {
                concepts.Add(ConceptForBedType(bedType));
            }

            return concepts;
        }

        /// <summary>Concept for bed type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>A CodeableConcept.</returns>
        private static CodeableConcept ConceptForBedType(string type)
        {
            switch (type)
            {
                case BedTypeAdultICU:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ICU",
                        "Adult ICU bed type.");

                case BedTypePediatricICU:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDICU",
                        "Pediatric ICU beds.");

                case BedTypeNeonatalICU:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDNICU",
                        "Neonatal ICU beds.");

                case BedTypeEmergencyRoom:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ER",
                        "Emergency Department beds.");

                case BedTypeHospitalUnit:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "HU",
                        "Hospital beds.");

                case BedTypeRehabLongTermCare:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "RHU",
                        "Rehabilitation - long term care beds.");

                case BedTypePediatric:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDU",
                        "Pediatric beds.");

                case BedTypePsychiatric:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PHU",
                        "Psyciatric beds.");

                case BedTypeOperatingRoom:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "OR",
                        "Operating Rooms");
            }

            return null;
        }

        /// <summary>Concept for bed feature.</summary>
        /// <param name="feature">The feature.</param>
        /// <returns>A CodeableConcept.</returns>
        private static CodeableConcept ConceptForBedFeature(string feature)
        {
            switch (feature)
            {
                case BedFeatureNegativeFlowIsolation:
                    return new CodeableConcept(
                        "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType",
                        "NEGISO",
                        "Negative airflow isolation beds.");

                case BedFeatureOtherIsolation:
                    return new CodeableConcept(
                        "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType",
                        "OTHISO",
                        "Isolation beds (airflow is not a concern).");

                case BedFeatureNonIsolating:
                    return new CodeableConcept(
                        "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType",
                        "NONISO",
                        "Non-isolating unit.");
            }

            return null;
        }
    }
}
