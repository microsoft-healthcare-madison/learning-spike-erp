// <copyright file="MeasureGroupingExtension.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Utils;

namespace covidReportTransformationLib.Formats
{
    /// <summary>A format field extension.</summary>
    public class MeasureGroupingExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupingExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">       The key.</param>
        /// <param name="text">      The text.</param>
        /// <param name="properties">The properties.</param>
        public MeasureGroupingExtension(string key, string text, List<FhirTriplet> properties)
        {
            Key = key;
            Text = text;
            Properties = properties;
            ValueString = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupingExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">       The key.</param>
        /// <param name="properties">The properties.</param>
        public MeasureGroupingExtension(string key, List<FhirTriplet> properties)
        {
            Key = key;
            Text = null;
            Properties = properties;
            ValueString = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureGroupingExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">     The key.</param>
        /// <param name="property">The property.</param>
        public MeasureGroupingExtension(string key, FhirTriplet property)
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
        /// Initializes a new instance of the <see cref="MeasureGroupingExtension"/>
        /// class.
        /// </summary>
        /// <param name="key">  The key.</param>
        /// <param name="value">The value.</param>
        public MeasureGroupingExtension(string key, string value)
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

        /// <summary>Gets a list of bed.</summary>
        /// <value>A list of bed.</value>
        public static List<MeasureGroupingExtension> BedList => new List<MeasureGroupingExtension>()
        {
            new MeasureGroupingExtension(
                CommonLiterals.Scoring,
                FhirTriplet.ScoringContinuousVariable),
            new MeasureGroupingExtension(
                CommonLiterals.Subject,
                "Hospital Beds",
                new List<FhirTriplet>()
                {
                    FhirTriplet.ResourceLocation,
                    FhirTriplet.SctHospitalBed,
                }),
            new MeasureGroupingExtension(
                CommonLiterals.Type,
                FhirTriplet.MeasureTypeStructure),
            new MeasureGroupingExtension(
                CommonLiterals.ImprovementNotation,
                FhirTriplet.ImprovementIncrease),
            new MeasureGroupingExtension(
                CommonLiterals.RateAggregation,
                CommonLiterals.AggregableByPeriod),
        };

        /// <summary>Gets a list of encounters.</summary>
        /// <value>A list of encounters.</value>
        public static List<MeasureGroupingExtension> EncounterList => new List<MeasureGroupingExtension>()
        {
            new MeasureGroupingExtension(
                CommonLiterals.Scoring,
                FhirTriplet.ScoringContinuousVariable),
            new MeasureGroupingExtension(
                CommonLiterals.Subject,
                "Encounter",
                new List<FhirTriplet>()
                {
                    FhirTriplet.ResourceEncounter,
                    FhirTriplet.SctPatientEncounter,
                }),
            new MeasureGroupingExtension(
                CommonLiterals.Type,
                FhirTriplet.MeasureTypeOutcome),
            new MeasureGroupingExtension(
                CommonLiterals.ImprovementNotation,
                FhirTriplet.ImprovementDecrease),
            new MeasureGroupingExtension(
                CommonLiterals.RateAggregation,
                CommonLiterals.AggregableByPeriod),
        };

        /// <summary>Gets a list of labs.</summary>
        /// <value>A list of labs.</value>
        public static List<MeasureGroupingExtension> LabList => new List<MeasureGroupingExtension>()
        {
            new MeasureGroupingExtension(
                CommonLiterals.Scoring,
                FhirTriplet.ScoringContinuousVariable),
            new MeasureGroupingExtension(
                CommonLiterals.Subject,
                "COVID-19 Diagnostic Testing",
                new List<FhirTriplet>()
                {
                    FhirTriplet.ResourceServiceRequest,
                    FhirTriplet.SctImmunologyLabTest,
                }),
            new MeasureGroupingExtension(
                CommonLiterals.Type,
                FhirTriplet.MeasureTypeOutcome),
            new MeasureGroupingExtension(
                CommonLiterals.ImprovementNotation,
                FhirTriplet.ImprovementDecrease),
            new MeasureGroupingExtension(
                CommonLiterals.RateAggregation,
                CommonLiterals.AggregableByPeriod),
        };

        /// <summary>Gets a list of nurses.</summary>
        /// <value>A list of nurses.</value>
        public static List<MeasureGroupingExtension> NurseList => new List<MeasureGroupingExtension>()
        {
            new MeasureGroupingExtension(
                CommonLiterals.Scoring,
                FhirTriplet.ScoringContinuousVariable),
            new MeasureGroupingExtension(
                CommonLiterals.Subject,
                "Nurses",
                new List<FhirTriplet>()
                {
                    FhirTriplet.ResourcePractitioner,
                    FhirTriplet.SctProfessionalNurse,
                }),
            new MeasureGroupingExtension(
                CommonLiterals.Type,
                FhirTriplet.MeasureTypeStructure),
            new MeasureGroupingExtension(
                CommonLiterals.ImprovementNotation,
                FhirTriplet.ImprovementIncrease),
            new MeasureGroupingExtension(
                CommonLiterals.RateAggregation,
                CommonLiterals.AggregableByPeriod),
        };

        /// <summary>Gets the unspecified.</summary>
        /// <value>The unspecified.</value>
        public static List<MeasureGroupingExtension> UnspecifiedList => new List<MeasureGroupingExtension>()
        {
            new MeasureGroupingExtension(
                CommonLiterals.Scoring,
                FhirTriplet.ScoringContinuousVariable),
            new MeasureGroupingExtension(
                CommonLiterals.Type,
                FhirTriplet.MeasureTypeStructure),
            new MeasureGroupingExtension(
                CommonLiterals.ImprovementNotation,
                FhirTriplet.ImprovementIncrease),
            new MeasureGroupingExtension(
                CommonLiterals.RateAggregation,
                CommonLiterals.AggregableByPeriod),
        };

        /// <summary>Gets a list of ventilators.</summary>
        /// <value>A list of ventilators.</value>
        public static List<MeasureGroupingExtension> VentilatorList => new List<MeasureGroupingExtension>()
        {
            new MeasureGroupingExtension(
                CommonLiterals.Scoring,
                FhirTriplet.ScoringContinuousVariable),
            new MeasureGroupingExtension(
                CommonLiterals.Subject,
                "Mechanical Ventilators",
                new List<FhirTriplet>()
                {
                    FhirTriplet.ResourceDevice,
                    FhirTriplet.SctVentilator,
                }),
            new MeasureGroupingExtension(
                CommonLiterals.Type,
                FhirTriplet.MeasureTypeStructure),
            new MeasureGroupingExtension(
                CommonLiterals.ImprovementNotation,
                FhirTriplet.ImprovementDecrease),
            new MeasureGroupingExtension(
                CommonLiterals.RateAggregation,
                CommonLiterals.AggregableByPeriod),
        };
    }
}
