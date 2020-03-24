// <copyright file="LocationManager.cs" company="Microsoft Corporation">
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
        private static Dictionary<string, List<ZipGeoCode>> _locationsByCityState = null;

        private static Random _rand = new Random();

        /// <summary>Initializes this object and loads location data.</summary>
        public static void Init()
        {
            string filename = Path.Combine(Directory.GetCurrentDirectory(), "data", "us-zip-code-latitude-and-longitude.json");

            _locations = JsonConvert.DeserializeObject<List<ZipGeoCode>>(File.ReadAllText(filename));

            _locationsByZip = new Dictionary<string, List<ZipGeoCode>>();
            _locationsByCityState = new Dictionary<string, List<ZipGeoCode>>();
            foreach (ZipGeoCode loc in _locations)
            {
                if (!_locationsByZip.ContainsKey(loc.fields.zip))
                {
                    _locationsByZip.Add(loc.fields.zip, new List<ZipGeoCode>());
                }

                _locationsByZip[loc.fields.zip].Add(loc);

                string cityState = $"{loc.fields.city},{loc.fields.state}".ToUpperInvariant();

                if (!_locationsByCityState.ContainsKey(cityState))
                {
                    _locationsByCityState.Add(cityState, new List<ZipGeoCode>());
                }

                _locationsByCityState[cityState].Add(loc);
            }

            Console.WriteLine(
                $"Loaded {_locations.Count} location records" +
                $", {_locationsByCityState.Count} city/states" +
                $", {_locationsByZip.Count} zip codes");
        }

        /// <summary>Address for postal code.</summary>
        /// <exception cref="ArgumentNullException">      Thrown when one or more required arguments are
        ///  null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the
        ///  required range.</exception>
        /// <param name="postalCode">   The postal code.</param>
        /// <param name="fillCityState">(Optional) True to fill city state.</param>
        /// <returns>The Address.</returns>
        public static Hl7.Fhir.Model.Address AddressForPostalCode(
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
                    PostalCode = postalCode,
                    City = _locationsByZip[postalCode][index].fields.city,
                    State = _locationsByZip[postalCode][index].fields.state,
                };
            }

            return new Hl7.Fhir.Model.Address()
            {
                PostalCode = postalCode,
            };
        }
    }
}
