// <copyright file="SanerQuestionnaire.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Formats.CDC;
using covidReportTransformationLib.Formats.FEMA;
using covidReportTransformationLib.Utils;
using Hl7.Fhir.Model;
using static covidReportTransformationLib.Formats.SANER.SanerCommon;

namespace covidReportTransformationLib.Formats.SANER
{
    /// <summary>A SANER questionnaire generator.</summary>
    public static class SanerQuestionnaire
    {
        /// <summary>True once initialization is complete.</summary>
        private static bool _initialized = false;

        /// <summary>The questionnaires.</summary>
        private static readonly Dictionary<string, Questionnaire> _questionnaires = new Dictionary<string, Questionnaire>();

        /// <summary>Initializes this object.</summary>
        public static void Init()
        {
            if (_initialized)
            {
                return;
            }

            _questionnaires.Add(PatientImpact.Current.Name, BuildQuestionnaire(PatientImpact.Current));
            _questionnaires.Add(HealthcareWorker.Current.Name, BuildQuestionnaire(HealthcareWorker.Current));
            _questionnaires.Add(DailyReporting.Current.Name, BuildQuestionnaire(DailyReporting.Current));

            _initialized = true;
        }

        /// <summary>Builds a questionnaire.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="format">Describes the format to use.</param>
        /// <returns>A Questionnaire.</returns>
        private static Questionnaire BuildQuestionnaire(
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

            if ((format.Fields == null) || (format.Fields.Count == 0))
            {
                throw new ArgumentNullException(nameof(format), $"Invalid IReportingFormat.Fields: {format.Fields}");
            }

            if ((format.QuestionnaireSections == null) || (format.QuestionnaireSections.Count == 0))
            {
                throw new ArgumentNullException(nameof(format), $"Invalid IReportingFormat.QuestionnaireSections: {format.QuestionnaireSections}");
            }

            Questionnaire questionnaire = new Questionnaire()
            {
                Id = format.Name,
                Name = format.Name,
                Url = $"{CanonicalUrl}/{format.Name}",
                Version = MeasureVersion,
                Title = format.Title,
                Description = new Markdown(format.Description),
                Status = PublicationStatus.Draft,
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
                Item = new List<Questionnaire.ItemComponent>(),
            };

            int sectionNumber = 0;

            foreach (QuestionnaireSection questionnaireSection in format.QuestionnaireSections)
            {
                Questionnaire.ItemComponent section = new Questionnaire.ItemComponent()
                {
                    LinkId = $"section_{sectionNumber++}",
                    Text = questionnaireSection.Title,
                    Type = Questionnaire.QuestionnaireItemType.Group,
                    Item = new List<Questionnaire.ItemComponent>(),
                };

                foreach (string fieldName in questionnaireSection.Fields)
                {
                    if (!format.Fields.ContainsKey(fieldName))
                    {
                        continue;
                    }

                    FormatField field = format.Fields[fieldName];

                    Questionnaire.ItemComponent question = new Questionnaire.ItemComponent()
                    {
                        LinkId = field.Name,
                        Text = $"{field.Title}: {field.Description}",
                        Code = new List<Coding>()
                        {
                            new Coding($"{CanonicalUrl}/{format.Name}", field.Name),
                        },
                        Required = field.IsRequired == true,
                    };

                    switch (field.Type)
                    {
                        case FormatField.FieldType.Date:
                            question.Type = Questionnaire.QuestionnaireItemType.Date;
                            break;
                        case FormatField.FieldType.Count:
                            question.Type = Questionnaire.QuestionnaireItemType.Integer;
                            break;
                        case FormatField.FieldType.Percentage:
                            question.Type = Questionnaire.QuestionnaireItemType.Decimal;
                            break;
                        case FormatField.FieldType.Boolean:
                            question.Type = Questionnaire.QuestionnaireItemType.Boolean;
                            break;
                        case FormatField.FieldType.Choice:
                            question.Type = Questionnaire.QuestionnaireItemType.Choice;
                            question.AnswerOption = new List<Questionnaire.AnswerOptionComponent>();
                            foreach (string choice in field.Choices)
                            {
                                question.AnswerOption.Add(new Questionnaire.AnswerOptionComponent()
                                {
                                    Value = new FhirString(choice),
                                });
                            }

                            break;
                        case FormatField.FieldType.Text:
                            question.Type = Questionnaire.QuestionnaireItemType.Text;
                            break;
                        default:
                            question.Type = Questionnaire.QuestionnaireItemType.Display;
                            break;
                    }

                    section.Item.Add(question);
                }

                questionnaire.Item.Add(section);
            }

            return questionnaire;
        }

        /// <summary>Cdc patient impact questionnaire.</summary>
        /// <returns>A Questionnaire.</returns>
        public static Questionnaire CDCPatientImpactQuestionnaire()
        {
            if (!_initialized)
            {
                Init();
            }

            return _questionnaires[PatientImpact.Current.Name];
        }

        /// <summary>Cdc patient impact bundle.</summary>
        /// <returns>A Bundle.</returns>
        public static Bundle CDCPatientImpactBundle()
        {
            if (!_initialized)
            {
                Init();
            }

            return GetBundleForQuestionnaire(
                _questionnaires[PatientImpact.Current.Name],
                PatientImpact.Current.Name);
        }

        /// <summary>Cdc healthcare worker bundle.</summary>
        /// <returns>A Bundle.</returns>
        public static Bundle CDCHealthcareWorkerBundle()
        {
            if (!_initialized)
            {
                Init();
            }

            return GetBundleForQuestionnaire(
                _questionnaires[HealthcareWorker.Current.Name],
                HealthcareWorker.Current.Name);
        }

        /// <summary>Fema daily questionnaire.</summary>
        /// <returns>A Questionnaire.</returns>
        public static Questionnaire FEMADailyQuestionnaire()
        {
            if (!_initialized)
            {
                Init();
            }

            return _questionnaires[DailyReporting.Current.Name];
        }

        /// <summary>Fema daily bundle.</summary>
        /// <returns>A Bundle.</returns>
        public static Bundle FEMADailyBundle()
        {
            if (!_initialized)
            {
                Init();
            }

            return GetBundleForQuestionnaire(
                _questionnaires[DailyReporting.Current.Name],
                DailyReporting.Current.Name);
        }

        /// <summary>Gets bundle for questionnaire.</summary>
        /// <param name="questionnaire">The questionnaire.</param>
        /// <param name="id">           The identifier.</param>
        /// <returns>The bundle for questionnaire.</returns>
        private static Bundle GetBundleForQuestionnaire(
            Questionnaire questionnaire,
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
                questionnaire,
                $"{FhirSystems.Internal}Questionnaire/{id}");

            return bundle;
        }
    }
}
