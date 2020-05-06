// <copyright file="LocationInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Models
{
    /// <summary>A data address.</summary>
    public class LocationInfo
    {
        /// <summary>Initializes a new instance of the <see cref="LocationInfo"/> class.</summary>
        /// <param name="fhirBaseUrl">  The fhir base URL.</param>
        /// <param name="fhirId">       The identifier.</param>
        /// <param name="identifiers">  The identifiers.</param>
        /// <param name="name">         The name.</param>
        /// <param name="streetAddress">The street address.</param>
        /// <param name="city">         The city.</param>
        /// <param name="state">        The state.</param>
        /// <param name="district">     The district.</param>
        /// <param name="postalCode">   The postal code.</param>
        /// <param name="country">      The country.</param>
        /// <param name="latitude">     The latitude.</param>
        /// <param name="longitude">    The longitude.</param>
        [Newtonsoft.Json.JsonConstructor]
        public LocationInfo(
            Uri fhirBaseUrl,
            string fhirId,
            List<Identifier> identifiers,
            string name,
            string streetAddress,
            string city,
            string state,
            string district,
            string postalCode,
            string country,
            decimal? latitude,
            decimal? longitude)
        {
            FhirBaseUrl = fhirBaseUrl;
            FhirId = fhirId;
            Identifiers = identifiers;
            Name = name;
            StreetAddress = streetAddress;
            City = city;
            State = state;
            District = district;
            PostalCode = postalCode;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>Gets URL of the fhir base.</summary>
        /// <value>The fhir base URL.</value>
        public Uri FhirBaseUrl { get; }

        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        public string FhirId { get; }

        /// <summary>Gets the identifiers.</summary>
        /// <value>The identifiers.</value>
        public List<Identifier> Identifiers { get; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>Gets the street address.</summary>
        /// <value>The street address.</value>
        public string StreetAddress { get; }

        /// <summary>Gets the city.</summary>
        /// <value>The city.</value>
        public string City { get; }

        /// <summary>Gets the state.</summary>
        /// <value>The state.</value>
        public string State { get; }

        /// <summary>Gets the district.</summary>
        /// <value>The district.</value>
        public string District { get; }

        /// <summary>Gets the postal code.</summary>
        /// <value>The postal code.</value>
        public string PostalCode { get; }

        /// <summary>Gets the country.</summary>
        /// <value>The country.</value>
        public string Country { get; }

        /// <summary>Gets the latitude.</summary>
        /// <value>The latitude.</value>
        public decimal? Latitude { get; }

        /// <summary>Gets the longitude.</summary>
        /// <value>The longitude.</value>
        public decimal? Longitude { get; }

        /// <summary>FHIR hir model.</summary>
        /// <param name="resourceReference">[out] The resource reference.</param>
        /// <param name="contained">        [out] The contained.</param>
        public void ToFhir(
            out ResourceReference resourceReference,
            out Resource contained)
        {
            if (!string.IsNullOrEmpty(FhirId))
            {
                string url;
                if (FhirBaseUrl == null)
                {
                    url = $"Location/{FhirId}";
                }
                else
                {
                    url = new Uri(FhirBaseUrl, $"Location/{FhirId}").ToString();
                }

                resourceReference = new ResourceReference(
                    url,
                    Name);
                contained = null;
                return;
            }

            Address address = new Address()
            {
                City = City,
                State = State,
                District = District,
                PostalCode = PostalCode,
                Country = Country,
            };

            if (!string.IsNullOrEmpty(StreetAddress))
            {
                address.Line = new string[1] { StreetAddress };
            }

            Location loc = new Location()
            {
                Address = address,
            };

            if ((Latitude != null) && (Longitude != null))
            {
                loc.Position = new Location.PositionComponent()
                {
                    Latitude = Latitude,
                    Longitude = Longitude,
                };
            }

            string id = Utils.FhirIds.NextId;

            resourceReference = new ResourceReference(
                $"#{id}",
                Name);
            contained = loc;
        }
    }
}
