// <copyright file="QuestionnaireSection.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A questionnaire section.</summary>
    public class QuestionnaireSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireSection"/>
        /// class.
        /// </summary>
        /// <param name="title"> The title.</param>
        /// <param name="fields">The fields.</param>
        public QuestionnaireSection(
            string title,
            List<QuestionnaireQuestion> fields)
        {
            Title = title;
            Fields = fields;
        }

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title { get; }

        /// <summary>Gets the fields.</summary>
        /// <value>The fields.</value>
        public List<QuestionnaireQuestion> Fields { get; }
    }
}
