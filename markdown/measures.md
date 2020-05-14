# FEMADailyHospitalCOVID19Reporting

## Related artifacts
* [Trump Administration Engages America’s Hospitals in Unprecedented Data Sharing](https://www.cms.gov/newsroom/press-releases/trump-administration-engages-americas-hospitals-unprecedented-data-sharing)
* [Text of a Letter from the Vice President to Hospital Administrators](https://www.whitehouse.gov/briefings-statements/text-letter-vice-president-hospital-administrators/)
* [Administration Requests Hospitals Report Daily on COVID-19 Testing Results, Bed and Ventilator](https://www.aha.org/advisory/2020-03-30-coronavirus-update-administration-requests-hospitals-report-daily-covid-19)
* [Template for Daily Hospital COVID-19 Reporting.xlsx](https://images.magnetmail.net/images/clients/AHA_MCHF/attach/2020/March/0330/Template_for_Daily_Hospital_COVID19_Reporting.xlsx)


## Group Definitions
Group System|Group Code|Population System|Population Code
------------|----------|-----------------|---------------
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|positiveIncreasePercent|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|totalOrdersIncrease<br/>initial-population
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|totalTestResultsIncrease<br/>denominator
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|positiveIncrease<br/>numerator
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|positivePercent|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|totalOrders<br/>initial-population
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|totalTestResults<br/>denominator
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|positive<br/>numerator
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|rejected<br/>denominator-exclusion
# CDCHealthcareSupplyPathway

## Related artifacts
* [CDC/NHSN COVID-19 Acute Care Module Home Page](https://www.cdc.gov/nhsn/acute-care-hospital/covid19/)
* [Facility - How to Upload COVID-19 CSV Data Files](https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf)
* [NHSN COVID-19 Module Analysis Reports](https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf)
* [Instructions for Completion of the COVID-19 Healthcare Supply Pathway (CDC 57.132)](https://www.cdc.gov/nhsn/pdfs/covid19/57.132-toi-508.pdf)
* [Healthcare Supply Pathway Form](https://www.cdc.gov/nhsn/pdfs/covid19/57.132-covid19-sup-blank-p.pdf)
* [CDC/NHSN COVID-19 Acute Care Healthcare Supply Reporting CSV File Template](https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-supplies.csv)


## Group Definitions
Group System|Group Code|Population System|Population Code
------------|----------|-----------------|---------------
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|Ventilators|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|ventsupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|ventreuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|ventobtain
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|N95Masks|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|n95masksupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|n95maskreuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|n95maskobtain
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|OtherRespirators|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|othrespsupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|othrespreuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|othrespobtain
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|SurgicalMasks|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|surgmasksupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|surgmaskreuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|surgmaskobtain
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|EyeProtection|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|shieldsupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|shieldreuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|shieldobtain
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|Gown|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|gownsupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|gownreuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|gownobtain
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|Glove|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|glovesupply
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|glovereuse
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|gloveobtain
# CDCHealthcareWorkerStaffingPathway

## Related artifacts
* [CDC/NHSN COVID-19 Acute Care Module Home Page](https://www.cdc.gov/nhsn/acute-care-hospital/covid19/)
* [Facility - How to Upload COVID-19 CSV Data Files](https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf)
* [NHSN COVID-19 Module Analysis Reports](https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf)
* [Instructions for Completion of the COVID-19 Healthcare Worker Staffing Pathway (CDC 57.131)](https://www.cdc.gov/nhsn/pdfs/covid19/57.131-toi-508.pdf)
* [Healthcare Worker Staffing Pathway Form](https://www.cdc.gov/nhsn/pdfs/covid19/57.131-covid19-hwp-blank-p.pdf)
* [CDC/NHSN COVID-19 Acute Care Healthcare Supply Reporting CSV File Template](https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-hcw.csv)


## Group Definitions
Group System|Group Code|Population System|Population Code
------------|----------|-----------------|---------------
http://hl7.org/fhir/us/saner|shortenvsvc|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortnurse|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortrt|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortphar|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortphys|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shorttemp|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortoth|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortothlic|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|shortothsfy|<nobr/>|<nobr/>
http://hl7.org/fhir/us/saner|posshortenvsvc|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortnurse|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortrt|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortphar|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortphys|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshorttemp|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortoth|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortothlic|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|true
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation|false
http://hl7.org/fhir/us/saner|posshortothsfy|<nobr/>|<nobr/>
# CDCPatientImpactAndHospitalCapacity

## Related artifacts
* [CDC/NHSN COVID-19 Acute Care Module Home Page](https://www.cdc.gov/nhsn/acute-care-hospital/covid19/)
* [Facility - How to Upload COVID-19 CSV Data Files](https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf)
* [NHSN COVID-19 Module Analysis Reports](https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf)
* [Instructions for Completion of the COVID-19 Patient Impact and Hospital Capacity Pathway (CDC 57.130)](https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf)
* [Patient Impact and Hospital Capacity Pathway Form](https://www.cdc.gov/nhsn/pdfs/covid19/57.130-covid19-pimhc-blank-p.pdf)
* [CDC/NHSN COVID-19 Acute Care Patient Impact Reporting CSV File Template](https://www.cdc.gov/nhsn/pdfs/covid19/covid19-test-csv-import.csv)


## Group Definitions
Group System|Group Code|Population System|Population Code
------------|----------|-----------------|---------------
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|Beds|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|numTotBeds<br/>initial-population
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numbeds
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numBedsOcc
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numICUBeds
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numICUBedsOcc
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|Ventilators|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population|numVent<br/>initial-population
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numVentUse
http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem|Encounters|<nobr/>|<nobr/>
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numC19HospPats
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numC19MechVentPats
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numC19HOPats
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numC19OverflowPats
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numC19OFMechVentPats
<nobr/>|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem|numC19Died
