// <copyright file="HospitalManager.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using covidReportTransformationLib.Formats;
using CsvHelper;
using CsvHelper.Configuration;
using generator_cli.Generators;

namespace generator_cli.Geographic
{
    /// <summary>Manager for hospitals.</summary>
    public abstract class HospitalManager
    {
        private static List<HospitalRecord> _hospitals = new List<HospitalRecord>();
        private static Dictionary<string, List<HospitalRecord>> _hospitalsByZip = new Dictionary<string, List<HospitalRecord>>();
        private static Dictionary<string, List<HospitalRecord>> _hospitalsByState = new Dictionary<string, List<HospitalRecord>>();
        private static Dictionary<string, HospitalRecord> _hospitalsById = new Dictionary<string, HospitalRecord>();

        private static Random _rand = null;

        private static bool _connectathonFlag = false;

        private static int _minBeds = 0;
        private static int _maxBeds = 0;

        /// <summary>Initializes this object.</summary>
        /// <param name="seed">               (Optional) The seed.</param>
        /// <param name="minBeds">            (Optional) The minimum beds.</param>
        /// <param name="maxBeds">            (Optional) The maximum beds.</param>
        /// <param name="dataDirectory">      (Optional) Pathname of the data directory.</param>
        /// <param name="useConnectathonData">(Optional) True to use connectathon data.</param>
        public static void Init(
            int seed = 0,
            int minBeds = 10,
            int maxBeds = 5000,
            string dataDirectory = null,
            bool useConnectathonData = false)
        {
            _connectathonFlag = useConnectathonData;

            if (seed == 0)
            {
                _rand = new Random();
            }
            else
            {
                _rand = new Random(seed);
            }

            _minBeds = minBeds;
            _maxBeds = maxBeds;

            if (!useConnectathonData)
            {
                LoadFromHosptialCsv(dataDirectory, "Hospitals.csv");
            }
            else
            {
                LoadFromConnectathonCsv(dataDirectory, "Hospitals_Chef_County.csv");
                LoadFromConnectathonCsv(dataDirectory, "Hospitals_State_of_New_Cyprus.csv");
            }

            Console.WriteLine(
                $"Loaded {_hospitals.Count} hospital records" +
                $", {_hospitalsByState.Count} states" +
                $", {_hospitalsByZip.Count} zip codes");
        }

        /// <summary>Loads from CSV.</summary>
        /// <param name="dataDirectory">Pathname of the data directory.</param>
        /// <param name="file">         Filename of the file.</param>
        private static void LoadFromConnectathonCsv(string dataDirectory, string file)
        {
            string filename = string.IsNullOrEmpty(dataDirectory)
                ? Path.Combine(Directory.GetCurrentDirectory(), "data", file)
                : Path.Combine(dataDirectory, file);

            if (!File.Exists(filename))
            {
                return;
            }

            long id = _hospitals.Count;

            IEnumerable<ConnectathonRecord> raw;

            using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                raw = csv.GetRecords<ConnectathonRecord>();

                using (IEnumerator<ConnectathonRecord> rawEnumerator = raw.GetEnumerator())
                {
                    while (rawEnumerator.MoveNext())
                    {
                        ConnectathonRecord rec = rawEnumerator.Current;

                        HospitalRecord hosp = new HospitalRecord()
                        {
                            OBJECTID = ++id,
                            ID = rec.CCN,
                            NAME = rec.FacilityName,
                            ADDRESS = rec.Address,
                            CITY = rec.City,
                            COUNTY = rec.County,
                            STATE = rec.State,
                            ZIP = rec.Zip,
                            ZIP4 = rec.ZipPlus4,
                            TYPE = rec.HospitalType,
                            LONGITUDE = rec.Longitude,
                            LATITUDE = rec.Latitude,
                        };

                        _hospitals.Add(hosp);

                        _hospitalsById.Add(
                            rec.CCN,
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
        }

        /// <summary>Loads from CSV.</summary>
        /// <param name="dataDirectory">Pathname of the data directory.</param>
        /// <param name="file">         Filename of the file.</param>
        private static void LoadFromHosptialCsv(string dataDirectory, string file)
        {
            string filename = string.IsNullOrEmpty(dataDirectory)
                ? Path.Combine(Directory.GetCurrentDirectory(), "data", file)
                : Path.Combine(dataDirectory, file);

            if (!File.Exists(filename))
            {
                return;
            }

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
        }

        /// <summary>Beds for hospital.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>An int.</returns>
        public static int BedsForHospital(string id)
        {
            if ((!string.IsNullOrEmpty(id)) && _hospitalsById.ContainsKey(id))
            {
                if (int.TryParse(_hospitalsById[id].BEDS, out int recordBeds))
                {
                    recordBeds = Math.Max(recordBeds, _minBeds);
                    recordBeds = Math.Min(recordBeds, _maxBeds);
                    return recordBeds;
                }
            }

            return _rand.Next(_minBeds, _maxBeds);
        }

        /// <summary>Gets the organizations.</summary>
        /// <param name="requestedCount">Number of.</param>
        /// <param name="state">         (Optional) The state.</param>
        /// <param name="postalCode">    (Optional) The postal code.</param>
        /// <param name="recordsToSkip"> (Optional) The records to skip.</param>
        /// <returns>The organizations.</returns>
        public static List<Hl7.Fhir.Model.Organization> GetOrganizations(
            int requestedCount,
            string state = null,
            string postalCode = null,
            int recordsToSkip = 0)
        {
            if ((!string.IsNullOrEmpty(state)) && _hospitalsByState.ContainsKey(state))
            {
                return GetOrgsFromList(
                    requestedCount,
                    _hospitalsByState[state],
                    recordsToSkip);
            }

            if ((!string.IsNullOrEmpty(postalCode)) && _hospitalsByZip.ContainsKey(postalCode))
            {
                return GetOrgsFromList(
                    requestedCount,
                    _hospitalsByZip[postalCode],
                    recordsToSkip);
            }

            return GetOrgsFromList(requestedCount, _hospitals, recordsToSkip);
        }

        /// <summary>Gets orgs from list.</summary>
        /// <param name="requestedCount">Number of.</param>
        /// <param name="records">       The records.</param>
        /// <param name="recordsToSkip"> (Optional) The records to skip.</param>
        /// <returns>The orgs from list.</returns>
        private static List<Hl7.Fhir.Model.Organization> GetOrgsFromList(
            int requestedCount,
            List<HospitalRecord> records,
            int recordsToSkip = 0)
        {
            List<Hl7.Fhir.Model.Organization> orgs = new List<Hl7.Fhir.Model.Organization>();

            List<int> indexes = Enumerable.Range(0, records.Count - 1).ToList();

            for (int i = Math.Min(requestedCount + recordsToSkip, records.Count - 1); i > 0; i--)
            {
                int index = _rand.Next(0, indexes.Count);
                int listIndex = indexes[index];
                indexes.RemoveAt(index);

                if (recordsToSkip > 0)
                {
                    recordsToSkip--;
                    continue;
                }

                orgs.Add(OrgForRec(_hospitals[listIndex]));
            }

            return orgs;
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
                Meta = new Hl7.Fhir.Model.Meta()
                {
                    Security = FhirTriplet.SecurityTest.GetCodingList(),
                },
                Id = _connectathonFlag ? hosp.ID : IdForOrg(hosp.OBJECTID),
                Identifier = _connectathonFlag
                    ? IdentifierForConnectathonOrg(hosp.ID)
                    : IdentifierForOrg(hosp.OBJECTID),
                Active = true,
                Type = FhirGenerator.ConceptListForOrganizationType(),
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
                        District = hosp.COUNTY,
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
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;.</returns>
        private static List<Hl7.Fhir.Model.Identifier> IdentifierForOrg(long id) =>
            new List<Hl7.Fhir.Model.Identifier>()
            {
                new Hl7.Fhir.Model.Identifier(
                    "https://hifld-geoplatform.opendata.arcgis.com/datasets/hospitals",
                    $"{id}"),
            };

        /// <summary>Identifier for connectathon organisation.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A List&lt;Hl7.Fhir.Model.Identifier&gt;</returns>
        private static List<Hl7.Fhir.Model.Identifier> IdentifierForConnectathonOrg(string id) =>
            new List<Hl7.Fhir.Model.Identifier>()
            {
                new Hl7.Fhir.Model.Identifier(
                    "http://build.fhir.org/ig/AudaciousInquiry/saner-ig/connectathon/hospitals",
                    id),
            };

        /// <summary>Identifier for organization.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A string.</returns>
        private static string IdForOrg(long id) => $"{CommonLiterals.OrgPrefix}{id}";

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
