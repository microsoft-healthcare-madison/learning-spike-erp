// <copyright file="MeasureInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using generator_cli.Models;
using Hl7.Fhir.Model;

namespace generator_cli.Generators
{
    /// <summary>Information about the measure.</summary>
    public class MeasureInfo
    {
        /// <summary>The canonical URL base.</summary>
        public const string CdcCanonicalUrl = "http://build.fhir.org/ig/AudaciousInquiry/saner-ig";

        /// <summary>The canonical URL base.</summary>
        public const string FemaCanonicalUrl = "http://build.fhir.org/ig/AudaciousInquiry/saner-ig";

        /// <summary>The canonical URL base.</summary>
        public const string SanerCanonicalUrl = "http://build.fhir.org/ig/AudaciousInquiry/saner-ig";

        /// <summary>List of cdc documents.</summary>
        public static readonly List<RelatedArtifact> CdcDocuments = new List<RelatedArtifact>()
        {
            new RelatedArtifact()
            {
                Type = RelatedArtifact.RelatedArtifactType.Documentation,
                Url = "https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf",
                Label = "Importing COVID-19 Patient Module Denominator data for Patient Safety Component",
                Display = "NHSN COVID-19 Patient Module Denominator Import File Format",
            },
        };

        /// <summary>List of fema documents.</summary>
        public static readonly List<RelatedArtifact> FemaDocuments = new List<RelatedArtifact>()
        {
            new RelatedArtifact()
                {
                    Type = RelatedArtifact.RelatedArtifactType.Documentation,
                    Url = "https://github.com/AudaciousInquiry/saner-ig/blob/master/resources/Template%20for%20Daily%20Hospital%20COVID-19%20Reporting.xlsx",
                    Label = "FEMA Template for Daily Hospital COVID-19 Reporting",
                    Display = "FEMA Template for Daily Hospital COVID-19 Reporting",
                },
        };

        /// <summary>Initializes a new instance of the <see cref="MeasureInfo"/> class.</summary>
        /// <param name="source">     The source.</param>
        /// <param name="name">       The name.</param>
        /// <param name="title">      The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="measureType">The type of the measure.</param>
        /// <param name="style">      The style.</param>
        public MeasureInfo(
            MeasureSource source,
            string name,
            string title,
            string description,
            FhirTriplet measureType,
            MeasureStyle style)
        {
            Source = source;
            Name = name;
            Title = title;
            Description = description;
            MeasureType = measureType;
            Style = style;
        }

        /// <summary>Values that represent measure sources.</summary>
        public enum MeasureSource
        {
            /// <summary>An enum constant representing the cdc option.</summary>
            CDC,

            /// <summary>An enum constant representing the fema option.</summary>
            FEMA,

            /// <summary>An enum constant representing the saner option.</summary>
            SANER,
        }

        /// <summary>Values that represent measure styles.</summary>
        public enum MeasureStyle
        {
            /// <summary>An enum constant representing the count option.</summary>
            Count,

            /// <summary>An enum constant representing the ratio option.</summary>
            Ratio,
        }

        /// <summary>Gets the source for the.</summary>
        /// <value>The source.</value>
        public MeasureSource Source { get; }

        /// <summary>Gets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>Gets the title.</summary>
        /// <value>The title.</value>
        public string Title { get; }

        /// <summary>Gets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; }

        /// <summary>Gets the type of the measure.</summary>
        /// <value>The type of the measure.</value>
        public FhirTriplet MeasureType { get; }

        /// <summary>Gets the measure style.</summary>
        /// <value>The measure style.</value>
        public MeasureStyle Style { get; }

        /// <summary>Gets the canonical.</summary>
        /// <value>The canonical.</value>
        public string Canonical
        {
            get
            {
                return Source switch
                {
                    MeasureSource.CDC => CdcCanonicalUrl,
                    MeasureSource.FEMA => FemaCanonicalUrl,
                    MeasureSource.SANER => SanerCanonicalUrl,
                    _ => null,
                };
            }
        }

        /// <summary>Gets the group the measure belongs to.</summary>
        /// <value>The measure group.</value>
        public Measure.GroupComponent MeasureGroupCohort
        {
            get
            {
                Measure.GroupComponent component = null;

                switch (Style)
                {
                    case MeasureStyle.Count:
                        component = new Measure.GroupComponent()
                        {
                            Code = new CodeableConcept(
                                CdcCanonicalUrl,
                                Name,
                                Description),
                            Population = new List<Measure.PopulationComponent>()
                            {
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.InitialPopulation.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = CriteriaDescription,
                                        Language = "text/plain",
                                        Expression_ = Description,
                                    },
                                },
                            },
                        };

                        break;
                    case MeasureStyle.Ratio:
                        SplitForRatio(
                            CriteriaDescription,
                            out string numeratorCriteria,
                            out string denominatorCriteria);

                        SplitForRatio(
                            Description,
                            out string numeratorDescription,
                            out string denominatorDescription);

                        component = new Measure.GroupComponent()
                        {
                            Code = new CodeableConcept(
                                CdcCanonicalUrl,
                                Name,
                                Description),
                            Population = new List<Measure.PopulationComponent>()
                            {
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Numerator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = numeratorCriteria,
                                        Language = "text/plain",
                                        Expression_ = numeratorDescription,
                                    },
                                },
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Denominator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = denominatorCriteria,
                                        Language = "text/plain",
                                        Expression_ = denominatorDescription,
                                    },
                                },
                            },
                        };

                        break;

                    default:
                        break;
                }

                return component;
            }
        }

        /// <summary>Gets the measure group proportion.</summary>
        /// <value>The measure group proportion.</value>
        public Measure.GroupComponent MeasureGroupProportion
        {
            get
            {
                Measure.GroupComponent component = null;

                switch (Style)
                {
                    case MeasureStyle.Count:
                        component = new Measure.GroupComponent()
                        {
                            Code = new CodeableConcept(
                                CdcCanonicalUrl,
                                Name,
                                Description),
                            Population = new List<Measure.PopulationComponent>()
                            {
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Numerator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = CriteriaDescription,
                                        Language = "text/plain",
                                        Expression_ = Description,
                                    },
                                },
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Denominator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = "One",
                                        Language = "text/plain",
                                        Expression_ = "One",
                                    },
                                },
                            },
                        };

                        break;
                    case MeasureStyle.Ratio:
                        SplitForRatio(
                            CriteriaDescription,
                            out string numeratorCriteria,
                            out string denominatorCriteria);

                        SplitForRatio(
                            Description,
                            out string numeratorDescription,
                            out string denominatorDescription);

                        component = new Measure.GroupComponent()
                        {
                            Code = new CodeableConcept(
                                CdcCanonicalUrl,
                                Name,
                                Description),
                            Population = new List<Measure.PopulationComponent>()
                            {
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Numerator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = numeratorCriteria,
                                        Language = "text/plain",
                                        Expression_ = numeratorDescription,
                                    },
                                },
                                new Measure.PopulationComponent()
                                {
                                    Code = FhirTriplet.Denominator.Concept,
                                    Criteria = new Expression()
                                    {
                                        Description = denominatorCriteria,
                                        Language = "text/plain",
                                        Expression_ = denominatorDescription,
                                    },
                                },
                            },
                        };

                        break;

                    default:
                        break;
                }

                return component;
            }
        }

        /// <summary>Gets information describing the criteria.</summary>
        /// <value>Information describing the criteria.</value>
        public string CriteriaDescription
        {
            get
            {
                return Source switch
                {
                    MeasureSource.CDC => $"CDC defined field: {Name}",
                    MeasureSource.FEMA => $"FEMA defined field: {Name}",
                    MeasureSource.SANER => $"SANER defined Measure: {Name}",
                    _ => Description,
                };
            }
        }

        /// <summary>Gets the artifacts.</summary>
        /// <value>The artifacts.</value>
        public List<RelatedArtifact> Artifacts
        {
            get
            {
                return Source switch
                {
                    MeasureSource.CDC => CdcDocuments,
                    MeasureSource.FEMA => FemaDocuments,
                    MeasureSource.SANER => null,
                    _ => null,
                };
            }
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
