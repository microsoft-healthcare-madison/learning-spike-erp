// <copyright file="MeasureGroupExtension.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A format field extension.</summary>
    public class MeasureGroupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">       The key.</param>
        /// <param name="text">      The text.</param>
        /// <param name="properties">The properties.</param>
        public MeasureGroupExtension(string key, string text, List<FhirTriplet> properties)
        {
            Key = key;
            Text = text;
            Properties = properties;
            ValueString = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">       The key.</param>
        /// <param name="properties">The properties.</param>
        public MeasureGroupExtension(string key, List<FhirTriplet> properties)
        {
            Key = key;
            Text = null;
            Properties = properties;
            ValueString = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">     The key.</param>
        /// <param name="property">The property.</param>
        public MeasureGroupExtension(string key, FhirTriplet property)
        {
            Key = key;
            Text = null;
            Properties = new List<FhirTriplet>()
            {
                property,
            };
            ValueString = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">  The key.</param>
        /// <param name="value">The value.</param>
        public MeasureGroupExtension(string key, string value)
        {
            Key = key;
            Text = null;
            Properties = null;
            ValueString = value;
        }

        /// <summary>Gets the key.</summary>
        /// <value>The key.</value>
        public string Key { get; }

        /// <summary>Gets the text.</summary>
        /// <value>The text.</value>
        public string Text { get; }

        /// <summary>Gets the properties.</summary>
        /// <value>The properties.</value>
        public List<FhirTriplet> Properties { get; }

        /// <summary>Gets the value string.</summary>
        /// <value>The value string.</value>
        public string ValueString { get; }
    }
}
