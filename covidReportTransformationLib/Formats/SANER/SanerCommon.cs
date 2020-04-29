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
        public const string CanonicalUrl = "http://hl7.org/fhir/us/saner";

        /// <summary>The measure version.</summary>
        public const string MeasureVersion = "0.1.0";

        /// <summary>The questionnaire version.</summary>
        public const string QuestionnaireVersion = "0.1.0";

        /// <summary>The publication date.</summary>
        public const string PublicationDate = "2020-04-27T07:08:50-04:00";

        /// <summary>The publisher.</summary>
        public const string Publisher = "HL7 International";

        /// <summary>The contacts.</summary>
        public static readonly List<Hl7.Fhir.Model.ContactDetail> Contacts = new List<Hl7.Fhir.Model.ContactDetail>()
        {
            new Hl7.Fhir.Model.ContactDetail()
            {
                Name = "HL7 Patient Administration Workgroup",
                Telecom = new List<Hl7.Fhir.Model.ContactPoint>()
                {
                    new Hl7.Fhir.Model.ContactPoint(
                        Hl7.Fhir.Model.ContactPoint.ContactPointSystem.Url,
                        null,
                        "http://hl7.org/Special/committees/pafm/index.cfm"),
                },
            },
            new Hl7.Fhir.Model.ContactDetail()
            {
                Name = "Audacious Inquiry",
                Telecom = new List<Hl7.Fhir.Model.ContactPoint>()
                {
                    new Hl7.Fhir.Model.ContactPoint(
                        Hl7.Fhir.Model.ContactPoint.ContactPointSystem.Url,
                        null,
                        "http://ainq.com"),
                },
            },
            new Hl7.Fhir.Model.ContactDetail()
            {
                Name = "Keith W. Boone",
                Telecom = new List<Hl7.Fhir.Model.ContactPoint>()
                {
                    new Hl7.Fhir.Model.ContactPoint(
                        Hl7.Fhir.Model.ContactPoint.ContactPointSystem.Email,
                        null,
                        "mailto:kboone@ainq.com"),
                },
            },
        };
    }
}
