// <copyright file="SanerCommon.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.SANER
{
    /// <summary>A saner common.</summary>
    public abstract class SanerCommon
    {
        /// <summary>The canonical URL base.</summary>
        public const string CanonicalUrl = "http://build.fhir.org/ig/AudaciousInquiry/saner-ig";

        /// <summary>The measure version.</summary>
        public const string MeasureVersion = "20200421.01";

        /// <summary>The questionnaire version.</summary>
        public const string QuestionnaireVersion = "20200422.01";

        /// <summary>The publication date.</summary>
        public const string PublicationDate = "2020-04-21T00:00:00Z";

        /// <summary>The publisher.</summary>
        public const string Publisher = "HL7 SANER-IG";
    }
}
