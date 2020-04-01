// <copyright file="PlottingModel.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;

namespace measureReportTransformer.Plotting
{
    /// <summary>A data Model for the plotting.</summary>
    public class PlottingModel
    {
        /// <summary>Initializes a new instance of the <see cref="PlottingModel"/> class.</summary>
        /// <param name="latitude">             The latitude.</param>
        /// <param name="longitude">            The longitude.</param>
        /// <param name="reportDate">           The report date.</param>
        /// <param name="totalBeds">            The total number of beds.</param>
        /// <param name="inpatientBeds">        The inpatient beds.</param>
        /// <param name="inpatientBedOccupancy">The inpatient bed occupancy.</param>
        /// <param name="hospitalizedPatients"> The hospitalized patients.</param>
        public PlottingModel(
            double latitude,
            double longitude,
            string reportDate,
            int totalBeds,
            int inpatientBeds,
            int inpatientBedOccupancy,
            int hospitalizedPatients)
        {
            Latitude = latitude;
            Longitude = longitude;
            ReportDate = reportDate;
            TotalBeds = totalBeds;
            InpatientBeds = inpatientBeds;
            InpatientBedOccupancy = inpatientBedOccupancy;
            HospitalizedPatients = hospitalizedPatients;
        }

        /// <summary>Gets the latitude.</summary>
        /// <value>The latitude.</value>
        [CsvHelper.Configuration.Attributes.Index(0)]
        [CsvHelper.Configuration.Attributes.Name("latitude")]
        public double Latitude { get; }

        /// <summary>Gets the longitude.</summary>
        /// <value>The longitude.</value>
        [CsvHelper.Configuration.Attributes.Index(1)]
        [CsvHelper.Configuration.Attributes.Name("longitude")]
        public double Longitude { get; }

        /// <summary>Gets the report date.</summary>
        /// <value>The report date.</value>
        [CsvHelper.Configuration.Attributes.Index(2)]
        [CsvHelper.Configuration.Attributes.Name("reportDate")]
        public string ReportDate { get; }

        /// <summary>Gets the total number of beds.</summary>
        /// <value>The total number of beds.</value>
        [CsvHelper.Configuration.Attributes.Index(3)]
        [CsvHelper.Configuration.Attributes.Name("totalBeds")]
        public int TotalBeds { get; }

        /// <summary>Gets the inpatient beds.</summary>
        /// <value>The inpatient beds.</value>
        [CsvHelper.Configuration.Attributes.Index(4)]
        [CsvHelper.Configuration.Attributes.Name("inpatientBeds")]
        public int InpatientBeds { get; }

        /// <summary>Gets the inpatient bed occupancy.</summary>
        /// <value>The inpatient bed occupancy.</value>
        [CsvHelper.Configuration.Attributes.Index(5)]
        [CsvHelper.Configuration.Attributes.Name("inpatientBedsOccupied")]
        public int InpatientBedOccupancy { get; }

        /// <summary>Gets the hospitalized patients.</summary>
        /// <value>The hospitalized patients.</value>
        [CsvHelper.Configuration.Attributes.Index(6)]
        [CsvHelper.Configuration.Attributes.Name("hospitalizedPatients")]
        public int HospitalizedPatients { get; }
    }
}
