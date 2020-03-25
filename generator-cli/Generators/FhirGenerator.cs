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

        /// <summary>The availability statuses.</summary>
        private static readonly Array _availabilityStatuses = Enum.GetValues(typeof(AvailabilityStatus));

        /// <summary>The operational statuses.</summary>
        private static readonly Array _operationalStatuses = Enum.GetValues(typeof(OperationalStatus));

        /// <summary>List of types of the bed.</summary>
        private static readonly Array _bedTypes = Enum.GetValues(typeof(BedType));

        /// <summary>The bed features.</summary>
        private static readonly Array _bedFeatures = Enum.GetValues(typeof(BedFeature));

        /// <summary>The random.</summary>
        private static Random _rand = new Random();

        /// <summary>The identifier.</summary>
        private static long _id = 1;

        /// <summary>Values that represent availability status.</summary>
        public enum AvailabilityStatus
        {
            /// <summary>Beds described by this characteristic are operational (but may be in use).</summary>
            Active,

            /// <summary>Beds described by this characteristic are temporarily out of service.</summary>
            Suspended,

            /// <summary>Beds described by this characteristic are no longer operational (e.g., Closed).</summary>
            Inactive,
        }

        /// <summary>Values that represent bed status.</summary>
        public enum OperationalStatus
        {
            /// <summary>This bed needs decontamination before it can be readied for use.</summary>
            Contaminated,

            /// <summary>This bed is no longer in service.</summary>
            Closed,

            /// <summary>This bed is not in use, but is presently ready for use.</summary>
            Housekeeping,

            /// <summary>This bed is presently in use.</summary>
            Occupied,

            /// <summary>This bed is presently ready for use.</summary>
            Unoccupied,
        }

        /// <summary>Values that represent bed types.</summary>
        public enum BedType
        {
            /// <summary>Adult ICU beds.</summary>
            AdultICU,

            /// <summary>Pediatric ICU beds.</summary>
            PediactricICU,

            /// <summary>Neonatal ICU beds.</summary>
            NeonatalICU,

            /// <summary>Emergency department beds for acute care.</summary>
            EmergencyRoom,

            /// <summary>Medical-surgical beds.</summary>
            MedicalSurgical,

            /// <summary>Rehabilitation / long term care beds.</summary>
            RehabLongTermCare,

            /// <summary>Pediatric beds.</summary>
            Pediatrics,

            /// <summary>Ward beds on a close/locked psychiatric unit or ward.</summary>
            Psychiatric,

            /// <summary>Operating rooms which are equipped, staffed, and could be made available in a short period of time.</summary>
            OperatingRoom,
        }

        /// <summary>Values that represent bed features.</summary>
        public enum BedFeature
        {
            /// <summary>Negative airflow isolation beds.</summary>
            NegativeFlowIsolation,

            /// <summary>Isolation beds (airflow is not a concern).</summary>
            OtherIsolation,

            /// <summary>An enum constant representing the non isolating option.</summary>
            NonIsolating,
        }

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
        /// <param name="address">       The address.</param>
        /// <param name="managing">      The managing organization.</param>
        /// <param name="parentLocation">The parent location.</param>
        /// <returns>The bed.</returns>
        public static Location GenerateBed(
            Address address,
            Organization managing,
            Location parentLocation)
        {
            // note: Random.Next max is exclusive
            AvailabilityStatus availability = (AvailabilityStatus)
                _availabilityStatuses.GetValue(_rand.Next(0, _availabilityStatuses.Length));

            OperationalStatus operational = (OperationalStatus)
                _operationalStatuses.GetValue(_rand.Next(0, _operationalStatuses.Length));

            List<BedType> bedTypes = new List<BedType>()
            {
                (BedType)_bedTypes.GetValue(_rand.Next(0, _bedTypes.Length)),
            };

            return GenerateBed(
                address,
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
                Status = GetLocationStatus(AvailabilityStatus.Active),
                Mode = Location.LocationMode.Instance,
                PhysicalType = ConceptForPhysicalTypeSite(),
                Position = position,
            };
        }

        /// <summary>Generates a bed with random properties.</summary>
        /// <param name="address">           The address.</param>
        /// <param name="availabilityStatus">The availability status.</param>
        /// <param name="operationalStatus"> The operational status.</param>
        /// <param name="bedTypes">          List of types of the bed.</param>
        /// <param name="managing">          The managing organization.</param>
        /// <param name="parentLocation">    The parent location.</param>
        /// <returns>The bed.</returns>
        public static Location GenerateBed(
            Address address,
            AvailabilityStatus availabilityStatus,
            OperationalStatus operationalStatus,
            List<BedType> bedTypes,
            Organization managing,
            Location parentLocation)
        {
            Location loc = new Location()
            {
                Id = NextId,
                Address = address,
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
            AvailabilityStatus availabilityStatus,
            OperationalStatus operationalStatus,
            BedType bedType,
            BedFeature bedFeature)
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
        private static Location.LocationStatus GetLocationStatus(AvailabilityStatus status)
        {
            switch (status)
            {
                case AvailabilityStatus.Active:
                    return Location.LocationStatus.Active;

                case AvailabilityStatus.Suspended:
                    return Location.LocationStatus.Suspended;

                case AvailabilityStatus.Inactive:
                    return Location.LocationStatus.Inactive;
            }

            throw new ArgumentOutOfRangeException(nameof(status));
        }

        /// <summary>Coding for availability status.</summary>
        /// <param name="status">The status.</param>
        /// <returns>A Coding.</returns>
        private static Coding CodingForAvailabilityStatus(AvailabilityStatus status)
        {
            switch (status)
            {
                case AvailabilityStatus.Active:
                    return new Coding(
                        "http://hl7.org/fhir/location-status",
                        "active",
                        "The location is operational.");

                case AvailabilityStatus.Suspended:
                    return new Coding(
                        "http://hl7.org/fhir/location-status",
                        "suspended",
                        "The location is temporarily closed.");

                case AvailabilityStatus.Inactive:
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
        private static Coding CodingForOperationalStatus(OperationalStatus status)
        {
            switch (status)
            {
                case OperationalStatus.Contaminated:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "K",
                        "Contaminated");

                case OperationalStatus.Closed:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "C",
                        "Closed");

                case OperationalStatus.Housekeeping:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "H",
                        "Housekeeping");

                case OperationalStatus.Occupied:
                    return new Coding(
                        "http://terminology.hl7.org/CodeSystem/v2-0116",
                        "O",
                        "Occupied");

                case OperationalStatus.Unoccupied:
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
        private static List<Coding> CodingsForBedTypes(List<BedType> bedTypes)
        {
            List<Coding> codings = new List<Coding>();

            foreach (BedType bedType in bedTypes)
            {
                codings.Add(CodingForBedType(bedType));
            }

            return codings;
        }

        /// <summary>Coding for bed type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>A Coding.</returns>
        private static Coding CodingForBedType(BedType type)
        {
            switch (type)
            {
                case BedType.AdultICU:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ICU",
                        "Adult ICU bed type.");

                case BedType.PediactricICU:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDICU",
                        "Pediatric ICU beds.");

                case BedType.NeonatalICU:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDNICU",
                        "Neonatal ICU beds.");

                case BedType.EmergencyRoom:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ER",
                        "Emergency Department beds.");

                case BedType.MedicalSurgical:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "HU",
                        "Medical-surgical beds.");

                case BedType.RehabLongTermCare:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "RHU",
                        "Rehabilitation - long term care beds.");

                case BedType.Pediatrics:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDU",
                        "Pediatric beds.");

                case BedType.Psychiatric:
                    return new Coding(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PHU",
                        "Psyciatric beds.");

                case BedType.OperatingRoom:
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
        private static List<CodeableConcept> ConceptsForBedTypes(List<BedType> bedTypes)
        {
            List<CodeableConcept> concepts = new List<CodeableConcept>();

            foreach (BedType bedType in bedTypes)
            {
                concepts.Add(ConceptForBedType(bedType));
            }

            return concepts;
        }

        /// <summary>Concept for bed type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>A CodeableConcept.</returns>
        private static CodeableConcept ConceptForBedType(BedType type)
        {
            switch (type)
            {
                case BedType.AdultICU:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ICU",
                        "Adult ICU bed type.");

                case BedType.PediactricICU:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDICU",
                        "Pediatric ICU beds.");

                case BedType.NeonatalICU:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDNICU",
                        "Neonatal ICU beds.");

                case BedType.EmergencyRoom:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "ER",
                        "Emergency Department beds.");

                case BedType.MedicalSurgical:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "HU",
                        "Medical-surgical beds.");

                case BedType.RehabLongTermCare:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "RHU",
                        "Rehabilitation - long term care beds.");

                case BedType.Pediatrics:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PEDU",
                        "Pediatric beds.");

                case BedType.Psychiatric:
                    return new CodeableConcept(
                        "http://terminology.hl7.org/ValueSet/v3-ServiceDeliveryLocationRoleType",
                        "PHU",
                        "Psyciatric beds.");

                case BedType.OperatingRoom:
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
        private static CodeableConcept ConceptForBedFeature(BedFeature feature)
        {
            switch (feature)
            {
                case BedFeature.NegativeFlowIsolation:
                    return new CodeableConcept(
                        "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType",
                        "NEGISO",
                        "Negative airflow isolation beds.");

                case BedFeature.OtherIsolation:
                    return new CodeableConcept(
                        "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType",
                        "OTHISO",
                        "Isolation beds (airflow is not a concern).");

                case BedFeature.NonIsolating:
                    return new CodeableConcept(
                        "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType",
                        "OTHISO",
                        "Non-isolating unit.");
            }

            return null;
        }
    }
}
