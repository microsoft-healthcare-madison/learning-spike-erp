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
        /// <param name="text">       The text.</param>
        /// <param name="isExclusive">True if this object is exclusive, false if not.</param>
        public FormatFieldOption(string text, bool isExclusive)
        {
            Text = text;
            IsExclusive = isExclusive;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatFieldOption"/>
        /// class.
        /// </summary>
        /// <param name="text">The text.</param>
        public FormatFieldOption(string text)
        {
            Text = text;
            IsExclusive = true;
        }

        /// <summary>Gets the text.</summary>
        /// <value>The text.</value>
        public string Text { get; }

        /// <summary>Gets a value indicating whether this object is exclusive.</summary>
        /// <value>True if this object is exclusive, false if not.</value>
        public bool IsExclusive { get; }
    }
}
