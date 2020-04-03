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
        public const string SanerCanonicalUrl = "http://build.fhir.org/ig/AudaciousInquiry/saner-ig";

        /// <summary>List of cdc documents.</summary>
        public static readonly List<string> CdcDocumentList = new List<string>()
        {
            "https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf",
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
                switch (Source)
                {
                    case MeasureSource.CDC:
                        return CdcCanonicalUrl;

                    case MeasureSource.FEMA:
                        break;
                    case MeasureSource.SANER:
                        return SanerCanonicalUrl;

                    default:
                        break;
                }

                return null;
            }
        }

        /// <summary>Gets the group the measure belongs to.</summary>
        /// <value>The measure group.</value>
        public Measure.GroupComponent MeasureGroup
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
                switch (Source)
                {
                    case MeasureSource.CDC:
                        return $"CDC defined field: {Name}";

                    case MeasureSource.FEMA:
                        return $"FEMA defined field: {Name}";

                    case MeasureSource.SANER:
                        return $"SANER defined Measure: {Name}";

                    default:
                        break;
                }

                return Description;
            }
        }

        /// <summary>Gets the documents.</summary>
        /// <value>The documents.</value>
        public List<string> DocumentUrls
        {
            get
            {
                switch (Source)
                {
                    case MeasureSource.CDC:
                        return CdcDocumentList;

                    case MeasureSource.FEMA:
                        break;
                    case MeasureSource.SANER:
                        break;
                    default:
                        break;
                }

                return null;
            }
        }

        /// <summary>Gets the artifacts.</summary>
        /// <value>The artifacts.</value>
        public List<RelatedArtifact> Artifacts
        {
            get
            {
                List<RelatedArtifact> artifactList = new List<RelatedArtifact>();

                foreach (string relatedDocumentUrl in DocumentUrls)
                {
                    switch (Source)
                    {
                        case MeasureSource.CDC:
                            artifactList.Add(
                                new RelatedArtifact()
                                {
                                    Type = RelatedArtifact.RelatedArtifactType.Documentation,
                                    Url = relatedDocumentUrl,
                                    Label = "Importing COVID-19 Patient Module Denominator data for Patient Safety Component",
                                    Display = "NHSN COVID-19 Patient Module Denominator Import File Format",
                                });
                            break;

                        case MeasureSource.FEMA:
                            artifactList.Add(
                                new RelatedArtifact()
                                {
                                    Type = RelatedArtifact.RelatedArtifactType.Documentation,
                                    Url = relatedDocumentUrl,
                                });
                            break;
                        case MeasureSource.SANER:
                            artifactList.Add(
                                new RelatedArtifact()
                                {
                                    Type = RelatedArtifact.RelatedArtifactType.Documentation,
                                    Url = relatedDocumentUrl,
                                });
                            break;
                        default:
                            artifactList.Add(
                                new RelatedArtifact()
                                {
                                    Type = RelatedArtifact.RelatedArtifactType.Documentation,
                                    Url = relatedDocumentUrl,
                                });
                            break;
                    }
                }

                if (artifactList.Count == 0)
                {
                    return null;
                }

                return artifactList;
            }
        }
    }
}
