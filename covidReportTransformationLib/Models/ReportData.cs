// <copyright file="ReportData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using covidReportTransformationLib.Formats;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Models
{
    /// <summary>An location data record.</summary>
    public class ReportData
    {
        private Organization _reporter;
        private Organization _coveredOrganization;
        private Location _coveredLocation;

        private Dictionary<string, FieldValue> _values;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportData"/> class.
        /// </summary>
        public ReportData()
        {
            ReportDate = DateTime.Now;
            CollectionStartDate = DateTime.Now;
            CollectionEndDate = DateTime.Now;

            FhirDateTime start = new FhirDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            CollectionPeriod = new Period(start, start);

            _values = new Dictionary<string, FieldValue>();
        }

        /// <summary>Initializes a new instance of the <see cref="ReportData"/> class.</summary>
        /// <param name="reportDate">           The report date.</param>
        /// <param name="collectionStartDate">  The collection start date.</param>
        /// <param name="collectionEndDate">    The collection end date.</param>
        /// <param name="reportingOrganization">The reporting organization.</param>
        /// <param name="coveredOrganization">  The covered organization information.</param>
        /// <param name="coveredLocation">      The covered location information.</param>
        public ReportData(
            DateTime? reportDate,
            DateTime collectionStartDate,
            DateTime collectionEndDate,
            Organization reportingOrganization,
            Organization coveredOrganization,
            Location coveredLocation)
        {
            ReportDate = reportDate;
            CollectionStartDate = collectionStartDate;
            CollectionEndDate = collectionEndDate;

            FhirDateTime start = new FhirDateTime(collectionStartDate.Year, collectionStartDate.Month, collectionStartDate.Day);
            FhirDateTime end = new FhirDateTime(collectionEndDate.Year, collectionEndDate.Month, collectionEndDate.Day);
            CollectionPeriod = new Period(start, end);

            _reporter = reportingOrganization;
            _coveredOrganization = coveredOrganization;
            _coveredLocation = coveredLocation;
        }

        /// <summary>Initializes a new instance of the <see cref="ReportData"/> class.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="reportDate">           The report date.</param>
        /// <param name="collectionStartDate">  The collection start date.</param>
        /// <param name="collectionEndDate">    The collection end date.</param>
        /// <param name="collectionPeriod">     The collection period.</param>
        /// <param name="reportingOrganization">The reporting organization.</param>
        /// <param name="coveredOrganization">  The covered organization information.</param>
        /// <param name="coveredLocation">      The covered location information.</param>
        /// <param name="values">               The field values.</param>
        [Newtonsoft.Json.JsonConstructor]
        public ReportData(
            DateTime? reportDate,
            DateTime collectionStartDate,
            DateTime collectionEndDate,
            Organization reportingOrganization,
            Organization coveredOrganization,
            Location coveredLocation,
            Dictionary<string, dynamic> values)
            : this(
                reportDate,
                collectionStartDate,
                collectionEndDate,
                reportingOrganization,
                coveredOrganization,
                coveredLocation)
        {
            if (values == null)
            {
                _values = new Dictionary<string, FieldValue>();
            }
            else
            {
                _values = new Dictionary<string, FieldValue>();

                foreach (KeyValuePair<string, dynamic> kvp in values)
                {
                    if (kvp.Value == null)
                    {
                        _values.Add(kvp.Key, new FieldValue());
                        continue;
                    }

                    switch (kvp.Value)
                    {
                        case FieldValue valueField:
                            _values.Add(kvp.Key, valueField);
                            break;

                        case decimal valueDecimal:
                            _values.Add(kvp.Key, new FieldValue(valueDecimal));
                            break;

                        case int valueInt:
                            _values.Add(kvp.Key, new FieldValue(valueInt));
                            break;

                        case string valueString:
                            _values.Add(kvp.Key, new FieldValue(valueString));
                            break;

                        case Tuple<decimal, int, int> valueTupleScore:
                            _values.Add(
                                kvp.Key,
                                new FieldValue(
                                    valueTupleScore.Item1,
                                    valueTupleScore.Item2,
                                    valueTupleScore.Item3));
                            break;

                        case Tuple<int, int> valueTupleInts:
                            _values.Add(kvp.Key, new FieldValue(valueTupleInts.Item1, valueTupleInts.Item2));
                            break;

                        default:
                            throw new Exception($"Cannot process value: {kvp.Key}:{kvp.Value} ({kvp.Value.Type.Name})");
                    }
                }
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ReportData"/> class.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="period">      The collection period.</param>
        /// <param name="organization">The organization.</param>
        /// <param name="location">    The location.</param>
        /// <param name="values">      The field values.</param>
        public ReportData(
            Period period,
            Organization organization,
            Location location,
            Dictionary<string, FieldValue> values)
        {
            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            CollectionStartDate = DateTime.Parse(period.Start, CultureInfo.InvariantCulture);
            CollectionEndDate = DateTime.Parse(period.End, CultureInfo.InvariantCulture);
            ReportDate = CollectionEndDate;

            CollectionPeriod = period;

            _reporter = organization;
            _coveredOrganization = organization;
            _coveredLocation = location;

            _values = values;
        }

        /// <summary>Initializes a new instance of the <see cref="ReportData"/> class.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="dateTime">    The collection period.</param>
        /// <param name="organization">The organization.</param>
        /// <param name="location">    The location.</param>
        /// <param name="values">      The field values.</param>
        public ReportData(
            DateTime dateTime,
            Organization organization,
            Location location,
            Dictionary<string, FieldValue> values)
        {
            DateTime dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);

            CollectionStartDate = dt;
            CollectionEndDate = dt;
            ReportDate = dt;

            FhirDateTime fhirDate = new FhirDateTime(dt.Year, dt.Month, dt.Day);
            CollectionPeriod = new Period(fhirDate, fhirDate);

            _reporter = organization;
            _coveredOrganization = organization;
            _coveredLocation = location;

            _values = values;
        }

        /// <summary>Gets the report date.</summary>
        /// <value>The report date.</value>
        public DateTime? ReportDate { get; }

        /// <summary>Gets the collection start date.</summary>
        /// <value>The collection start date.</value>
        public DateTime CollectionStartDate { get; }

        /// <summary>Gets the collection end date.</summary>
        /// <value>The collection end date.</value>
        public DateTime CollectionEndDate { get; }

        /// <summary>Gets the collection period.</summary>
        /// <value>The collection period.</value>
        public Period CollectionPeriod { get; }

        /// <summary>Gets the reporter.</summary>
        /// <value>The reporter.</value>
        public Organization Reporter => _reporter;

        /// <summary>Gets the covered organization.</summary>
        /// <value>The covered organization.</value>
        public Organization CoveredOrganization => _coveredOrganization;

        /// <summary>Gets the covered location.</summary>
        /// <value>The covered location.</value>
        public Location CoveredLocation => _coveredLocation;

        /// <summary>Gets the field values.</summary>
        /// <value>The field values.</value>
        public Dictionary<string, FieldValue> Values => _values;

        /// <summary>FHIR period.</summary>
        /// <returns>A Period.</returns>
        public Period FhirPeriod()
        {
            if (CollectionPeriod != null)
            {
                return CollectionPeriod;
            }

            if ((CollectionStartDate == null) && (CollectionEndDate == null))
            {
                return null;
            }

            FhirDateTime start = new FhirDateTime(
                CollectionStartDate.Year,
                CollectionStartDate.Month,
                CollectionStartDate.Day);

            FhirDateTime end = new FhirDateTime(
                CollectionEndDate.Year,
                CollectionEndDate.Month,
                CollectionEndDate.Day);

            return new Period(start, end);
        }
    }
}
