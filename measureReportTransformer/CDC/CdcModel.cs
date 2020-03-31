// <copyright file="CdcModel.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace measureReportTransformer.CDC
{
    /// <summary>A data Model for the cdc.</summary>
    public class CdcModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdcModel"/> class.
        /// </summary>
        /// <param name="collectionDate">       The collection date.</param>
        /// <param name="totalBeds">            The total number of beds.</param>
        /// <param name="inpatientBeds">        The inpatient beds.</param>
        /// <param name="inpatientBedOccupancy">The inpatient bed occupancy.</param>
        /// <param name="icuBeds">              The icu beds.</param>
        /// <param name="icuBedOccupancy">      The icu bed occupancy.</param>
        /// <param name="ventilators">          The ventilators.</param>
        /// <param name="ventilatorsInUse">     The ventilators in use.</param>
        /// <param name="hospitalizedPatients"> The hospitalized patients.</param>
        /// <param name="ventilatedPatients">   The ventilated patients.</param>
        /// <param name="hospitalOnset">        The hospital onset.</param>
        /// <param name="awaitingBeds">         The awaiting beds.</param>
        /// <param name="awaitingVentilators">  The awaiting ventilators.</param>
        /// <param name="died">                 The died.</param>
        public CdcModel(
            string collectionDate,
            int? totalBeds,
            int inpatientBeds,
            int? inpatientBedOccupancy,
            int? icuBeds,
            int? icuBedOccupancy,
            int? ventilators,
            int? ventilatorsInUse,
            int? hospitalizedPatients,
            int? ventilatedPatients,
            int? hospitalOnset,
            int? awaitingBeds,
            int? awaitingVentilators,
            int? died)
        {
            CollectionDate = collectionDate;
            TotalBeds = totalBeds;
            InpatientBeds = inpatientBeds;
            InpatientBedOccupancy = inpatientBedOccupancy;
            IcuBeds = icuBeds;
            IcuBedOccupancy = icuBedOccupancy;
            Ventilators = ventilators;
            VentilatorsInUse = ventilatorsInUse;
            HospitalizedPatients = hospitalizedPatients;
            VentilatedPatients = ventilatedPatients;
            HospitalOnset = hospitalOnset;
            AwaitingBeds = awaitingBeds;
            AwaitingVentilators = awaitingVentilators;
            Died = died;
        }

        /// <summary>Gets the collection date.</summary>
        /// <value>The collection date.</value>
        [CsvHelper.Configuration.Attributes.Index(0)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCCollectionDate)]
        public string CollectionDate { get; }

        /// <summary>Gets the total number of beds.</summary>
        /// <value>The total number of beds.</value>
        [CsvHelper.Configuration.Attributes.Index(1)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCTotalBeds)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? TotalBeds { get; }

        /// <summary>Gets the inpatient beds.</summary>
        /// <value>The inpatient beds.</value>
        [CsvHelper.Configuration.Attributes.Index(2)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCInpatientBeds)]
        public int InpatientBeds { get; }

        /// <summary>Gets the inpatient bed occupancy.</summary>
        /// <value>The inpatient bed occupancy.</value>
        [CsvHelper.Configuration.Attributes.Index(3)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCInpatientBedOccupancy)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? InpatientBedOccupancy { get; }

        /// <summary>Gets the icu beds.</summary>
        /// <value>The icu beds.</value>
        [CsvHelper.Configuration.Attributes.Index(4)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCIcuBeds)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? IcuBeds { get; }

        /// <summary>Gets the icu bed occupancy.</summary>
        /// <value>The icu bed occupancy.</value>
        [CsvHelper.Configuration.Attributes.Index(5)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCIcuBedOccupancy)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? IcuBedOccupancy { get; }

        /// <summary>Gets the ventilators.</summary>
        /// <value>The ventilators.</value>
        [CsvHelper.Configuration.Attributes.Index(6)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCVentilators)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? Ventilators { get; }

        /// <summary>Gets the ventilators in use.</summary>
        /// <value>The ventilators in use.</value>
        [CsvHelper.Configuration.Attributes.Index(7)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCVentilatorsInUse)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? VentilatorsInUse { get; }

        /// <summary>Gets the hospitalized patients.</summary>
        /// <value>The hospitalized patients.</value>
        [CsvHelper.Configuration.Attributes.Index(8)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCHospitalizedPatients)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? HospitalizedPatients { get; }

        /// <summary>Gets the ventilated patients.</summary>
        /// <value>The ventilated patients.</value>
        [CsvHelper.Configuration.Attributes.Index(9)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCVentilatedPatients)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? VentilatedPatients { get; }

        /// <summary>Gets the hospital onset.</summary>
        /// <value>The hospital onset.</value>
        [CsvHelper.Configuration.Attributes.Index(10)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCHospitalOnset)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? HospitalOnset { get; }

        /// <summary>Gets the awaiting beds.</summary>
        /// <value>The awaiting beds.</value>
        [CsvHelper.Configuration.Attributes.Index(11)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCAwaitingBeds)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? AwaitingBeds { get; }

        /// <summary>Gets the awaiting ventilators.</summary>
        /// <value>The awaiting ventilators.</value>
        [CsvHelper.Configuration.Attributes.Index(12)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCAwaitingVentilators)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? AwaitingVentilators { get; }

        /// <summary>Gets the died.</summary>
        /// <value>The died.</value>
        [CsvHelper.Configuration.Attributes.Index(13)]
        [CsvHelper.Configuration.Attributes.Name(CdcLiterals.CDCDied)]
        [CsvHelper.Configuration.Attributes.Optional]
        public int? Died { get; }
    }
}
