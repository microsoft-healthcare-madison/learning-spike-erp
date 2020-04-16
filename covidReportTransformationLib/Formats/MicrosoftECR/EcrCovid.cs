﻿// <auto-generated/>
// <copyright file="EcrCovid.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.MicrosoftECR
{
    /// <summary>An ecr covid.</summary>
    public class EcrCovid
    {
        public const string EntityName = "msft_covid";
        public const string EntityPlural = "msft_covids";

        public string odataetag { get; set; }
        public int? msft_covidpui { get; set; }
        public string _owningbusinessunit_value { get; set; }
        public int? statecode { get; set; }
        public int? statuscode { get; set; }
        public string _msft_location_value { get; set; }
        public string _createdby_value { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public DateTime msft_coviddate { get; set; }
        public string _ownerid_value { get; set; }
        public int? msft_covidpos { get; set; }
        public string _modifiedby_value { get; set; }
        public string _owninguser_value { get; set; }
        public DateTime createdon { get; set; }
        public int? versionnumber { get; set; }
        public DateTime modifiedon { get; set; }
        public string msft_covidid { get; set; }
        public object _createdonbehalfby_value { get; set; }
        public object msft_name { get; set; }
        public object _modifiedonbehalfby_value { get; set; }
        public object importsequencenumber { get; set; }
        public object overriddencreatedon { get; set; }
        public object utcconversiontimezonecode { get; set; }
        public object _owningteam_value { get; set; }

        /// <summary>Gets an URI.</summary>
        /// <param name="baseUri">   URI of the base.</param>
        /// <param name="locationId">(Optional) Identifier for the location.</param>
        /// <returns>The URI.</returns>
        public static Uri GetUri(Uri baseUri, string locationId = null)
        {
            if (string.IsNullOrEmpty(locationId))
            {
                return new Uri(baseUri, EntityPlural);
            }

            return new Uri(
                baseUri,
                $"{EntityPlural}?$filter=(_msft_location_value%20eq%20%27{locationId}%27)");
        }
    }
}