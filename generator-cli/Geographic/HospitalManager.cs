// <copyright file="HospitalManager.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CsvHelper;
using generator_cli.Generators;

namespace generator_cli.Geographic
{
    /// <summary>Manager for hospitals.</summary>
    public abstract class HospitalManager
    {
        /// <summary>The hospital prefix.</summary>
        public const string HospitalPrefix = "Org-";

        private static List<HospitalRecord> _hospitals = new List<HospitalRecord>();
        private static Dictionary<string, List<HospitalRecord>> _hospitalsByZip = new Dictionary<string, List<HospitalRecord>>();
        private static Dictionary<string, List<HospitalRecord>> _hospitalsByState = new Dictionary<string, List<HospitalRecord>>();
        private static Dictionary<string, HospitalRecord> _hospitalsById = new Dictionary<string, HospitalRecord>();

        private static Random _rand = null;

        /// <summary>Initializes this object.</summary>
        /// <param name="seed">(Optional) The seed.</param>
        public static void Init(int seed = 0)
        {
            if (seed == 0)
            {
                _rand = new Random();
            }
            else
            {
                _rand = new Random(seed);
            }

            string filename = Path.Combine(Directory.GetCurrentDirectory(), "data", "Hospitals.csv");

            IEnumerable<HospitalRecord> raw;

            using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                raw = csv.GetRecords<HospitalRecord>();

                using (IEnumerator<HospitalRecord> rawEnumerator = raw.GetEnumerator())
                {
                    while (rawEnumerator.MoveNext())
                    {
                        HospitalRecord hosp = rawEnumerator.Current;

                        _hospitals.Add(hosp);

                        _hospitalsById.Add(
                            IdForOrg(hosp.OBJECTID),
                            hosp);

                        if (!_hospitalsByZip.ContainsKey(hosp.ZIP))
                        {
                            _hospitalsByZip.Add(hosp.ZIP, new List<HospitalRecord>());
                        }

                        _hospitalsByZip[hosp.ZIP].Add(hosp);

                        if (!_hospitalsByState.ContainsKey(hosp.STATE))
                        {
                            _hospitalsByState.Add(hosp.STATE, new List<HospitalRecord>());
                        }

                        _hospitalsByState[hosp.STATE].Add(hosp);
                    }
                }
            }

            Console.WriteLine(
                $"Loaded {_hospitals.Count} hospital records" +
                $", {_hospitalsByState.Count} states" +
                $", {_hospitalsByZip.Count} zip codes");
        }

        /// <summary>Gets an organization.</summary>
        /// <param name="state">     (Optional) The state.</param>
        /// <param name="postalCode">(Optional) The postal code.</param>
        /// <returns>The organization.</returns>
        public static Hl7.Fhir.Model.Organization GetOrganization(
            string state = null,
            string postalCode = null)
        {
            if (!string.IsNullOrEmpty(state))
            {
                return GetOrganizationForState(state);
            }

            if (!string.IsNullOrEmpty(postalCode))
            {
                return GetOrganizationForPostalCode(postalCode);
            }

            return GetAnyOrganization();
        }

        /// <summary>Gets any organization.</summary>
        /// <returns>any organization.</returns>
        public static Hl7.Fhir.Model.Organization GetAnyOrganization()
        {
            // note: Random.next max is exclusive
            int index = _rand.Next(0, _hospitals.Count);

            return OrgForRec(_hospitals[index]);
        }

        /// <summary>Hospital for postal code.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="postalCode">The postal code.</param>
        /// <returns>A HospitalRecord.</returns>
        public static Hl7.Fhir.Model.Organization GetOrganizationForPostalCode(
            string postalCode)
        {
            if (string.IsNullOrEmpty(postalCode))
            {
                throw new ArgumentNullException(nameof(postalCode));
            }

            if (!_hospitalsByZip.ContainsKey(postalCode))
            {
                throw new ArgumentOutOfRangeException(nameof(postalCode));
            }

            // note: Random.next max is exclusive
            int index = _rand.Next(0, _hospitalsByZip[postalCode].Count);

            return OrgForRec(_hospitalsByZip[postalCode][index]);
        }

        /// <summary>Organisation for record.</summary>
        /// <param name="hosp">The hosp.</param>
        /// <returns>A Hl7.Fhir.Model.Organization.</returns>
        private static Hl7.Fhir.Model.Organization OrgForRec(HospitalRecord hosp)
        {
            return new Hl7.Fhir.Model.Organization()
            {
                Id = IdForOrg(hosp.OBJECTID),
                Identifier = IdentifierForOrg(hosp.OBJECTID),
                Active = true,
                Type = ConceptForOrganizationType(),
                Name = hosp.NAME,
                Address = new List<Hl7.Fhir.Model.Address>()
                {
                    new Hl7.Fhir.Model.Address()
                    {
                        Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                        Type = Hl7.Fhir.Model.Address.AddressType.Both,
                        Line = new List<string>()
                        {
                            hosp.ADDRESS,
                        },
                        City = hosp.CITY,
                        State = hosp.STATE,
                        PostalCode = hosp.ZIP,
                        Country = "USA",
                    },
                },
            };
        }

        /// <summary>Hospital for state.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="state">The state.</param>
        /// <returns>A HospitalRecord.</returns>
        public static Hl7.Fhir.Model.Organization GetOrganizationForState(
            string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (!_hospitalsByState.ContainsKey(state))
            {
                throw new ArgumentOutOfRangeException(nameof(state));
            }

            // note: Random.next max is exclusive
            int index = _rand.Next(0, _hospitalsByState[state].Count);

            return OrgForRec(_hospitalsByState[state][index]);
        }

        /// <summary>Identifier for identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;</returns>
        private static List<Hl7.Fhir.Model.Identifier> IdentifierForOrg(long id) =>
            new List<Hl7.Fhir.Model.Identifier>()
            {
                new Hl7.Fhir.Model.Identifier(
                    "https://hifld-geoplatform.opendata.arcgis.com/datasets/hospitals",
                    $"{id}"),
            };

        /// <summary>Identifier for organization.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A string.</returns>
        private static string IdForOrg(long id) => $"{HospitalPrefix}{id}";

        /// <summary>Concept for organization type.</summary>
        /// <returns>A List&lt;Hl7.Fhir.Model.CodeableConcept&gt;</returns>
        private static List<Hl7.Fhir.Model.CodeableConcept> ConceptForOrganizationType() =>
            new List<Hl7.Fhir.Model.CodeableConcept>()
            {
                new Hl7.Fhir.Model.CodeableConcept(
                    "http://hl7.org/fhir/ValueSet/organization-type",
                    "prov"),
            };

        /// <summary>Query if 'id' is hospital known.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if hospital known, false if not.</returns>
        public static bool IsHospitalKnown(string id)
        {
            return _hospitalsById.ContainsKey(id);
        }

        /// <summary>Position for hospital.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="id">The identifier.</param>
        /// <returns>A Hl7.Fhir.Model.Location.PositionComponent.</returns>
        public static Hl7.Fhir.Model.Location.PositionComponent PositionForHospital(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!_hospitalsById.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return new Hl7.Fhir.Model.Location.PositionComponent()
            {
                Longitude = (decimal)_hospitalsById[id].LONGITUDE,
                Latitude = (decimal)_hospitalsById[id].LATITUDE,
            };
        }
    }
}
