// <copyright file="MeasureReportGenerator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
//     Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// </copyright>
using System;
using System.Collections.Generic;
using System.Text;
using generator_cli.Models;
using Hl7.Fhir.Model;

namespace generator_cli.Generators
{
    /// <summary>A measure report generator.</summary>
    public class MeasureReportGenerator
    {
        private Organization _org;
        private Location _location;
        private OrgDeviceData _deviceData;
        private OrgPatientData _patientData;
        private OrgTestData _testData;
        private Period _period;
        Dictionary<string, decimal> _scoresByGroupCode = new Dictionary<string, decimal>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureReportGenerator"/> class.
        /// </summary>
        /// <param name="org">        The organisation.</param>
        /// <param name="location">   The location.</param>
        /// <param name="deviceData"> Information describing the device.</param>
        /// <param name="patientData">Information describing the patient.</param>
        /// <param name="testData">   Information describing the test.</param>
        /// <param name="period">     The period.</param>
        public MeasureReportGenerator(
            Organization org,
            Location location,
            OrgDeviceData deviceData,
            OrgPatientData patientData,
            OrgTestData testData,
            Period period)
        {
            if (org == null)
            {
                throw new ArgumentNullException(nameof(org));
            }

            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }

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

            if (period == null)
            {
                throw new ArgumentNullException(nameof(period));
            }

            _org = org;
            _location = location;
            _deviceData = deviceData;
            _patientData = patientData;
            _testData = testData;
            _period = period;

            BuildScoreDict();
        }

        /// <summary>Builds score dictionaries.</summary>
        private void BuildScoreDict()
        {
            int val;

            _scoresByGroupCode.Add(MeasureGenerator.CDCTotalBeds, _deviceData.TotalBeds);

            _scoresByGroupCode.Add(MeasureGenerator.CDCInpatientBeds, _deviceData.Inpatient);

            val = Math.Min(_deviceData.Inpatient, _patientData.Total);
            _scoresByGroupCode.Add(MeasureGenerator.CDCInpatientBedOccupancy, val);

            _scoresByGroupCode.Add(MeasureGenerator.CDCIcuBeds, _deviceData.ICU);

            val = Math.Min(_deviceData.ICU, _patientData.NegativeNeedIcu + _patientData.PositiveNeedIcu);
            _scoresByGroupCode.Add(MeasureGenerator.CDCIcuBedOccupancy, val);

            _scoresByGroupCode.Add(MeasureGenerator.CDCVentilators, _deviceData.Ventilators);

            val = Math.Min(_deviceData.Ventilators, _patientData.NegativeNeedVent + _patientData.PositiveNeedVent);
            _scoresByGroupCode.Add(MeasureGenerator.CDCVentilatorsInUse, val);

            val = Math.Min(_deviceData.TotalBeds - _patientData.Negative, _patientData.Positive);
            _scoresByGroupCode.Add(MeasureGenerator.CDCHospitalizedPatients, val);

            val = Math.Min(_deviceData.Ventilators - _patientData.NegativeNeedVent, _patientData.PositiveNeedVent);
            _scoresByGroupCode.Add(MeasureGenerator.CDCVentilatedPatients, val);

            _scoresByGroupCode.Add(MeasureGenerator.CDCHospitalOnset, _patientData.OnsetInCare);

            val = Math.Max(0, _patientData.Positive - (_deviceData.TotalBeds - _patientData.Negative));
            _scoresByGroupCode.Add(MeasureGenerator.CDCAwaitingBeds, val);

            val = Math.Max(0, _patientData.PositiveNeedVent - (_deviceData.Ventilators - _patientData.NegativeNeedVent));
            _scoresByGroupCode.Add(MeasureGenerator.CDCAwaitingVentilators, val);

            _scoresByGroupCode.Add(MeasureGenerator.CDCDied, _patientData.Died);
        }

        /// <summary>Gets report bundle.</summary>
        /// <returns>The report bundle.</returns>
        public Bundle GetReportBundle()
        {
            string bundleId = FhirGenerator.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirGenerator.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            string id;

            bundle.Entry = new List<Bundle.EntryComponent>();

            // bundle.AddResourceEntry(
            //     ReportForMeasure(out id, MeasureGenerator.TestTotal, _testData.Performed),
            //     $"{SystemLiterals.Internal}MeasureReport/{id}");
            // bundle.AddResourceEntry(
            //     ReportForMeasure(out id, MeasureGenerator.TestPositiveTotal, _testData.Positive),
            //     $"{SystemLiterals.Internal}MeasureReport/{id}");

            AddCdcMeasure(ref bundle, MeasureGenerator.CDCTotalBeds);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCInpatientBeds);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCInpatientBedOccupancy);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCIcuBeds);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCIcuBedOccupancy);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCVentilators);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCVentilatorsInUse);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCHospitalizedPatients);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCVentilatedPatients);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCHospitalOnset);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCAwaitingBeds);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCAwaitingVentilators);
            AddCdcMeasure(ref bundle, MeasureGenerator.CDCDied);

            return bundle;
        }

        /// <summary>Adds a cdc measure to 'measureName'.</summary>
        /// <param name="bundle">     [in,out] The bundle.</param>
        /// <param name="measureName">Name of the measure.</param>
        private void AddCdcMeasure(ref Bundle bundle, string measureName)
        {
            bundle.AddResourceEntry(
                ReportForMeasure(
                    out string id,
                    MeasureGenerator.CDCMeasure(measureName),
                    _scoresByGroupCode[measureName]),
                $"{SystemLiterals.Internal}MeasureReport/{id}");
        }

        /// <summary>Gets group report bundle.</summary>
        /// <returns>The group report bundle.</returns>
        public Bundle GetGroupReportBundle()
        {
            string bundleId = FhirGenerator.NextId;

            Bundle bundle = new Bundle()
            {
                Id = bundleId,
                Identifier = FhirGenerator.IdentifierForId(bundleId),
                Type = Bundle.BundleType.Collection,
                Timestamp = new DateTimeOffset(DateTime.Now),
                Meta = new Meta(),
            };

            string id;

            bundle.Entry = new List<Bundle.EntryComponent>();

            bundle.AddResourceEntry(
                ReportForCdcGroupedMeasure(
                    out id,
                    MeasureGenerator.CDCGroupedMeasure("beds")),
                $"{SystemLiterals.Internal}MeasureReport/{id}");

            return bundle;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureReportGenerator"/> class.
        /// </summary>
        /// <param name="id">     [out] The identifier.</param>
        /// <param name="measure">The measure.</param>
        /// <returns>A MeasureReport.</returns>
        private MeasureReport ReportForCdcGroupedMeasure(
            out string id,
            Measure measure)
        {
            id = FhirGenerator.NextId;

            MeasureReport report = new MeasureReport()
            {
                Id = id,
                Subject = new ResourceReference(
                    $"{_location.ResourceType}/{_location.Id}",
                    _org.Name),
                Status = MeasureReport.MeasureReportStatus.Complete,
                Type = MeasureReport.MeasureReportType.Summary,
                Date = new FhirDateTime(new DateTimeOffset(DateTime.Now)).ToString(),
                Period = _period,
                Measure = measure.Url,
                Reporter = new ResourceReference(
                    $"{_org.ResourceType}/{_org.Id}",
                    _org.Name),
                Group = new List<MeasureReport.GroupComponent>(),
            };

            foreach (Measure.GroupComponent measureGroup in measure.Group)
            {
                string groupName = measureGroup.Code.Coding[0].Code;

                if (!_scoresByGroupCode.ContainsKey(groupName))
                {
                    continue;
                }

                MeasureReport.GroupComponent reportGroup = new MeasureReport.GroupComponent()
                {
                    Code = measureGroup.Code,
                    Population = new List<MeasureReport.PopulationComponent>(),
                    MeasureScore = new Quantity() { Value = _scoresByGroupCode[groupName], },
                };

                foreach (Measure.PopulationComponent population in measure.Group[0].Population)
                {
                    if ((population.Code == null) ||
                        (population.Code.Coding == null) ||
                        (population.Code.Coding.Count == 0))
                    {
                        continue;
                    }

                    switch (population.Code.Coding[0].Code)
                    {
                        case "measure-population":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (int)_scoresByGroupCode[groupName],
                            });
                            break;

                        case "measure-observation":
                            // ignore
                            break;

                        // TODO: need numerators and denominators
                        case "numerator":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = 1,
                            });
                            break;

                        // TODO: need numerators and denominators
                        case "denominator":
                            reportGroup.Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = 1,
                            });
                            break;
                    }
                }

                if (reportGroup.Population.Count == 0)
                {
                    reportGroup.Population = null;
                }

                report.Group.Add(reportGroup);
            }

            return report;

        }

        /// <summary>Reports for measure.</summary>
        /// <param name="id">         [out] The identifier.</param>
        /// <param name="measure">    The measure.</param>
        /// <param name="score">      The score.</param>
        /// <param name="numerator">  (Optional) The numerator.</param>
        /// <param name="denominator">(Optional) The denominator.</param>
        /// <returns>A MeasureReport.</returns>
        private MeasureReport ReportForMeasure(
            out string id,
            Measure measure,
            decimal score,
            int? numerator = null,
            int? denominator = null)
        {
            id = FhirGenerator.NextId;

            MeasureReport report = new MeasureReport()
            {
                Id = id,
                Subject = new ResourceReference(
                    $"{_location.ResourceType}/{_location.Id}",
                    _org.Name),
                Status = MeasureReport.MeasureReportStatus.Complete,
                Type = MeasureReport.MeasureReportType.Summary,
                Date = new FhirDateTime(new DateTimeOffset(DateTime.Now)).ToString(),
                Period = _period,
                Measure = measure.Url,
                Reporter = new ResourceReference(
                    $"{_org.ResourceType}/{_org.Id}",
                    _org.Name),
                Group = new List<MeasureReport.GroupComponent>()
                {
                    new MeasureReport.GroupComponent(),
                },
            };

            report.Group[0].MeasureScore = new Quantity() { Value = score };
            report.Group[0].Population = new List<MeasureReport.PopulationComponent>();

            if (measure.Group[0].Population != null)
            {
                foreach (Measure.PopulationComponent population in measure.Group[0].Population)
                {
                    if ((population.Code == null) ||
                        (population.Code.Coding == null) ||
                        (population.Code.Coding.Count == 0))
                    {
                        continue;
                    }

                    switch (population.Code.Coding[0].Code)
                    {
                        case "measure-population":
                            report.Group[0].Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = (int)score,
                            });
                            break;

                        case "measure-observation":
                            // ignore
                            break;

                        case "numerator":
                            report.Group[0].Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = numerator ?? 1,
                            });
                            break;

                        case "denominator":
                            report.Group[0].Population.Add(new MeasureReport.PopulationComponent()
                            {
                                Code = population.Code,
                                Count = denominator ?? 1,
                            });
                            break;
                    }
                }
            }

            if (report.Group[0].Population.Count == 0)
            {
                report.Group[0].Population = null;
            }

            return report;
        }
    }
}
