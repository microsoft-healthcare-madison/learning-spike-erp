// <copyright file="FhirGenerator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// <summary>The fhir identifier prefix.</summary>
        public const string FhirIdPrefix = "FHIR-";

        /// <summary>The hospital prefix.</summary>
        public const string OrgPrefix = "Org-";

        /// <summary>The root location prefix.</summary>
        public const string RootLocationPrefix = "Loc-";

        /// <summary>The measure report measurement.</summary>
        public const string MeasureReportMeasurement = "https://audaciousinquiry.github.io/saner-ig/Measure/bed-availability-measure";

        /// <summary>The internal system.</summary>
        public const string SystemInternal = "https://github.com/microsoft-healthcare-madison/learning-spike-erp/";

        /// <summary>The SANER-IG characteristic system.</summary>
        public const string SystemSanerCharacteristic = "http://hl7.org/fhir/R4/StructureDefinition/Location";

        /// <summary>The system saner bed feature.</summary>
        public const string SystemSanerBedFeature = "https://audaciousinquiry.github.io/saner-ig/CodeSystem-SanerBedType";

        /// <summary>Type of the system location physical.</summary>
        public const string SystemLocationPhysicalType = "http://terminology.hl7.org/CodeSystem/location-physical-type";

        /// <summary>The system measure report.</summary>
        public const string SystemMeasureReport = "http://hl7.org/fhir/R4/StructureDefinition/MeasureReport";

        /// <summary>The system availability status.</summary>
        public const string SystemAvailabilityStatus = "http://hl7.org/fhir/location-status";

        /// <summary>The system operational status.</summary>
        public const string SystemOperationalStatus = "http://terminology.hl7.org/CodeSystem/v2-0116";

        /// <summary>Type of the system bed.</summary>
        public const string SystemBedType = "http://terminology.hl7.org/CodeSystem/v3-RoleCode";

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
                    SystemInternal,
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
                        SystemInternal,
                        id),
            };
        }

        /// <summary>Concept for organization type.</summary>
        /// <returns>A List&lt;Hl7.Fhir.Model.CodeableConcept&gt;</returns>
        public static List<Hl7.Fhir.Model.CodeableConcept> ConceptListForOrganizationType() =>
            new List<Hl7.Fhir.Model.CodeableConcept>()
            {
                new Hl7.Fhir.Model.CodeableConcept(
                    "http://hl7.org/fhir/CodeSystem/organization-type",
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
                PhysicalType = FhirTriplet.PhysicalTypeSite.Concept,
                Position = position,
            };
        }

        /// <summary>Generates a bed with random properties.</summary>
        /// <param name="availabilityStatus">The availability status.</param>
        /// <param name="operationalStatus"> The operational status.</param>
        /// <param name="bedTypes">          List of types of the bed.</param>
        /// <param name="org">          The managing organization.</param>
        /// <param name="parentLocation">    The parent location.</param>
        /// <returns>The bed.</returns>
        public static Location GenerateBed(
            string availabilityStatus,
            string operationalStatus,
            List<string> bedTypes,
            Organization org,
            Location parentLocation)
        {
            Location loc = new Location()
            {
                Id = NextId,
                Status = GetLocationStatus(availabilityStatus),
                OperationalStatus = FhirTriplet.OperationalStatus(operationalStatus).Coding,
                Type = (bedTypes == null) ? null : ConceptsForBedTypes(bedTypes),
                Mode = Location.LocationMode.Instance,
                PhysicalType = FhirTriplet.PhysicalTypeBed.Concept,
            };

            if (org != null)
            {
                loc.ManagingOrganization = new ResourceReference($"{org.ResourceType}/{org.Id}");
            }

            if (parentLocation != null)
            {
                loc.PartOf = new ResourceReference($"{loc.ResourceType}/{parentLocation.Id}");
            }

            return loc;
        }

        /// <summary>Updates the bed configuration.</summary>
        /// <param name="bed"> [in,out] The bed.</param>
        /// <param name="next">The next.</param>
        public static void UpdateBedConfiguration(
            ref Location bed,
            BedConfiguration next)
        {
            if (bed == null)
            {
                throw new ArgumentNullException(nameof(bed));
            }

            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            bed.Status = GetLocationStatus(next.Availability);
            bed.OperationalStatus = FhirTriplet.OperationalStatus(next.Status).Coding;
        }

        /// <summary>Generates a group.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="org">           The managing organization.</param>
        /// <param name="parentLocation">The parent location.</param>
        /// <param name="name">          The name.</param>
        /// <param name="bedConfig">     The bed configuration this group represents.</param>
        /// <param name="bedCount">      Number of beds.</param>
        /// <param name="period">        The period.</param>
        /// <returns>The group.</returns>
        public static Group GenerateGroup(
            Organization org,
            Location parentLocation,
            string name,
            BedConfiguration bedConfig,
            int bedCount,
            Period period)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if (parentLocation == null)
            {
                throw new ArgumentNullException(nameof(parentLocation));
            }

            if (bedConfig == null)
            {
                throw new ArgumentNullException(nameof(bedConfig));
            }

            List<Group.CharacteristicComponent> characteristics = new List<Group.CharacteristicComponent>()
            {
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerAvailabilityStatus.Concept,
                    Value = FhirTriplet.AvailabilityStatus(bedConfig.Availability).Concept,
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerOperationalStatus.Concept,
                    Value = FhirTriplet.OperationalStatus(bedConfig.Status).Concept,
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerType.Concept,
                    Value = FhirTriplet.BedType(bedConfig.Type).Concept,
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerFeature.Concept,
                    Value = FhirTriplet.BedFeature(bedConfig.Feature).Concept,
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerLocation.Concept,
                    Value = new ResourceReference($"{parentLocation.ResourceType}/{parentLocation.Id}"),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    // Code = FhirTriplet.SanerPeriod.Concept,
                    Code = FhirTriplet.MeasureReportPeriod.Concept,
                    Value = FhirTriplet.EmptyRequired.Concept,
                    Period = period,
                    Exclude = false,
                },
            };

            return new Group()
            {
                Id = NextId,
                Type = Group.GroupType.Device,
                Actual = true,
                Code = FhirTriplet.PhysicalTypeBed.Concept,
                Name = name,
                Quantity = bedCount,
                ManagingEntity = new ResourceReference($"{org.ResourceType}/{org.Id}"),
                Characteristic = characteristics,
                Text = new Narrative()
                {
                    Div = $"<div xmlns=\"http://www.w3.org/1999/xhtml\">" +
                        $" {org.Name} Beds of type: {bedConfig.Type} ({bedConfig.Feature})" +
                        $" Flagged {bedConfig.Availability} and {bedConfig.Status}</div>",
                },
            };
        }

        public static MeasureReport GenerateScreeningRateReport()
        {
            return null;
        }

        /// <summary>Generates a measure report.</summary>
        /// <param name="org">           The organization.</param>
        /// <param name="parentLocation">The parent location.</param>
        /// <param name="period">        The period.</param>
        /// <param name="bedsByConfig">  The beds by configuration.</param>
        /// <returns>The measure report.</returns>
        public static MeasureReport GenerateBedMeasureReportV01(
            Organization org,
            Location parentLocation,
            Period period,
            Dictionary<BedConfiguration, List<Location>> bedsByConfig)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if (parentLocation == null)
            {
                throw new ArgumentNullException(nameof(parentLocation));
            }

            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            if (bedsByConfig == null)
            {
                throw new ArgumentNullException(nameof(bedsByConfig));
            }

            int totalBedCount = 0;
            List<MeasureReport.StratifierGroupComponent> stratums = new List<MeasureReport.StratifierGroupComponent>();

            foreach (BedConfiguration bedConfig in bedsByConfig.Keys)
            {
                if (bedsByConfig[bedConfig].Count == 0)
                {
                    continue;
                }

                totalBedCount += bedsByConfig[bedConfig].Count;

                stratums.Add(new MeasureReport.StratifierGroupComponent()
                {
                    MeasureScore = new Quantity(bedsByConfig[bedConfig].Count, "Number"),
                    Component = new List<MeasureReport.ComponentComponent>()
                    {
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerAvailabilityStatus.Concept,
                            Value = FhirTriplet.AvailabilityStatus(bedConfig.Availability).Concept,
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerOperationalStatus.Concept,
                            Value = FhirTriplet.OperationalStatus(bedConfig.Status).Concept,
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerType.Concept,
                            Value = FhirTriplet.BedType(bedConfig.Type).Concept,
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerFeature.Concept,
                            Value = FhirTriplet.BedFeature(bedConfig.Feature).Concept,
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerLocation.Concept,
                            Value = new CodeableConcept(
                                $"{SystemSanerCharacteristic}/partOf",
                                $"{parentLocation.ResourceType}/{parentLocation.Id}"),
                        },
                    },
                });
            }

            MeasureReport.GroupComponent component = new MeasureReport.GroupComponent()
            {
                MeasureScore = new Quantity(totalBedCount, "Number"),
                Stratifier = new List<MeasureReport.StratifierComponent>()
                {
                    new MeasureReport.StratifierComponent()
                    {
                        Stratum = stratums,
                    },
                },
            };

            return new MeasureReport()
            {
                Id = NextId,
                Status = MeasureReport.MeasureReportStatus.Complete,
                Type = MeasureReport.MeasureReportType.Summary,
                Date = new FhirDateTime(new DateTimeOffset(DateTime.Now)).ToString(),
                Period = period,
                Measure = MeasureReportMeasurement,
                Reporter = new ResourceReference($"{org.ResourceType}/{org.Id}"),
                Group = new List<MeasureReport.GroupComponent>() { component },
                Text = new Narrative()
                {
                    Div = $"<div xmlns=\"http://www.w3.org/1999/xhtml\">" + 
                        $" {org.Name} Bed report for" +
                        $" {period.Start.ToString(CultureInfo.InvariantCulture)}" +
                        $" to" +
                        $" {period.End.ToString(CultureInfo.InvariantCulture)}</div>",
                },
            };
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

        /// <summary>Concepts for bed types.</summary>
        /// <param name="bedTypes">List of types of the bed.</param>
        /// <returns>A List&lt;CodeableConcept&gt;</returns>
        private static List<CodeableConcept> ConceptsForBedTypes(List<string> bedTypes)
        {
            List<CodeableConcept> concepts = new List<CodeableConcept>();

            foreach (string bedType in bedTypes)
            {
                concepts.Add(FhirTriplet.BedType(bedType).Concept);
            }

            return concepts;
        }
    }
}
