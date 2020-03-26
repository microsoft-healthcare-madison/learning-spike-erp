// <copyright file="LocationManager.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using generator_cli.Generators;
using Newtonsoft.Json;

namespace generator_cli.Geographic
{
    /// <summary>Manager for locations.</summary>
    public abstract class GeoManager
    {
        private static List<ZipGeoCode> _locations = null;
        private static Dictionary<string, List<ZipGeoCode>> _locationsByZip = null;
        private static Dictionary<string, List<ZipGeoCode>> _locationsByState = null;

        private static Random _rand = null;

        private static int _minBeds = 0;
        private static int _maxBeds = 0;

        /// <summary>Initializes this object and loads location data.</summary>
        /// <param name="seed">         (Optional) The seed.</param>
        /// <param name="minBeds">      (Optional) The minimum beds.</param>
        /// <param name="maxBeds">      (Optional) The maximum beds.</param>
        /// <param name="dataDirectory">(Optional) Pathname of the data directory.</param>
        public static void Init(
            int seed = 0,
            int minBeds = 10,
            int maxBeds = 5000,
            string dataDirectory = null)
        {
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

            string filename = string.IsNullOrEmpty(dataDirectory)
                ? Path.Combine(Directory.GetCurrentDirectory(), "data", "us-zip-code-latitude-and-longitude.json")
                : Path.Combine(dataDirectory, "us-zip-code-latitude-and-longitude.json");

            _locations = JsonConvert.DeserializeObject<List<ZipGeoCode>>(File.ReadAllText(filename));

            _locationsByZip = new Dictionary<string, List<ZipGeoCode>>();
            _locationsByState = new Dictionary<string, List<ZipGeoCode>>();
            foreach (ZipGeoCode loc in _locations)
            {
                if (!_locationsByZip.ContainsKey(loc.fields.zip))
                {
                    _locationsByZip.Add(loc.fields.zip, new List<ZipGeoCode>());
                }

                _locationsByZip[loc.fields.zip].Add(loc);

                // string cityState = $"{loc.fields.city},{loc.fields.state}".ToUpperInvariant();
                if (!_locationsByState.ContainsKey(loc.fields.state))
                {
                    _locationsByState.Add(loc.fields.state, new List<ZipGeoCode>());
                }

                _locationsByState[loc.fields.state].Add(loc);
            }

            Console.WriteLine(
                $"Loaded {_locations.Count} location records" +
                $", {_locationsByState.Count} states" +
                $", {_locationsByZip.Count} zip codes");
        }

        /// <summary>Gets any address.</summary>
        /// <returns>any address.</returns>
        public static Hl7.Fhir.Model.Address GetAnyAddress()
        {
            // note: Random.next max is exclusive
            int index = _rand.Next(0, _locations.Count);

            return CreateAddress(
                _locations[index].fields.city,
                _locations[index].fields.state,
                _locations[index].fields.zip);
        }

        /// <summary>Beds for hospital.</summary>
        /// <returns>An int.</returns>
        public static int BedsForHospital()
        {
            return _rand.Next(_minBeds, _maxBeds);
        }

        /// <summary>Address for state.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="state">   The state.</param>
        /// <param name="fillCity">(Optional) True to fill city.</param>
        /// <returns>The Hl7.Fhir.Model.Address.</returns>
        public static Hl7.Fhir.Model.Address GetAddressForState(
            string state,
            bool fillCity = true)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (!_locationsByState.ContainsKey(state))
            {
                throw new ArgumentOutOfRangeException(nameof(state));
            }

            // note: Random.next max is exclusive
            int index = _rand.Next(0, _locationsByState[state].Count);

            if (fillCity)
            {
                return CreateAddress(
                    _locationsByState[state][index].fields.city,
                    state,
                    _locationsByState[state][index].fields.zip);
            }

            return CreateAddress(
                string.Empty,
                state,
                _locationsByState[state][index].fields.zip);
        }

        /// <summary>Creates the address.</summary>
        /// <param name="city">      The city.</param>
        /// <param name="state">     The state.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <returns>The new address.</returns>
        private static Hl7.Fhir.Model.Address CreateAddress(
            string city,
            string state,
            string postalCode)
        {
            return new Hl7.Fhir.Model.Address()
            {
                Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                Type = Hl7.Fhir.Model.Address.AddressType.Both,
                City = city,
                State = state,
                PostalCode = postalCode,
                Country = "USA",
            };
        }

        /// <summary>Gets the organizations.</summary>
        /// <param name="requestedCount">Number of.</param>
        /// <param name="state">         (Optional) The state.</param>
        /// <param name="postalCode">    (Optional) The postal code.</param>
        /// <returns>The organizations.</returns>
        public static List<Hl7.Fhir.Model.Organization> GetOrganizations(
            int requestedCount,
            string state = null,
            string postalCode = null)
        {
            List<Hl7.Fhir.Model.Organization> orgs = new List<Hl7.Fhir.Model.Organization>();

            orgs.Add(GetOrganization(state, postalCode));

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
            string localCity = string.Empty;
            string localState = string.Empty;
            string localPostalCode = string.Empty;

            // check for needing a postal code
            if (string.IsNullOrEmpty(postalCode))
            {
                // use state if present
                if ((!string.IsNullOrEmpty(state)) && _locationsByState.ContainsKey(state))
                {
                    // note: Rand.Next max is exclusive
                    int index = _rand.Next(0, _locationsByState[state].Count);

                    localCity = _locationsByState[state][index].fields.city;
                    localState = state;
                    localPostalCode = _locationsByState[state][index].fields.zip;
                }
                else
                {
                    // note: Rand.Next max is exclusive
                    int index = _rand.Next(0, _locations.Count);

                    localCity = _locations[index].fields.city;
                    localState = _locations[index].fields.state;
                    localPostalCode = _locations[index].fields.zip;
                }
            }

            // check for not having a state
            if (string.IsNullOrEmpty(localState))
            {
                // use state if present
                if ((!string.IsNullOrEmpty(postalCode)) && _locationsByZip.ContainsKey(postalCode))
                {
                    // note: Rand.Next max is exclusive
                    int index = _rand.Next(0, _locationsByZip[postalCode].Count);

                    localCity = _locationsByZip[postalCode][index].fields.city;
                    localState = _locationsByZip[postalCode][index].fields.state;
                    localPostalCode = postalCode;
                }
                else
                {
                    // note: Rand.Next max is exclusive
                    int index = _rand.Next(0, _locations.Count);

                    localCity = _locations[index].fields.city;
                    localState = _locations[index].fields.state;
                    localPostalCode = _locations[index].fields.zip;
                }
            }

            return CreateOrgForGeo(localCity, localState, localPostalCode);
        }

        /// <summary>Organisation for geo.</summary>
        /// <param name="city">      The city.</param>
        /// <param name="state">     The state.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <returns>A Hl7.Fhir.Model.Organization.</returns>
        private static Hl7.Fhir.Model.Organization CreateOrgForGeo(
            string city,
            string state,
            string postalCode)
        {
            string id = FhirGenerator.NextOrgId;
            string name = $"HOSPITAL {id}";

            return new Hl7.Fhir.Model.Organization()
            {
                Id = id,
                Identifier = FhirGenerator.IdentifierListForId(id),
                Active = true,
                Type = FhirGenerator.ConceptListForOrganizationType(),
                Name = name,
                Address = new List<Hl7.Fhir.Model.Address>()
                {
                    CreateAddress(city, state, postalCode),
                },
            };
        }

        /// <summary>Address for postal code.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="postalCode">   The postal code.</param>
        /// <param name="fillCityState">(Optional) True to fill city state.</param>
        /// <returns>The Address.</returns>
        public static Hl7.Fhir.Model.Address GetAddressForPostalCode(
            string postalCode,
            bool fillCityState = true)
        {
            if (string.IsNullOrEmpty(postalCode))
            {
                throw new ArgumentNullException(nameof(postalCode));
            }

            if (!_locationsByZip.ContainsKey(postalCode))
            {
                throw new ArgumentOutOfRangeException(nameof(postalCode));
            }

            if (fillCityState)
            {
                // note: Random.next max is exclusive
                int index = _rand.Next(0, _locationsByZip[postalCode].Count);

                return CreateAddress(
                    _locationsByZip[postalCode][index].fields.city,
                    _locationsByZip[postalCode][index].fields.state,
                    postalCode);
            }

            return CreateAddress(
                string.Empty,
                string.Empty,
                postalCode);
        }

        /// <summary>Position for postal code.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="postalCode">The postal code.</param>
        /// <returns>A Hl7.Fhir.Model.Location.PositionComponent.</returns>
        public static Hl7.Fhir.Model.Location.PositionComponent PositionForPostalCode(string postalCode)
        {
            if (string.IsNullOrEmpty(postalCode))
            {
                throw new ArgumentNullException(nameof(postalCode));
            }

            if (!_locationsByZip.ContainsKey(postalCode))
            {
                throw new ArgumentOutOfRangeException(nameof(postalCode));
            }

            return new Hl7.Fhir.Model.Location.PositionComponent()
            {
                Longitude = (decimal)_locationsByZip[postalCode][0].fields.longitude,
                Latitude = (decimal)_locationsByZip[postalCode][0].fields.latitude,
            };
        }

        /// <summary>Query if 'postalCode' is postal code known.</summary>
        /// <param name="postalCode">The postal code.</param>
        /// <returns>True if postal code known, false if not.</returns>
        public static bool IsPostalCodeKnown(string postalCode)
        {
            return _locationsByZip.ContainsKey(postalCode);
        }
    }
}
