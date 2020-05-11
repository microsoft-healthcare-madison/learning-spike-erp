// <copyright file="QuestionnaireQuestion.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A questionnaire question.</summary>
    public class QuestionnaireQuestion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireQuestion"/>
        /// class.
        /// </summary>
        /// <param name="valueFieldName">  The name of the value field.</param>
        /// <param name="useTitleOnly">    True if use title only, false if not.</param>
        /// <param name="displayFieldName">The name of the display field.</param>
        /// <param name="fieldSystem">     The field system.</param>
        public QuestionnaireQuestion(
            string valueFieldName,
            bool useTitleOnly,
            string displayFieldName,
            string fieldSystem)
        {
            ValueFieldName = valueFieldName;
            UseTitleOnly = useTitleOnly;
            DisplayFieldName = displayFieldName;
            FieldSystem = fieldSystem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireQuestion"/>
        /// class.
        /// </summary>
        /// <param name="fieldName">  Name of the field.</param>
        /// <param name="fieldSystem">The field system.</param>
        public QuestionnaireQuestion(string fieldName, string fieldSystem)
        {
            ValueFieldName = fieldName;
            UseTitleOnly = false;
            FieldSystem = fieldSystem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireQuestion"/>
        /// class.
        /// </summary>
        /// <param name="fieldName">   Name of the field.</param>
        /// <param name="useTitleOnly">True if use title only, false if not.</param>
        /// <param name="fieldSystem"> The field system.</param>
        public QuestionnaireQuestion(string fieldName, bool useTitleOnly, string fieldSystem)
        {
            ValueFieldName = fieldName;
            UseTitleOnly = useTitleOnly;
            FieldSystem = fieldSystem;
        }

        /// <summary>Gets the name of the value field.</summary>
        /// <value>The name of the value field.</value>
        public string ValueFieldName { get; }

        /// <summary>Gets a value indicating whether this object use title only.</summary>
        /// <value>True if use title only, false if not.</value>
        public bool UseTitleOnly { get; }

        /// <summary>Gets the name of the display field.</summary>
        /// <value>The name of the display field.</value>
        public string DisplayFieldName { get; }

        /// <summary>Gets the field system.</summary>
        /// <value>The field system.</value>
        public string FieldSystem { get; }
    }
}
