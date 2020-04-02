// <copyright file="MeasureInfo.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using generator_cli.Models;

namespace generator_cli.Generators
{
    /// <summary>Information about the measure.</summary>
    public class MeasureInfo
    {
        /// <summary>The canonical URL base.</summary>
        private const string _cdcCanonicalUrl = "http://cdcmeasures.example.org/modules/covid19/20200331";

        /// <summary>The canonical URL base.</summary>
        private const string _sanerCanonicalUrl = "http://saner.example.org/covid19/20200331";

        /// <summary>List of cdc documents.</summary>
        private static readonly List<string> _cdcDocumentList = new List<string>()
        {
            "https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf",
        };

        /// <summary>Initializes a new instance of the <see cref="MeasureInfo"/> class.</summary>
        /// <param name="source">     The source.</param>
        /// <param name="name">       The name.</param>
        /// <param name="title">      The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="measureType">The type of the measure.</param>
        public MeasureInfo(
            MeasureSource source,
            string name,
            string title,
            string description,
            FhirTriplet measureType)
        {
            Source = source;
            Name = name;
            Title = title;
            Description = description;
            MeasureType = measureType;
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

        /// <summary>Gets the canonical.</summary>
        /// <value>The canonical.</value>
        public string Canonical
        {
            get
            {
                switch (Source)
                {
                    case MeasureSource.CDC:
                        return _cdcCanonicalUrl;

                    case MeasureSource.FEMA:
                        break;
                    case MeasureSource.SANER:
                        return _sanerCanonicalUrl;

                    default:
                        break;
                }

                return null;
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
                        return _cdcDocumentList;

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
    }
}
