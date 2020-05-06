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
using covidReportTransformationLib.Formats;
using generator_cli.Geographic;
using generator_cli.Models;
using Hl7.Fhir.Model;
using static generator_cli.Generators.CommonLiterals;

namespace generator_cli.Generators
{
    /// <summary>A ben generator.</summary>
    public abstract class FhirGenerator
    {
        /// <summary>The random.</summary>
        private static Random _rand = new Random();

        /// <summary>The identifier.</summary>
        private static long _id = 1;

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
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;.</returns>
        public static Hl7.Fhir.Model.Identifier IdentifierForId(string id)
        {
            return new Hl7.Fhir.Model.Identifier(
                    SystemLiterals.Internal,
                    id);
        }

        /// <summary>Identifier list for identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;.</returns>
        public static List<Hl7.Fhir.Model.Identifier> IdentifierListForId(string id)
        {
            return new List<Identifier>()
            {
                new Hl7.Fhir.Model.Identifier(
                        SystemLiterals.Internal,
                        id),
            };
        }

        /// <summary>Concept for organization type.</summary>
        /// <returns>A List&lt;Hl7.Fhir.Model.CodeableConcept&gt;.</returns>
        public static List<Hl7.Fhir.Model.CodeableConcept> ConceptListForOrganizationType() =>
            new List<Hl7.Fhir.Model.CodeableConcept>()
            {
                new Hl7.Fhir.Model.CodeableConcept(
                    SystemLiterals.OrganizationType,
                    OrganizationTypeProvider),
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

            if (orgId.Contains(OrgPrefix, StringComparison.Ordinal))
            {
                return orgId.Replace(
                    OrgPrefix,
                    $"{RootLocationPrefix}{OrgPrefix}",
                    StringComparison.Ordinal);
            }

            return $"{RootLocationPrefix}{orgId}";
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
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                throw new ArgumentException("Organization Address is required!");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
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
                Meta = new Hl7.Fhir.Model.Meta()
                {
                    Security = FhirTriplet.SecurityTest.GetCodingList(),
                },
                Id = IdForOrgRootLocation(org.Id),
                Address = org.Address[0],
                Status = GetLocationStatus(AvailabilityStatusActive),
                Mode = Location.LocationMode.Instance,
                PhysicalType = FhirTriplet.PhysicalTypeSite.GetConcept(),
                Position = position,
                ManagingOrganization = new ResourceReference($"{org.ResourceType}/{org.Id}"),
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
                OperationalStatus = FhirTriplet.OperationalStatus(operationalStatus).GetCoding(),
                Type = (bedTypes == null) ? null : ConceptsForBedTypes(bedTypes),
                Mode = Location.LocationMode.Instance,
                PhysicalType = FhirTriplet.PhysicalTypeBed.GetConcept(),
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
            bed.OperationalStatus = FhirTriplet.OperationalStatus(next.Status).GetCoding();
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
                    Code = FhirTriplet.SanerAvailabilityStatus.GetConcept(),
                    Value = FhirTriplet.AvailabilityStatus(bedConfig.Availability).GetConcept(),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerOperationalStatus.GetConcept(),
                    Value = FhirTriplet.OperationalStatus(bedConfig.Status).GetConcept(),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerType.GetConcept(),
                    Value = FhirTriplet.BedType(bedConfig.Type).GetConcept(),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerFeature.GetConcept(),
                    Value = FhirTriplet.BedFeature(bedConfig.Feature).GetConcept(),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    Code = FhirTriplet.SanerLocation.GetConcept(),
                    Value = new ResourceReference($"{parentLocation.ResourceType}/{parentLocation.Id}"),
                    Exclude = false,
                },
                new Group.CharacteristicComponent()
                {
                    // Code = FhirTriplet.SanerPeriod.GetConcept(),
                    Code = FhirTriplet.MeasureReportPeriod.GetConcept(),
                    Value = FhirTriplet.EmptyRequired.GetConcept(),
                    Period = period,
                    Exclude = false,
                },
            };

            return new Group()
            {
                Id = NextId,
                Type = Group.GroupType.Device,
                Actual = true,
                Code = FhirTriplet.PhysicalTypeBed.GetConcept(),
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
                            Code = FhirTriplet.SanerAvailabilityStatus.GetConcept(),
                            Value = FhirTriplet.AvailabilityStatus(bedConfig.Availability).GetConcept(),
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerOperationalStatus.GetConcept(),
                            Value = FhirTriplet.OperationalStatus(bedConfig.Status).GetConcept(),
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerType.GetConcept(),
                            Value = FhirTriplet.BedType(bedConfig.Type).GetConcept(),
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerFeature.GetConcept(),
                            Value = FhirTriplet.BedFeature(bedConfig.Feature).GetConcept(),
                        },
                        new MeasureReport.ComponentComponent()
                        {
                            Code = FhirTriplet.SanerLocation.GetConcept(),
                            Value = new CodeableConcept(
                                $"{SystemLiterals.SanerCharacteristic}/partOf",
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
                Measure = SystemLiterals.MeasureReportMeasurement,
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
    }
}
