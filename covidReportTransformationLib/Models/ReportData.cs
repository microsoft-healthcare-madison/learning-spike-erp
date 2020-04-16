// <copyright file="ReportData.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace covidReportTransformationLib.Models
{
    /// <summary>An location data record.</summary>
    public class ReportData
    {
        private LocationInfo _reporter;
        private LocationInfo _coveredOrganization;
        private LocationInfo _coveredLocation;
        private int? _bedsTotal;
        private int? _bedsInpatientTotal;
        private int? _bedsInpatientInUse;
        private int? _bedsIcuTotal;
        private int? _bedsIcuInUse;
        private int? _ventilatorsTotal;
        private int? _ventilatorsInUse;
        private int? _patientsHospitalized;
        private int? _patientsC19Hospitalized;
        private int? _patientsC19Ventilated;
        private int? _c19TestsPerformedTotal;
        private int? _c19TestsPositiveTotal;
        private int? _c19TestsNegativeTotal;
        private int? _c19TestsPendingTotal;
        private int? _c19TestsPerformedToday;
        private int? _c19TestsPositiveToday;
        private int? _c19TestsNegativeToday;
        private int? _c19TestsPendingToday;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportData"/> class.
        /// </summary>
        public ReportData()
        {
            ReportDate = DateTime.Now;
            CollectionStartDate = DateTime.Now;
            CollectionEndDate = DateTime.Now;
        }

        /// <summary>Initializes a new instance of the <see cref="ReportData"/> class.</summary>
        /// <param name="reportDate">                   The report date.</param>
        /// <param name="collectionStartDate">          The collection start date.</param>
        /// <param name="collectionEndDate">            The collection end date.</param>
        /// <param name="reporter">                     The reporter information.</param>
        /// <param name="coveredOrganization">          The covered organization information.</param>
        /// <param name="coveredLocation">              The covered location information.</param>
        /// <param name="bedsTotal">                    The beds total.</param>
        /// <param name="bedsInpatientTotal">           The beds inpatient total.</param>
        /// <param name="bedsInpatientInUse">           The beds inpatient in use.</param>
        /// <param name="bedsIcuTotal">                 The beds icu total.</param>
        /// <param name="bedsIcuInUse">                 The beds icu in use.</param>
        /// <param name="ventilatorsTotal">             The ventilators total.</param>
        /// <param name="ventilatorsInUse">             The ventilators in use.</param>
        /// <param name="c19TestsPerformedTotal">       The c 19 tests performed total.</param>
        /// <param name="c19TestsPositiveTotal">        The c 19 tests positive total.</param>
        /// <param name="c19TestsNegativeTotal">        The c 19 tests negative total.</param>
        /// <param name="c19TestsPendingTotal">         The c 19 tests pending total.</param>
        /// <param name="c19TestsPerformedToday">       The c 19 tests performed today.</param>
        /// <param name="c19TestsPositiveToday">        The c 19 tests positive today.</param>
        /// <param name="c19TestsNegativeToday">        The c 19 tests negative today.</param>
        /// <param name="c19TestsPendingToday">         The c 19 tests pending today.</param>
        /// <param name="testsPerformedToday">          The tests performed today.</param>
        /// <param name="testsPerformedTotal">          The tests performed total.</param>
        /// <param name="testsResultedToday">           The tests resulted today.</param>
        /// <param name="testsResultedTotal">           The tests resulted total.</param>
        /// <param name="testsRejectedTotal">           The total number tests rejected.</param>
        /// <param name="patientsHospitalized">         The patients hospitalized.</param>
        /// <param name="patientsC19Hospitalized">      The patients c 19 hospitalized.</param>
        /// <param name="patientsC19Ventilated">        The patients c 19 on a mechanical ventilator.</param>
        /// <param name="patientsC19NeedBed">           The patients c 19 need bed.</param>
        /// <param name="patientsC19NeedVent">          The patients c 19 need vent.</param>
        /// <param name="patientsC19HospitalOnsetTotal">The patients c 19 hospital onset total.</param>
        /// <param name="patientsC19HospitalOnsetToday">The patients c 19 hospital onset today.</param>
        /// <param name="patientsC19DiedTotal">         The patients c 19 died total.</param>
        /// <param name="patientsC19DiedToday">         The patients c 19 died today.</param>
        [Newtonsoft.Json.JsonConstructor]
        public ReportData(
            DateTime? reportDate,
            DateTime collectionStartDate,
            DateTime collectionEndDate,
            LocationInfo reporter,
            LocationInfo coveredOrganization,
            LocationInfo coveredLocation,
            int? bedsTotal,
            int? bedsInpatientTotal,
            int? bedsInpatientInUse,
            int? bedsIcuTotal,
            int? bedsIcuInUse,
            int? ventilatorsTotal,
            int? ventilatorsInUse,
            int? c19TestsPerformedTotal,
            int? c19TestsPositiveTotal,
            int? c19TestsNegativeTotal,
            int? c19TestsPendingTotal,
            int? c19TestsPerformedToday,
            int? c19TestsPositiveToday,
            int? c19TestsNegativeToday,
            int? c19TestsPendingToday,
            int? testsPerformedToday,
            int? testsPerformedTotal,
            int? testsResultedToday,
            int? testsResultedTotal,
            int? testsRejectedTotal,
            int? patientsHospitalized,
            int? patientsC19Hospitalized,
            int? patientsC19Ventilated,
            int? patientsC19NeedBed,
            int? patientsC19NeedVent,
            int? patientsC19HospitalOnsetTotal,
            int? patientsC19HospitalOnsetToday,
            int? patientsC19DiedTotal,
            int? patientsC19DiedToday)
        {
            ReportDate = reportDate;
            CollectionStartDate = collectionStartDate;
            CollectionEndDate = collectionEndDate;
            _reporter = reporter;
            _coveredOrganization = coveredOrganization;
            _coveredLocation = coveredLocation;
            _bedsTotal = bedsTotal;
            _bedsInpatientTotal = bedsInpatientTotal;
            _bedsInpatientInUse = bedsInpatientInUse;
            _bedsIcuTotal = bedsIcuTotal;
            _bedsIcuInUse = bedsIcuInUse;
            _ventilatorsTotal = ventilatorsTotal;
            _ventilatorsInUse = ventilatorsInUse;
            _c19TestsPerformedTotal = c19TestsPerformedTotal;
            _c19TestsPositiveTotal = c19TestsPositiveTotal;
            _c19TestsNegativeTotal = c19TestsNegativeTotal;
            _c19TestsPendingTotal = c19TestsPendingTotal;
            _c19TestsPerformedToday = c19TestsPerformedToday;
            _c19TestsPositiveToday = c19TestsPositiveToday;
            _c19TestsNegativeToday = c19TestsNegativeToday;
            _c19TestsPendingToday = c19TestsPendingToday;
            TestsPerformedToday = testsPerformedToday;
            TestsPerformedTotal = testsPerformedTotal;
            TestsResultedToday = testsResultedToday;
            TestsResultedTotal = testsResultedTotal;
            TestsRejectedTotal = testsRejectedTotal;
            _patientsHospitalized = patientsHospitalized;
            _patientsC19Hospitalized = patientsC19Hospitalized;
            _patientsC19Ventilated = patientsC19Ventilated;
            PatientsC19NeedBed = patientsC19NeedBed;
            PatientsC19NeedVent = patientsC19NeedVent;
            PatientsC19HospitalOnsetTotal = patientsC19HospitalOnsetTotal;
            PatientsC19HospitalOnsetToday = patientsC19HospitalOnsetToday;
            PatientsC19DiedTotal = patientsC19DiedTotal;
            PatientsC19DiedToday = patientsC19DiedToday;
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

        /// <summary>Gets the reporter.</summary>
        /// <value>The reporter.</value>
        public LocationInfo Reporter => _reporter;

        /// <summary>Gets the covered organization.</summary>
        /// <value>The covered organization.</value>
        public LocationInfo CoveredOrganization => _coveredOrganization;

        /// <summary>Gets the covered location.</summary>
        /// <value>The covered location.</value>
        public LocationInfo CoveredLocation => _coveredLocation;

        /// <summary>Gets the beds total.</summary>
        /// <value>The beds total.</value>
        public int? BedsTotal => _bedsTotal;

        /// <summary>Gets the beds inpatient total.</summary>
        /// <value>The beds inpatient total.</value>
        public int? BedsInpatientTotal => _bedsInpatientTotal;

        /// <summary>Gets the beds inpatient in use.</summary>
        /// <value>The beds inpatient in use.</value>
        public int? BedsInpatientInUse => _bedsInpatientInUse;

        /// <summary>Gets the beds icu total.</summary>
        /// <value>The beds icu total.</value>
        public int? BedsIcuTotal => _bedsIcuTotal;

        /// <summary>Gets the beds icu in use.</summary>
        /// <value>The beds icu in use.</value>
        public int? BedsIcuInUse => _bedsIcuInUse;

        /// <summary>Gets the ventilators total.</summary>
        /// <value>The ventilators total.</value>
        public int? VentilatorsTotal => _ventilatorsTotal;

        /// <summary>Gets the ventilators in use.</summary>
        /// <value>The ventilators in use.</value>
        public int? VentilatorsInUse => _ventilatorsInUse;

        /// <summary>Gets the 19 tests performed total.</summary>
        /// <value>The c 19 tests performed total.</value>
        public int? C19TestsPerformedTotal => _c19TestsPerformedTotal;

        /// <summary>Gets the 19 tests positive total.</summary>
        /// <value>The c 19 tests positive total.</value>
        public int? C19TestsPositiveTotal => _c19TestsPositiveTotal;

        /// <summary>Gets the 19 tests negative total.</summary>
        /// <value>The c 19 tests negative total.</value>
        public int? C19TestsNegativeTotal => _c19TestsNegativeTotal;

        /// <summary>Gets the 19 tests pending total.</summary>
        /// <value>The c 19 tests pending total.</value>
        public int? C19TestsPendingTotal => _c19TestsPendingTotal;

        /// <summary>Gets the 19 tests performed today.</summary>
        /// <value>The c 19 tests performed today.</value>
        public int? C19TestsPerformedToday => _c19TestsPerformedToday;

        /// <summary>Gets the 19 tests positive today.</summary>
        /// <value>The c 19 tests positive today.</value>
        public int? C19TestsPositiveToday => _c19TestsPositiveToday;

        /// <summary>Gets the 19 tests negative today.</summary>
        /// <value>The c 19 tests negative today.</value>
        public int? C19TestsNegativeToday => _c19TestsNegativeToday;

        /// <summary>Gets the 19 tests pending today.</summary>
        /// <value>The c 19 tests pending today.</value>
        public int? C19TestsPendingToday => _c19TestsPendingToday;

        /// <summary>Gets the tests performed today.</summary>
        /// <value>The tests performed today.</value>
        public int? TestsPerformedToday { get; }

        /// <summary>Gets the tests performed total.</summary>
        /// <value>The tests performed total.</value>
        public int? TestsPerformedTotal { get; }

        /// <summary>Gets the tests resulted today.</summary>
        /// <value>The tests resulted today.</value>
        public int? TestsResultedToday { get; }

        /// <summary>Gets the tests resulted total.</summary>
        /// <value>The tests resulted total.</value>
        public int? TestsResultedTotal { get; }

        /// <summary>Gets the total number of tests rejected.</summary>
        /// <value>The total number tests rejected.</value>
        public int? TestsRejectedTotal { get; }

        /// <summary>Gets the patients hospitalized.</summary>
        /// <value>The patients hospitalized.</value>
        public int? PatientsHospitalized => _patientsHospitalized;

        /// <summary>Gets the patients c 19 hospitalized.</summary>
        /// <value>The patients c 19 hospitalized.</value>
        public int? PatientsC19Hospitalized => _patientsC19Hospitalized;

        /// <summary>Gets the patients c 19 on a mechanical ventilator.</summary>
        /// <value>The patients c 19 on a mechanical ventilator.</value>
        public int? PatientsC19Ventilated => _patientsC19Ventilated;

        /// <summary>Gets the patients c 19 need bed.</summary>
        /// <value>The patients c 19 need bed.</value>
        public int? PatientsC19NeedBed { get; }

        /// <summary>Gets the patients c 19 need vent.</summary>
        /// <value>The patients c 19 need vent.</value>
        public int? PatientsC19NeedVent { get; }

        /// <summary>Gets the patients c 19 hospital onset total.</summary>
        /// <value>The patients c 19 hospital onset total.</value>
        public int? PatientsC19HospitalOnsetTotal { get; }

        /// <summary>Gets the patients c 19 hospital onset today.</summary>
        /// <value>The patients c 19 hospital onset today.</value>
        public int? PatientsC19HospitalOnsetToday { get; }

        /// <summary>Gets the patients c 19 died total.</summary>
        /// <value>The patients c 19 died total.</value>
        public int? PatientsC19DiedTotal { get; }

        /// <summary>Gets the patients c 19 died today.</summary>
        /// <value>The patients c 19 died today.</value>
        public int? PatientsC19DiedToday { get; }

        /// <summary>Sets the locations.</summary>
        /// <param name="reporter">    The reporter information.</param>
        /// <param name="organization">The organization.</param>
        /// <param name="location">    The location.</param>
        public void SetLocations(
            LocationInfo reporter,
            LocationInfo organization,
            LocationInfo location)
        {
            _reporter = reporter;
            _coveredOrganization = organization;
            _coveredLocation = location;
        }

        /// <summary>Adds a c 19 tests today.</summary>
        /// <param name="performed">The performed.</param>
        /// <param name="positive"> The positive.</param>
        /// <param name="negative"> The negative.</param>
        public void AddC19TestsToday(int? performed, int? positive, int? negative)
        {
            if (_c19TestsPerformedToday == null)
            {
                _c19TestsPerformedToday = 0;
            }

            _c19TestsPerformedToday += performed ?? 0;

            if (_c19TestsPositiveToday == null)
            {
                _c19TestsPositiveToday = 0;
            }

            _c19TestsPositiveToday += positive ?? 0;

            if (_c19TestsNegativeToday == null)
            {
                _c19TestsNegativeToday = 0;
            }

            _c19TestsNegativeToday += negative ?? 0;
        }

        /// <summary>Adds a c 19 tests total.</summary>
        /// <param name="performed">The performed.</param>
        /// <param name="positive"> The positive.</param>
        /// <param name="negative"> The negative.</param>
        public void AddC19TestsTotal(int? performed, int? positive, int? negative)
        {
            if (_c19TestsPerformedTotal == null)
            {
                _c19TestsPerformedTotal = 0;
            }

            _c19TestsPerformedTotal += performed ?? 0;

            if (_c19TestsPositiveTotal == null)
            {
                _c19TestsPositiveTotal = 0;
            }

            _c19TestsPositiveTotal += positive ?? 0;

            if (_c19TestsNegativeTotal == null)
            {
                _c19TestsNegativeTotal = 0;
            }

            _c19TestsNegativeTotal += negative ?? 0;
        }


        /// <summary>Adds inpatient beds.</summary>
        /// <param name="total">Number of.</param>
        /// <param name="used"> The used.</param>
        public void AddInpatientBeds(int? total, int? used)
        {
            if (_bedsTotal == null)
            {
                _bedsTotal = 0;
            }

            _bedsTotal += total ?? 0;

            if (_bedsInpatientTotal == null)
            {
                _bedsInpatientTotal = 0;
            }

            _bedsInpatientTotal += total ?? 0;

            if (_bedsInpatientInUse == null)
            {
                _bedsInpatientInUse = 0;
            }

            _bedsInpatientInUse += used ?? 0;
        }

        /// <summary>Adds icu beds.</summary>
        /// <param name="total">Number of.</param>
        /// <param name="used"> The used.</param>
        public void AddIcuBeds(int? total, int? used)
        {
            if (_bedsTotal == null)
            {
                _bedsTotal = 0;
            }

            _bedsTotal += total ?? 0;

            if (_bedsIcuTotal == null)
            {
                _bedsIcuTotal = 0;
            }

            _bedsIcuTotal += total ?? 0;

            if (_bedsIcuInUse == null)
            {
                _bedsIcuInUse = 0;
            }

            _bedsIcuInUse += used ?? 0;
        }

        /// <summary>Adds ventilators.</summary>
        /// <param name="total">Number of.</param>
        /// <param name="used"> The used.</param>
        public void AddVentilators(int? total, int? used)
        {
            if (_ventilatorsTotal == null)
            {
                _ventilatorsTotal = 0;
            }

            _ventilatorsTotal += total ?? 0;

            if (_ventilatorsInUse == null)
            {
                _ventilatorsInUse = 0;
            }

            _ventilatorsInUse += used ?? 0;
        }

        /// <summary>Adds a hospitalized.</summary>
        /// <param name="total">        Number of.</param>
        /// <param name="c19Positive">  The 19 position.</param>
        /// <param name="c19Ventilated">The 19 vent.</param>
        public void AddHospitalized(int? total, int? c19Positive, int? c19Ventilated)
        {
            if (_patientsHospitalized == null)
            {
                _patientsHospitalized = 0;
            }

            _patientsHospitalized += total ?? 0;

            if (_patientsC19Hospitalized == null)
            {
                _patientsC19Hospitalized = 0;
            }

            _patientsC19Hospitalized += c19Positive ?? 0;

            if (_patientsC19Ventilated == null)
            {
                _patientsC19Ventilated = 0;
            }

            _patientsC19Ventilated += c19Ventilated ?? 0;
        }

        /// <summary>FHIR period.</summary>
        /// <returns>A Period.</returns>
        public Period FhirPeriod()
        {
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
