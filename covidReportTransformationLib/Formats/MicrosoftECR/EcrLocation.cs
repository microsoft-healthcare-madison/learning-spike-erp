﻿// <auto-generated/>
// <copyright file="EcrLocation.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.MicrosoftECR
{
    /// <summary>An ecr location.</summary>
    public class EcrLocation
    {
        public const string EntityName = "msft_location";
        public const string EntityPlural = "msft_locations";

        public string odataetag { get; set; }
        public int? statecode { get; set; }
        public string msft_locationid { get; set; }
        public int? statuscode { get; set; }
        public bool msft_alternatesiteflag { get; set; }
        public int? msft_totalbeds { get; set; }
        public DateTime createdon { get; set; }
        public int? importsequencenumber { get; set; }
        public string msft_unit { get; set; }
        public string _ownerid_value { get; set; }
        public DateTime modifiedon { get; set; }
        public int? versionnumber { get; set; }
        public string msft_locationname { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public DateTime msft_effectivestatedate { get; set; }
        public int? msft_occupancypercentage { get; set; }
        public string _modifiedby_value { get; set; }
        public string _msft_factility_value { get; set; }
        public int? msft_lastcensus { get; set; }
        public string msft_floor { get; set; }
        public string _msft_acuity_value { get; set; }
        public string _createdby_value { get; set; }
        public int? msft_blockedbeds { get; set; }
        public string _owningbusinessunit_value { get; set; }
        public string _owninguser_value { get; set; }
        public object _modifiedonbehalfby_value { get; set; }
        public object utcconversiontimezonecode { get; set; }
        public object _createdonbehalfby_value { get; set; }
        public object msft_locationorder { get; set; }
        public object msft_effectiveenddate { get; set; }
        public object msft_locationacuity { get; set; }
        public object _owningteam_value { get; set; }
        public object overriddencreatedon { get; set; }

        /// <summary>Gets an URI.</summary>
        /// <param name="baseUri">   URI of the base.</param>
        /// <param name="facilityId">(Optional) Identifier for the facility.</param>
        /// <returns>The URI.</returns>
        public static Uri GetUri(Uri baseUri, string facilityId = null)
        {
            if (string.IsNullOrEmpty(facilityId))
            {
                return new Uri(baseUri, EntityPlural);
            }

            return new Uri(
                baseUri,
                $"{EntityPlural}?$filter=(_msft_factility_value%20eq%20%27{facilityId}%27)");
        }
    }
}
