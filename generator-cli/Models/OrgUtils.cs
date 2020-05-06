// <copyright file="OrgUtils.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using covidReportTransformationLib.Formats;
using covidReportTransformationLib.Formats.CDC;
using covidReportTransformationLib.Formats.FEMA;

namespace generator_cli.Models
{
    /// <summary>An organization utilities.</summary>
    public abstract class OrgUtils
    {
        /// <summary>Builds field dictionary.</summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="deviceData"> Information describing the device.</param>
        /// <param name="patientData">Information describing the patient.</param>
        /// <param name="testData">   Information describing the test.</param>
        /// <returns>A Dictionary&lt;string,FieldValue&gt;</returns>
        public static Dictionary<string, FieldValue> BuildFieldDict(
            OrgDeviceData deviceData,
            OrgPatientData patientData,
            OrgTestData testData)
        {
            if (deviceData == null)
            {
                throw new ArgumentNullException(nameof(deviceData));
            }

            if (patientData == null)
            {
                throw new ArgumentNullException(nameof(patientData));
            }

            if (testData == null)
            {
                throw new ArgumentNullException(nameof(testData));
            }

            Dictionary<string, FieldValue> fieldDict = new Dictionary<string, FieldValue>();

            ComputeCdcPatientImpactScores(
                fieldDict,
                deviceData,
                patientData);

            ComputeFemaDailyReportingScores(
                fieldDict,
                testData);

            return fieldDict;
        }

        /// <summary>Builds score dictionaries.</summary>
        /// <param name="values">     The values.</param>
        /// <param name="deviceData"> Information describing the device.</param>
        /// <param name="patientData">Information describing the patient.</param>
        private static void ComputeCdcPatientImpactScores(
            Dictionary<string, FieldValue> values,
            OrgDeviceData deviceData,
            OrgPatientData patientData)
        {
            int val;

            values.Add(AcutePatientImpact.TotalBeds, new FieldValue(deviceData.TotalBeds));

            values.Add(AcutePatientImpact.InpatientBeds, new FieldValue(deviceData.Inpatient));

            val = Math.Min(deviceData.Inpatient, patientData.Total);
            values.Add(AcutePatientImpact.InpatientBedOccupancy, new FieldValue(val));

            values.Add(AcutePatientImpact.IcuBeds, new FieldValue(deviceData.ICU));

            val = Math.Min(deviceData.ICU, patientData.NegativeNeedIcu + patientData.PositiveNeedIcu);
            values.Add(AcutePatientImpact.IcuBedOccupancy, new FieldValue(val));

            values.Add(AcutePatientImpact.Ventilators, new FieldValue(deviceData.Ventilators));

            val = Math.Min(deviceData.Ventilators, patientData.NegativeNeedVent + patientData.PositiveNeedVent);
            values.Add(AcutePatientImpact.VentilatorsInUse, new FieldValue(val));

            val = Math.Min(deviceData.TotalBeds - patientData.Negative, patientData.Positive);
            values.Add(AcutePatientImpact.HospitalizedPatients, new FieldValue(val));

            val = Math.Min(deviceData.Ventilators - patientData.NegativeNeedVent, patientData.PositiveNeedVent);
            values.Add(AcutePatientImpact.VentilatedPatients, new FieldValue(val));

            values.Add(AcutePatientImpact.HospitalOnset, new FieldValue(patientData.OnsetInCare));

            val = Math.Max(0, patientData.Positive - (deviceData.TotalBeds - patientData.Negative));
            values.Add(AcutePatientImpact.AwaitingBeds, new FieldValue(val));

            val = Math.Max(0, patientData.PositiveNeedVent - (deviceData.Ventilators - patientData.NegativeNeedVent));
            values.Add(AcutePatientImpact.AwaitingVentilators, new FieldValue(val));

            values.Add(AcutePatientImpact.Died, new FieldValue(patientData.Died));
        }

        /// <summary>Calculates the fema scores.</summary>
        /// <param name="values">  The values.</param>
        /// <param name="testData">Information describing the test.</param>
        private static void ComputeFemaDailyReportingScores(
            Dictionary<string, FieldValue> values,
            OrgTestData testData)
        {
            decimal val;

            val = testData.PerformedToday * 2;
            values.Add(DailyReporting.TestsOrderedToday, new FieldValue(val));

            val = testData.Performed * 2;
            values.Add(DailyReporting.TestsOrderedTotal, new FieldValue(val));

            val = (testData.Positive + testData.Negative) * 2;
            values.Add(DailyReporting.TestsWithResultsToday, new FieldValue(val));

            values.Add(DailyReporting.SpecimensRejectedTotal, new FieldValue(testData.Rejected));

            val = (testData.Performed * 2) - testData.Rejected - testData.Pending;
            values.Add(DailyReporting.TestsCompletedTotal, new FieldValue(val));

            values.Add(DailyReporting.PositiveC19Today, new FieldValue(testData.PositiveToday));

            values.Add(DailyReporting.PositiveC19Total, new FieldValue(testData.Positive));

            values.Add(
                DailyReporting.PercentC19PositiveToday,
                new FieldValue(testData.PositiveToday, testData.PositiveToday * 2));

            values.Add(
                DailyReporting.PercentC19PositiveTotal,
                new FieldValue(testData.Positive, testData.Positive * 2));
        }
    }
}
