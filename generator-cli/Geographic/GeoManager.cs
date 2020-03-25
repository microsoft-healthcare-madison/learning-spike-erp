﻿// <copyright file="LocationManager.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        /// <summary>Initializes this object and loads location data.</summary>
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

            string filename = Path.Combine(Directory.GetCurrentDirectory(), "data", "us-zip-code-latitude-and-longitude.json");

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

            return new Hl7.Fhir.Model.Address()
            {
                Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                Type = Hl7.Fhir.Model.Address.AddressType.Both,
                City = _locations[index].fields.city,
                State = _locations[index].fields.state,
                PostalCode = _locations[index].fields.zip,
            };
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
                return new Hl7.Fhir.Model.Address()
                {
                    Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                    Type = Hl7.Fhir.Model.Address.AddressType.Both,
                    State = state,
                    City = _locationsByState[state][index].fields.city,
                    PostalCode = _locationsByState[state][index].fields.zip,
                };
            }

            return new Hl7.Fhir.Model.Address()
            {
                Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                Type = Hl7.Fhir.Model.Address.AddressType.Both,
                State = state,
                PostalCode = _locationsByState[state][index].fields.zip,
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

                return new Hl7.Fhir.Model.Address()
                {
                    Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                    Type = Hl7.Fhir.Model.Address.AddressType.Both,
                    PostalCode = postalCode,
                    City = _locationsByZip[postalCode][index].fields.city,
                    State = _locationsByZip[postalCode][index].fields.state,
                };
            }

            return new Hl7.Fhir.Model.Address()
            {
                Use = Hl7.Fhir.Model.Address.AddressUse.Work,
                Type = Hl7.Fhir.Model.Address.AddressType.Both,
                PostalCode = postalCode,
            };
        }
    }
}
