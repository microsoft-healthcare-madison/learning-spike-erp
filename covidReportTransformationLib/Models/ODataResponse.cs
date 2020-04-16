// <copyright file="ODataResponse.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Models
{
    /// <summary>A data response.</summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public class ODataResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataResponse{T}"/>
        /// class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="value">  The value.</param>
        [Newtonsoft.Json.JsonConstructor]
        public ODataResponse(string context, List<T> value)
        {
            Context = context;
            Value = value;
        }

        /// <summary>Gets the context.</summary>
        /// <value>The context.</value>
        [Newtonsoft.Json.JsonProperty(PropertyName = "@odata.context")]
        public string Context { get; }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public List<T> Value { get; }
    }
}
