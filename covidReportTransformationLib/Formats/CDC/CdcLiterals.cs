// <copyright file="CdcLiterals.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace covidReportTransformationLib.Formats.CDC
{
    /// <summary>A cdc literals.</summary>
    public abstract class CdcLiterals
    {
        /// <summary>The cdc collection date.</summary>
        public const string CollectionDate = "collectionDate";

        /// <summary>The collection date title.</summary>
        public const string CollectionDateTitle = "Collection Date";

        /// <summary>Information describing the collection date.</summary>
        public const string CollectionDateDesc = "Collection date this report represents.";

        /// <summary>The CDC total beds.</summary>
        public const string TotalBeds = "numTotBeds";

        /// <summary>The total beds title.</summary>
        public const string TotalBedsTitle = "All Hospital Beds";

        /// <summary>Information describing the total beds.</summary>
        public const string TotalBedsDesc = "Total number of all Inpatient and outpatient beds, including all staffed, ICU, licensed, and overflow(surge) beds used for inpatients or outpatients.";

        /// <summary>The CDC inpatient beds.</summary>
        public const string InpatientBeds = "numbeds";

        /// <summary>The inpatient beds title.</summary>
        public const string InpatientBedsTitle = "Hospital Inpatient Beds";

        /// <summary>Information describing the inpatient beds.</summary>
        public const string InpatientBedsDesc = "Inpatient beds, including all staffed, licensed, and overflow(surge) beds used for inpatients.";

        /// <summary>The CDC inpatient bed occupancy.</summary>
        public const string InpatientBedOccupancy = "numBedsOcc";

        /// <summary>The inpatient bed occupancy title.</summary>
        public const string InpatientBedOccupancyTitle = "Hospital Inpatient Bed Occupancy";

        /// <summary>Information describing the inpatient bed occupancy.</summary>
        public const string InpatientBedOccupancyDesc = "Total number of staffed inpatient beds that are occupied.";

        /// <summary>The CDC icu beds.</summary>
        public const string IcuBeds = "numICUBeds";

        /// <summary>The icu beds title.</summary>
        public const string IcuBedsTitle = "Hospital ICU Beds";

        /// <summary>Information describing the icu beds.</summary>
        public const string IcuBedsDesc = "Total number of staffed inpatient intensive care unit (ICU) beds.";

        /// <summary>The CDC icu bed occupancy.</summary>
        public const string IcuBedOccupancy = "numICUBedsOcc";

        /// <summary>The icu bed occupancy title.</summary>
        public const string IcuBedOccupancyTitle = "Hospital ICU Bed Occupancy";

        /// <summary>Information describing the icu bed occupancy.</summary>
        public const string IcuBedOccupancyDesc = "Total number of staffed inpatient ICU beds that are occupied.";

        /// <summary>The CDC ventilators.</summary>
        public const string Ventilators = "numVent";

        /// <summary>The ventilators title.</summary>
        public const string VentilatorsTitle = "Mechanical Ventilators";

        /// <summary>Information describing the ventilators.</summary>
        public const string VentilatorsDesc = "Total number of ventilators available.";

        /// <summary>The cdc ventilators in use.</summary>
        public const string VentilatorsInUse = "numVentUse";

        /// <summary>The ventilators in use title.</summary>
        public const string VentilatorsInUseTitle = "Mechanical Ventilators In Use";

        /// <summary>Information describing the ventilators in use.</summary>
        public const string VentilatorsInUseDesc = "Total number of ventilators in use.";

        /// <summary>The cdc hospitalized patients.</summary>
        public const string HospitalizedPatients = "numC19HospPats";

        /// <summary>The hospitalized patients title.</summary>
        public const string HospitalizedPatientsTitle = "COVID-19 Patients Hospitalized";

        /// <summary>Information describing the hospitalized patients.</summary>
        public const string HospitalizedPatientsDesc = "Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.";

        /// <summary>The cdc ventilated patients.</summary>
        public const string VentilatedPatients = "numC19MechVentPats";

        /// <summary>The ventilated patients title.</summary>
        public const string VentilatedPatientsTitle = "COVID-19 Patients Hospitalized and Ventilated";

        /// <summary>Information describing the ventilated patients.</summary>
        public const string VentilatedPatientsDesc = "Patients hospitalized in an NHSN inpatient care location who have suspected or confirmed COVID - 19 and are on a mechanical ventilator.";

        /// <summary>The cdc hospital onset.</summary>
        public const string HospitalOnset = "numC19HOPats";

        /// <summary>The hospital onset title.</summary>
        public const string HospitalOnsetTitle = "COVID-19 Hospital Onset";

        /// <summary>Information describing the hospital onset.</summary>
        public const string HospitalOnsetDesc = "Patients hospitalized in an NHSN inpatient care location with onset of suspected or confirmed COVID - 19 14 or more days after hospitalization.";

        /// <summary>The cdc overflow patients.</summary>
        public const string AwaitingBeds = "numC19OverflowPats";

        /// <summary>The awaiting beds title.</summary>
        public const string AwaitingBedsTitle = "ED/Overflow";

        /// <summary>Information describing the awaiting beds.</summary>
        public const string AwaitingBedsDesc = "Patients with suspected or confirmed COVID-19 who are in the ED or any overflow location awaiting an inpatient bed.";

        /// <summary>The cdc awaiting ventilators.</summary>
        public const string AwaitingVentilators = "numC19OFMechVentPats";

        /// <summary>The awaiting ventilators title.</summary>
        public const string AwaitingVentilatorsTitle = "ED/Overflow and Ventilated";

        /// <summary>Information describing the awaiting ventilators.</summary>
        public const string AwaitingVentilatorsDesc = "Patients with suspected or confirmed COVID - 19 who are in the ED or any overflow location awaiting an inpatient bed and on a mechanical ventilator.";

        /// <summary>The cdc died.</summary>
        public const string Died = "numC19Died";

        /// <summary>The died title.</summary>
        public const string DiedTitle = "COVID-19 Patients Died";

        /// <summary>Information describing the died.</summary>
        public const string DiedDesc = "Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location.";
    }
}
