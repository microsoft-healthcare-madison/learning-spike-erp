// <copyright file="Program.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using covidReportTransformationLib.Formats.CDC;
using covidReportTransformationLib.Formats.FEMA;
using covidReportTransformationLib.Formats.MicrosoftECR;
using covidReportTransformationLib.Formats.SANER;
using covidReportTransformationLib.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;

namespace conversion_cli
{
    /// <summary>A program.</summary>
    public static class Program
    {
        private static Hl7.Fhir.Serialization.FhirJsonSerializer _jsonSerializer = null;

        /// <summary>Main entry-point for this application.</summary>
        /// <param name="sourceUrl">          URL of the server we are accessing.</param>
        /// <param name="sourceToken">        Access token for the server.</param>
        /// <param name="sourceTokenFile">    File containing an access token.</param>
        /// <param name="sourceType">         Type of server being accessed: 'FHIR', 'MS-ECR'.</param>
        /// <param name="outputDirectory">    Directory to use for export files.</param>
        /// <param name="generateCdcCsv">     True to generate CDC CSV files.</param>
        /// <param name="generateFemaXlsx">   True to generate FEMA XLSX files.</param>
        /// <param name="generateSanerBundle">True to generate FHIR SANER bundles.</param>
        public static void Main(
            Uri sourceUrl,
            string sourceToken,
            string sourceTokenFile,
            string sourceType,
            string outputDirectory,
            bool generateCdcCsv,
            bool generateFemaXlsx,
            bool generateSanerBundle)
        {
            if (sourceUrl == null)
            {
                throw new ArgumentNullException(nameof(sourceUrl));
            }

            if (string.IsNullOrEmpty(sourceType))
            {
                throw new ArgumentNullException(nameof(sourceType));
            }

            if (string.IsNullOrEmpty(outputDirectory))
            {
                throw new ArgumentNullException(nameof(outputDirectory));
            }

            string authToken = sourceToken;

            if (string.IsNullOrEmpty(authToken) &&
                (!string.IsNullOrEmpty(sourceTokenFile)) &&
                File.Exists(sourceTokenFile))
            {
                authToken = File.ReadAllText(sourceTokenFile);
            }

            Hl7.Fhir.Serialization.SerializerSettings settings = new Hl7.Fhir.Serialization.SerializerSettings()
            {
                Pretty = true,
            };

            _jsonSerializer = new Hl7.Fhir.Serialization.FhirJsonSerializer(settings);

            sourceType = sourceType.ToUpperInvariant();

            switch (sourceType)
            {
                case "MS-ECR":
                case "MSECR":

                    GetDataFromMsEcr(
                        sourceUrl,
                        authToken,
                        outputDirectory,
                        generateCdcCsv,
                        generateFemaXlsx,
                        generateSanerBundle);

                    break;
            }
        }

        /// <summary>Gets data from a Microsoft ECR server.</summary>
        /// <param name="baseUrl">            URL of the resource.</param>
        /// <param name="token">              The token.</param>
        /// <param name="outputDirectory">    Directory to use for export files.</param>
        /// <param name="generateCdcCsv">     True to generate CDC CSV files.</param>
        /// <param name="generateFemaXlsx">   True to generate FEMA XLSX files.</param>
        /// <param name="generateSanerBundle">True to generate FHIR SANER bundles.</param>
        private static void GetDataFromMsEcr(
            Uri baseUrl,
            string token,
            string outputDirectory,
            bool generateCdcCsv,
            bool generateFemaXlsx,
            bool generateSanerBundle)
        {
            Uri uri = new Uri(baseUrl, "/api/data/v9.0/");

            string result;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // get facilities
                result = httpClient.GetStringAsync(EcrFacility.GetUri(uri)).Result;
                ODataResponse<EcrFacility> facilities = JsonConvert.DeserializeObject<ODataResponse<EcrFacility>>(result);

                foreach (EcrFacility ecrFacility in facilities.Value)
                {
                    ReportData data = new ReportData();

                    LocationInfo locationInfo = new LocationInfo(
                        null,
                        ecrFacility.msft_facilityid,
                        null,
                        ecrFacility.msft_facilityname,
                        ecrFacility.msft_addressstreet,
                        ecrFacility.msft_addresscity,
                        ecrFacility.msft_addressstate,
                        null,
                        ecrFacility.msft_addresszip,
                        "US",
                        ecrFacility.msft_addresslat,
                        ecrFacility.msft_addresslong);

                    data.SetLocations(locationInfo, locationInfo, locationInfo);

                    data.AddVentilators(ecrFacility.msft_totalvents, null);

                    result = httpClient.GetStringAsync(EcrLocation.GetUri(uri, ecrFacility.msft_facilityid)).Result;
                    ODataResponse<EcrLocation> locations = JsonConvert.DeserializeObject<ODataResponse<EcrLocation>>(result);

                    foreach (EcrLocation ecrLoc in locations.Value)
                    {
                        switch (ecrLoc.msft_unit)
                        {
                            case "CARD":
                            case "ED":
                            case "GEN SURG":
                            case "MED SURG":

                                data.AddInpatientBeds(
                                    ecrLoc.msft_totalbeds,
                                    (ecrLoc.msft_totalbeds ?? 0) * (ecrLoc.msft_occupancypercentage ?? 0) / 100);

                                break;

                            case "CCU":
                            case "ICU":
                            case "NICU":
                            case "PICU":

                                data.AddIcuBeds(
                                    ecrLoc.msft_totalbeds,
                                    (ecrLoc.msft_totalbeds ?? 0) * (ecrLoc.msft_occupancypercentage ?? 0) / 100);

                                break;

                            case "TELE":
                            default:
                                break;
                        }

                        result = httpClient.GetStringAsync(EcrCovid.GetUri(uri, ecrLoc.msft_locationid)).Result;
                        ODataResponse<EcrCovid> covids = JsonConvert.DeserializeObject<ODataResponse<EcrCovid>>(result);

                        foreach (EcrCovid ecrCovid in covids.Value)
                        {
                            data.AddHospitalized(null, ecrCovid.msft_covidpos, null);
                            data.AddC19TestsToday(ecrCovid.msft_covidpui, ecrCovid.msft_covidpos, null);
                            data.AddC19TestsTotal(ecrCovid.msft_covidpui, ecrCovid.msft_covidpos, null);
                        }
                    }

                    if (generateCdcCsv)
                    {
                        WriteCdcCsv(outputDirectory, data);
                    }

                    if (generateFemaXlsx)
                    {
                        WriteFemaXlsx(outputDirectory, data);
                    }

                    if (generateSanerBundle)
                    {
                        WriteSanerJson(outputDirectory, data);
                    }
                }
            }
        }

        /// <summary>Writes a CDC CSV.</summary>
        /// <param name="outputDirectory">Directory to use for export files.</param>
        /// <param name="data">           The data.</param>
        private static void WriteCdcCsv(
            string outputDirectory,
            ReportData data)
        {
            CdcModel cdc = new CdcModel(data);

            string filename = Path.Combine(outputDirectory, $"{data.Reporter.Name}-cdc.csv");

            using (StreamWriter writer = new StreamWriter(filename))
            using (CsvHelper.CsvWriter csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(new CdcModel[1] { cdc });
            }
        }

        /// <summary>Writes a FEMA XLSX.</summary>
        /// <param name="outputDirectory">Directory to use for export files.</param>
        /// <param name="data">           The data.</param>
        private static void WriteFemaXlsx(
            string outputDirectory,
            ReportData data)
        {
            FemaModel fema = new FemaModel(data);

            string templateName = Path.Combine(Directory.GetCurrentDirectory(), "data", "FEMA_Template.xlsx");

            if (!File.Exists(templateName))
            {
                throw new FileNotFoundException($"Missing FEMA template: {templateName}");
            }

            string filename = Path.Combine(outputDirectory, $"{data.Reporter.Name}-fema.xlsx");

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            File.Copy(templateName, filename);

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filename, true))
            {
                WorksheetPart worksheetPart = doc.WorkbookPart.GetPartsOfType<WorksheetPart>().First();

                SetCellText(
                    worksheetPart,
                    "A",
                    1,
                    $"Date: {data.CollectionStartDate.ToString("yyyy-MM-dd", null)}");

                SetCellNumber(
                    worksheetPart,
                    "B",
                    1,
                    data.TestsPerformedToday);

                SetCellNumber(
                    worksheetPart,
                    "C",
                    1,
                    data.TestsPerformedTotal);

                SetCellNumber(
                    worksheetPart,
                    "D",
                    1,
                    data.TestsResultedToday);

                SetCellNumber(
                    worksheetPart,
                    "E",
                    1,
                    data.TestsRejectedTotal);

                SetCellNumber(
                    worksheetPart,
                    "F",
                    1,
                    data.TestsResultedTotal);

                SetCellNumber(
                    worksheetPart,
                    "G",
                    1,
                    data.C19TestsPositiveToday);

                SetCellNumber(
                    worksheetPart,
                    "H",
                    1,
                    data.C19TestsPositiveTotal);

                if ((data.C19TestsPositiveToday != null) &&
                    (data.TestsPerformedToday != null))
                {
                    SetCellNumber(
                        worksheetPart,
                        "I",
                        1,
                        data.C19TestsPositiveToday / data.TestsPerformedToday);
                }

                if ((data.C19TestsPositiveTotal != null) &&
                    (data.TestsPerformedTotal != null))
                {
                    SetCellNumber(
                        worksheetPart,
                        "J",
                        1,
                        data.C19TestsPositiveTotal / data.TestsPerformedTotal);
                }

                SetCellText(
                    worksheetPart,
                    "B",
                    4,
                    data.CoveredLocation.State);

                SetCellText(
                    worksheetPart,
                    "B",
                    5,
                    data.CoveredLocation.District);

                doc.Save();
            }
        }

        /// <summary>Sets cell number.</summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="col">      The col.</param>
        /// <param name="row">      The row.</param>
        /// <param name="val">      The value.</param>
        private static void SetCellNumber(
            WorksheetPart worksheet,
            string col,
            uint row,
            decimal? val)
        {
            if (val == null)
            {
                return;
            }

            Cell cell = InsertCellInWorksheet(col, row, worksheet);

            cell.CellValue = new CellValue($"{(decimal)val}");
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
        }

        /// <summary>Sets a cell.</summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="col">      The col.</param>
        /// <param name="row">      The row.</param>
        /// <param name="val">      The value.</param>
        private static void SetCellText(
            WorksheetPart worksheet,
            string col,
            uint row,
            string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return;
            }

            Cell cell = InsertCellInWorksheet(col, row, worksheet);

            cell.CellValue = new CellValue(val);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
        }

        /// <summary>
        /// Inserts a cell in worksheet. From:
        /// https://docs.microsoft.com/en-us/office/open-xml/how-to-insert-text-into-a-cell-in-a-spreadsheet.
        /// </summary>
        /// <param name="columnName">   Name of the column.</param>
        /// <param name="rowIndex">     Zero-based index of the row.</param>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <returns>A Cell.</returns>
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        /// <summary>
        /// Inserts a shared string item.
        /// From: https://docs.microsoft.com/en-us/office/open-xml/how-to-insert-text-into-a-cell-in-a-spreadsheet
        /// </summary>
        /// <param name="text">           The text.</param>
        /// <param name="shareStringPart">The share string part.</param>
        /// <returns>An int.</returns>
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int index = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return index;
                }

                index++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return index;
        }

        /// <summary>Writes a saner.</summary>
        /// <param name="outputDirectory">Directory to use for export files.</param>
        /// <param name="data">           The data.</param>
        private static void WriteSanerJson(
            string outputDirectory,
            ReportData data)
        {
            Hl7.Fhir.Model.Bundle bundle = SanerMeasureReport.GetBundle(data, true, true);

            string filename = Path.Combine(outputDirectory, $"{data.Reporter.Name}-saner.json");

            File.WriteAllText(
                filename,
                _jsonSerializer.SerializeToString(bundle));
        }
    }
}
