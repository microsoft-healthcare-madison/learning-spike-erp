// <copyright file="SanerMeasure.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Formats.CDC;
using covidReportTransformationLib.Formats.FEMA;
using covidReportTransformationLib.Models;
using covidReportTransformationLib.Utils;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Formats.SANER
{
    /// <summary>A SANER measure generator.</summary>
    public static class SanerMeasure
    {
        /// <summary>The canonical URL base.</summary>
        public const string CanonicalUrl = "http://build.fhir.org/ig/AudaciousInquiry/saner-ig";

        /// <summary>The measure version.</summary>
        private const string MeasureVersion = "20200421.01";

        /// <summary>The publication date.</summary>
        private const string PublicationDate = "2020-04-21T00:00:00Z";

        /// <summary>The publisher.</summary>
        private const string Publisher = "HL7 SANER-IG";

        /// <summary>True once initialization is complete.</summary>
        private static bool _initialized = false;

        /// <summary>The measures.</summary>
        private static readonly Dictionary<string, Measure> _measures = new Dictionary<string, Measure>();

        /// <summary>Builds a measure.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="format">Describes the format to use.</param>
        /// <returns>A Measure.</returns>
        private static Measure BuildMeasure(
            IReportingFormat format)
        {
            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (string.IsNullOrEmpty(format.Name))
            {
                throw new ArgumentNullException(nameof(format), $"Invalid IReportingFormat.Name: {format.Name}");
            }

            if (format.Fields == null)
            {
                throw new ArgumentNullException(nameof(format), $"Invalid IReportingFormat.Fields: {format.Fields}");
            }

            if (format.MeasureReportFields == null)
            {
                throw new ArgumentNullException(nameof(format), $"Invalid IReportingFormat.MeasureReportFields: {format.MeasureReportFields}");
            }

            Measure measure = new Measure()
            {
                Id = format.Name,
                Name = format.Name,
                Url = $"{CanonicalUrl}/{format.Name}",
                Version = MeasureVersion,
                Title = format.Title,
                Description = new Markdown(format.Description),
                Status = PublicationStatus.Draft,
                Subject = new CodeableConcept("Location", "Location"),
                Date = PublicationDate,
                Publisher = Publisher,
                Jurisdiction = new List<CodeableConcept>()
                {
                    FhirTriplet.UnitedStates.Concept,
                },
                UseContext = new List<UsageContext>()
                {
                    new UsageContext()
                    {
                        Code = FhirTriplet.GetCode(FhirSystems.UsageContextType, CommonLiterals.ContextFocus),
                        Value = FhirTriplet.SctCovid.Concept,
                    },
                },
                Type = new List<CodeableConcept>()
                {
                    FhirTriplet.MeasureTypeComposite.Concept,
                },
                Group = new List<Measure.GroupComponent>(),
            };

            if (format.Artifacts != null)
            {
                measure.RelatedArtifact = new List<RelatedArtifact>();
                measure.RelatedArtifact.AddRange(format.Artifacts);
            }

            foreach (string field in format.MeasureReportFields)
            {
                if (!format.Fields.ContainsKey(field))
                {
                    continue;
                }

                measure.Group.Add(SanerMeasureInfo.GetGroupComponent(format.Fields[field]));
            }

            return measure;
        }

        /// <summary>Initializes this object.</summary>
        public static void Init()
        {
            if (_initialized)
            {
                return;
            }

            _measures.Add(PatientImpact.Current.Name, BuildMeasure(PatientImpact.Current));
            _measures.Add(DailyReporting.Current.Name, BuildMeasure(DailyReporting.Current));
            _initialized = true;
        }

        /// <summary>Cdc grouped measure.</summary>
        /// <returns>A Measure.</returns>
        public static Measure CDCPatientImpactMeasure()
        {
            if (!_initialized)
            {
                Init();
            }

            return _measures[PatientImpact.Current.Name];
        }

        /// <summary>Cdc patient impact bundle.</summary>
        /// <returns>A Bundle.</returns>
        public static Bundle CDCPatientImpactBundle()
        {
            if (!_initialized)
            {
                Init();
            }

            return GetBundleForMeasure(
                _measures[PatientImpact.Current.Name],
                PatientImpact.Current.Name);
        }

        /// <summary>Fema complete measure.</summary>
        /// <returns>A Measure.</returns>
        public static Measure FEMADailyMeasure()
        {
            if (!_initialized)
            {
                Init();
            }

            return _measures[DailyReporting.Current.Name];
        }

        /// <summary>Fema daily bundle.</summary>
        /// <returns>A Bundle.</returns>
        public static Bundle FEMADailyBundle()
        {
            if (!_initialized)
            {
                Init();
            }

            return GetBundleForMeasure(
                _measures[DailyReporting.Current.Name],
                DailyReporting.Current.Name);
        }

        /// <summary>Gets bundle for measure.</summary>
        /// <param name="measure">The measure.</param>
        /// <param name="id">     The identifier.</param>
        /// <returns>The bundle for measure.</returns>
        private static Bundle GetBundleForMeasure(
            Measure measure,
            string id)
        {
            string bundleId = FhirIds.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirIds.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                measure,
                $"{FhirSystems.Internal}MeasureReport/{id}");

            return bundle;
        }
    }
}
