﻿// <auto-generated/>
// <copyright file="EcrFacility.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.MicrosoftECR
{
    /// <summary>An ECR facility.</summary>
    public class EcrFacility
    {
        public const string EntityName = "msft_facility";
        public const string EntityPlural = "msft_facilities";

        public string odataetag { get; set; }
        public string _msft_region_value { get; set; }
        public string msft_addresszip { get; set; }
        public string msft_addressstreet { get; set; }
        public string msft_facilityid { get; set; }
        public string _ownerid_value { get; set; }
        public string msft_addressstate { get; set; }
        public DateTime msft_effectivestartdate { get; set; }
        public int? statuscode { get; set; }
        public string msft_addresscity { get; set; }
        public string _createdby_value { get; set; }
        public int? timezoneruleversionnumber { get; set; }
        public string msft_facilityname { get; set; }
        public int? statecode { get; set; }
        public int? msft_anticipateddischargesinnext24hours { get; set; }
        public DateTime modifiedon { get; set; }
        public string _owningbusinessunit_value { get; set; }
        public string _owninguser_value { get; set; }
        public string _modifiedby_value { get; set; }
        public int versionnumber { get; set; }
        public DateTime createdon { get; set; }
        public int? msft_yesterdaysactualdischarge { get; set; }
        public int? msft_totalvents { get; set; }
        public object _owningteam_value { get; set; }
        public object msft_effectiveenddate { get; set; }
        public int? utcconversiontimezonecode { get; set; }
        public decimal? msft_addresslat { get; set; }
        public object _createdonbehalfby_value { get; set; }
        public string msft_description { get; set; }
        public decimal? msft_addresslong { get; set; }
        public object _modifiedonbehalfby_value { get; set; }
        public object overriddencreatedon { get; set; }
        public object importsequencenumber { get; set; }

        /// <summary>Gets an URI.</summary>
        /// <param name="baseUri">(Optional) URI of the base.</param>
        /// <returns>The URI.</returns>
        public static Uri GetUri(Uri baseUri)
        {
            return new Uri(baseUri, EntityPlural);
        }
    }
}
