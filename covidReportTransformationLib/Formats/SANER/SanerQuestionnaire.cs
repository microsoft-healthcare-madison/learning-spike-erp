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

            List<IReportingFormat> formats = FormatHelper.GetFormatList();

            foreach (IReportingFormat format in formats)
            {
                if (format.QuestionnaireSections != null)
                {
                    _questionnaires.Add(format.Name, BuildQuestionnaire(format));
                }
            }

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
                Meta = new Meta()
                {
                    Profile = new string[]
                    {
                        FhirSystems.Questionnaire,
                    },
                },
                Id = format.Name,
                Name = format.Name,
                Url = $"{CanonicalUrl}/{format.Name}",
                Version = QuestionnaireVersion,
                Title = format.Title,
                Description = new Markdown(format.Description),
                Status = PublicationStatus.Draft,
                Date = PublicationDate,
                Publisher = Publisher,
                Jurisdiction = new List<CodeableConcept>()
                {
                    FhirTriplet.UnitedStates.GetConcept(),
                },
                UseContext = new List<UsageContext>()
                {
                    new UsageContext()
                    {
                        Code = FhirTriplet.GetCode(FhirSystems.UsageContextType, CommonLiterals.ContextFocus),
                        Value = FhirTriplet.SctCovid.GetConcept(),
                    },
                },
                Item = new List<Questionnaire.ItemComponent>(),
            };

            int sectionNumber = -1;
            int itemNumber = 0;

            foreach (QuestionnaireSection questionnaireSection in format.QuestionnaireSections)
            {
                sectionNumber++;
                itemNumber = 0;

                Questionnaire.ItemComponent section = new Questionnaire.ItemComponent()
                {
                    LinkId = $"section_{sectionNumber}",
                    Type = Questionnaire.QuestionnaireItemType.Group,
                    Item = new List<Questionnaire.ItemComponent>(),
                    Repeats = false,
                };

                section.AddExtension(
                    "http://fhir.org/guides/argonaut/questionnaire/StructureDefinition/extension-itemOrder",
                    new FhirDecimal(sectionNumber));

                if (format.Fields.ContainsKey(questionnaireSection.Title))
                {
                    section.Text = $"{format.Fields[questionnaireSection.Title].Title}: {format.Fields[questionnaireSection.Title].Description}";
                }
                else
                {
                    section.Text = questionnaireSection.Title;
                }

                foreach (QuestionnaireQuestion question in questionnaireSection.Fields)
                {
                    Questionnaire.ItemComponent component = ComponentFromQuestion(
                        format,
                        question,
                        ref itemNumber);

                    if (component == null)
                    {
                        continue;
                    }

                    section.Item.Add(component);
                }

                questionnaire.Item.Add(section);
            }

            return questionnaire;
        }

        /// <summary>Component from question.</summary>
        /// <param name="format">   Describes the format to use.</param>
        /// <param name="question"> The question.</param>
        /// <param name="itemOrder">[in,out] The item order.</param>
        /// <returns>A Questionnaire.ItemComponent.</returns>
        private static Questionnaire.ItemComponent ComponentFromQuestion(
            IReportingFormat format,
            QuestionnaireQuestion question,
            ref int itemOrder)
        {
            if (!format.Fields.ContainsKey(question.ValueFieldName))
            {
                return null;
            }

            FormatField valueField = format.Fields[question.ValueFieldName];
            FormatField displayField;

            if (string.IsNullOrEmpty(question.DisplayFieldName) ||
                (!format.Fields.ContainsKey(question.DisplayFieldName)))
            {
                displayField = valueField;
            }
            else
            {
                displayField = format.Fields[question.DisplayFieldName];
            }

            Questionnaire.ItemComponent component = new Questionnaire.ItemComponent()
            {
                LinkId = valueField.Name,
                Code = new List<Coding>()
                {
                    new Coding($"{CanonicalUrl}/{format.Name}", valueField.Name),
                },
                Required = valueField.IsRequired == true,
                Repeats = false,
            };

            component.AddExtension(
                "http://fhir.org/guides/argonaut/questionnaire/StructureDefinition/extension-itemOrder",
                new FhirDecimal(itemOrder++));

            if (question.UseTitleOnly)
            {
                component.Text = $"{displayField.Title}";
            }
            else
            {
                component.Text = $"{displayField.Title}: {displayField.Description}";
            }

            switch (valueField.Type)
            {
                case FormatField.FieldType.Date:
                    component.Type = Questionnaire.QuestionnaireItemType.Date;
                    break;

                case FormatField.FieldType.Count:
                    component.Type = Questionnaire.QuestionnaireItemType.Integer;
                    break;

                case FormatField.FieldType.Percentage:
                    component.Type = Questionnaire.QuestionnaireItemType.Decimal;
                    break;

                case FormatField.FieldType.Boolean:
                    component.Type = Questionnaire.QuestionnaireItemType.Boolean;
                    break;

                case FormatField.FieldType.Choice:
                    component.Type = Questionnaire.QuestionnaireItemType.Choice;
                    component.AnswerOption = new List<Questionnaire.AnswerOptionComponent>();

                    int optionOrder = 0;
                    foreach (FormatFieldOption option in valueField.Options)
                    {
                        Element element = new FhirString(option.Text);

                        element.AddExtension(
                            "http://hl7.org/fhir/StructureDefinition/questionnaire-optionExclusive",
                            new FhirBoolean(option.IsExclusive),
                            true);

                        element.AddExtension(
                            "http://fhir.org/guides/argonaut/questionnaire/StructureDefinition/extension-itemOrder",
                            new FhirDecimal(optionOrder++));

                        component.AnswerOption.Add(new Questionnaire.AnswerOptionComponent()
                        {
                            Value = element,
                        });
                    }

                    break;

                case FormatField.FieldType.Text:
                    component.Type = Questionnaire.QuestionnaireItemType.Text;
                    break;

                case FormatField.FieldType.ShortString:
                    component.Type = Questionnaire.QuestionnaireItemType.String;
                    break;

                case FormatField.FieldType.Display:
                default:
                    component.Type = Questionnaire.QuestionnaireItemType.Display;
                    break;
            }

            return component;
        }

        /// <summary>Gets a questionnaire.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="format">Describes the format to use.</param>
        /// <returns>The questionnaire.</returns>
        public static Questionnaire GetQuestionnaire(IReportingFormat format)
        {
            if (!_initialized)
            {
                Init();
            }

            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (!_questionnaires.ContainsKey(format.Name))
            {
                return null;
            }

            return _questionnaires[format.Name];
        }

        /// <summary>Gets a bundle.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="format">Describes the format to use.</param>
        /// <returns>The bundle.</returns>
        public static Bundle GetBundle(IReportingFormat format)
        {
            if (!_initialized)
            {
                Init();
            }

            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (!_questionnaires.ContainsKey(format.Name))
            {
                return null;
            }

            return GetBundleForQuestionnaire(
                _questionnaires[format.Name],
                format.Name);
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
                Meta = new Meta()
                {
                    Profile = new string[]
                    {
                        FhirSystems.Bundle,
                    },
                },
                Id = bundleId,
                Identifier = FhirIds.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                questionnaire,
                $"{FhirSystems.Internal}Questionnaire/{id}");

            return bundle;
        }
    }
}
