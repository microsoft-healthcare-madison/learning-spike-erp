// <copyright file="MeasureGroupInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats
{
    /// <summary>Information about the measure group.</summary>
    public class MeasureGroupInfo
    {

        /// <summary>Values that represent group systems.</summary>
        public enum SanerGroupSystem
        {
            /// <summary>An enum constant representing the none option.</summary>
            None,

            /// <summary>An enum constant representing the beds option.</summary>
            Beds,

            /// <summary>An enum constant representing the ventilators option.</summary>
            Ventilators,

            /// <summary>An enum constant representing the encounters option.</summary>
            Encounters,
        }

        /// <summary>Gets the code.</summary>
        /// <value>The code.</value>
        public FhirTriplet CodeCoding { get; }

        /// <summary>Gets the group code text.</summary>
        /// <value>The group code text.</value>
        public string CodeText { get; }

        /// <summary>Gets the initial population field.</summary>
        /// <value>The initial population field.</value>
        public string InitialPopulationField { get; }

        /// <summary>Gets the populations.</summary>
        /// <value>The populations.</value>
        public List<string> AdditionalPopulationFields { get; }

        /// <summary>Gets the extensions.</summary>
        /// <value>The extensions.</value>
        public List<MeasureGroupExtension> GroupAttributes { get; }

#if CAKE
                    new List<FormatFieldExtension>()
                    {
                        new FormatFieldExtension(
                            CommonLiterals.Scoring,
                            FhirTriplet.ScoringContinuousVariable),
                        new FormatFieldExtension(
                            CommonLiterals.Subject,
                            "Hospital Beds",
                            new List<FhirTriplet>()
                            {
                                FhirTriplet.ResourceLocation,
                                FhirTriplet.SctHospitalBed,
                            }),
                        new FormatFieldExtension(
                            CommonLiterals.Type,
                            FhirTriplet.MeasureTypeStructure),
                        new FormatFieldExtension(
                            CommonLiterals.ImprovementNotation,
                            FhirTriplet.ImprovementIncrease),
                        new FormatFieldExtension(
                            CommonLiterals.RateAggregation,
                            CommonLiterals.AggregableByPeriod),
                    }
#endif
    }
}
