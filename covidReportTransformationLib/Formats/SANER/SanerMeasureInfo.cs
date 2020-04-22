// <copyright file="SanerMeasureInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Models;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Formats.SANER
{
    /// <summary>Information about the SANER measure.</summary>
    public abstract class SanerMeasureInfo
    {
        /// <summary>Gets group component.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="field">The field.</param>
        /// <returns>The group component.</returns>
        public static Measure.GroupComponent GetGroupComponent(FormatField field)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            Measure.GroupComponent component = null;

            string title = field.Title ?? field.Name;
            string description = field.Description ?? field.Title ?? field.Name;

            switch (field.Type)
            {
                case FormatField.FieldType.Count:
                    component = new Measure.GroupComponent()
                    {
                        Code = new CodeableConcept(
                            SanerMeasure.CanonicalUrl,
                            field.Name,
                            description),
                        Population = new List<Measure.PopulationComponent>()
                            {
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.InitialPopulation.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = description,
                                        Language = "text/plain",
                                        Expression_ = $"Source defined field: {field.Name}",
                                    },
                                },
                            },
                    };

                    break;
                case FormatField.FieldType.Percentage:
                    SplitForRatio(
                        description,
                        out string numeratorDescription,
                        out string denominatorDescription);

                    component = new Measure.GroupComponent()
                    {
                        Code = new CodeableConcept(
                            SanerMeasure.CanonicalUrl,
                            field.Name,
                            description),
                        Population = new List<Measure.PopulationComponent>()
                            {
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Numerator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = numeratorDescription,
                                        Language = "text/plain",
                                        Expression_ = $"Numerator for source defined field: {field.Name}",
                                    },
                                },
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Denominator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = denominatorDescription,
                                        Language = "text/plain",
                                        Expression_ = $"Denominator for source defined field: {field.Name}",
                                    },
                                },
                            },
                    };

                    break;

                case FormatField.FieldType.Date:
                case FormatField.FieldType.Boolean:
                case FormatField.FieldType.Choice:
                case FormatField.FieldType.Text:
                    return null;
            }

            return component;
        }

        /// <summary>Splits for ratio.</summary>
        /// <param name="value">      The value.</param>
        /// <param name="numerator">  [out] The numerator.</param>
        /// <param name="denominator">[out] The denominator.</param>
        private static void SplitForRatio(
            string value,
            out string numerator,
            out string denominator)
        {
            if (string.IsNullOrEmpty(value))
            {
                numerator = string.Empty;
                denominator = string.Empty;
                return;
            }

            string[] split = value.Split('/');
            if (split.Length == 2)
            {
                numerator = split[0].Trim();
                denominator = split[1].Trim();
                return;
            }

            numerator = value;
            denominator = value;
            return;
        }
    }
}
