// <copyright file="FormatFieldOption.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A format field option.</summary>
    public class FormatFieldOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatFieldOption"/>
        /// class.
        /// </summary>
        /// <param name="text">The text.</param>
        public FormatFieldOption(string text)
        {
            Text = text;
        }

        /// <summary>Gets the text.</summary>
        /// <value>The text.</value>
        public string Text { get; }
    }
}
