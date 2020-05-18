# FEMADailyHospitalCOVID19Reporting

SANER FEMA Template for daily Hospital COVID-19 Reporting

SANER implementation of the FEMA Template for daily Hospital COVID-19 Reporting
## Related artifacts
* [Trump Administration Engages America’s Hospitals in Unprecedented Data Sharing](https://www.cms.gov/newsroom/press-releases/trump-administration-engages-americas-hospitals-unprecedented-data-sharing)
* [Text of a Letter from the Vice President to Hospital Administrators](https://www.whitehouse.gov/briefings-statements/text-letter-vice-president-hospital-administrators/)
* [Administration Requests Hospitals Report Daily on COVID-19 Testing Results, Bed and Ventilator](https://www.aha.org/advisory/2020-03-30-coronavirus-update-administration-requests-hospitals-report-daily-covid-19)
* [Template for Daily Hospital COVID-19 Reporting.xlsx](https://images.magnetmail.net/images/clients/AHA_MCHF/attach/2020/March/0330/Template_for_Daily_Hospital_COVID19_Reporting.xlsx)


## Group Definitions
Group Code|Population Code|System
----------|---------------|------
positiveIncreasePercent|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|totalOrdersIncrease<br/>initial-population|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|totalTestResultsIncrease<br/>denominator|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|positiveIncrease<br/>numerator|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
positivePercent|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|totalOrders<br/>initial-population|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|totalTestResults<br/>denominator|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|positive<br/>numerator|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|rejected<br/>denominator-exclusion|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population

## Field Definitions
Field Name|Title|Description
----------|-----|-----------
positive|Cumulative Positive COVID-19 Tests|All positive test results released to date.
positiveIncrease|New Positive COVID-19 Tests|Midnight to midnight cutoff, positive test results released on the previous date queried.
rejected|Cumulative Specimens Rejected|All specimens rejected for testing to date.
totalOrders|Cumulative Diagnostic Tests Ordered/Received|All tests ordered to date.
totalOrdersIncrease|New Diagnostic Tests Ordered/Received|Midnight to midnight cutoff, tests ordered on previous date queried.
totalTestResults|Cumulative Tests Performed|All tests with results released to date.
totalTestResultsIncrease|New Tests Resulted|Midnight to midnight cutoff, test results released on previous date queried.
# CDCHealthcareSupplyPathway

COVID-19 Healthcare Supply Pathway

SANER implementation of the CDC COVID-19 Healthcare Supply Pathway
## Related artifacts
* [CDC/NHSN COVID-19 Acute Care Module Home Page](https://www.cdc.gov/nhsn/acute-care-hospital/covid19/)
* [Facility - How to Upload COVID-19 CSV Data Files](https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf)
* [NHSN COVID-19 Module Analysis Reports](https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf)
* [Instructions for Completion of the COVID-19 Healthcare Supply Pathway (CDC 57.132)](https://www.cdc.gov/nhsn/pdfs/covid19/57.132-toi-508.pdf)
* [Healthcare Supply Pathway Form](https://www.cdc.gov/nhsn/pdfs/covid19/57.132-covid19-sup-blank-p.pdf)
* [CDC/NHSN COVID-19 Acute Care Healthcare Supply Reporting CSV File Template](https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-supplies.csv)


## Group Definitions
Group Code|Population Code|System
----------|---------------|------
Ventilators|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|ventsupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|ventreuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|ventobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
N95Masks|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|n95masksupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|n95maskreuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|n95maskobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
OtherRespirators|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|othrespsupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|othrespreuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|othrespobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
SurgicalMasks|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|surgmasksupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|surgmaskreuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|surgmaskobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
EyeProtection|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|shieldsupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|shieldreuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|shieldobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
Gown|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|gownsupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|gownreuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|gownobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
Glove|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|glovesupply|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|glovereuse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|gloveobtain|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem

## Field Definitions
Field Name|Title|Description
----------|-----|-----------
gloveobtain|Gloves|
glovereuse|Gloves|
glovesupply|Gloves|
gownobtain|Gowns (single use)|
gownreuse|Gowns (single use)|
gownsupply|Gowns (single use)|
n95maskobtain|N95 masks|
n95maskreuse|N95 masks|
n95masksupply|N95 masks|
othrespobtain|Other respirators including PAPRs|Other respirators such as PAPRs or elastomerics
othrespreuse|Other respirators including PAPRs|Other respirators such as PAPRs or elastomerics
othrespsupply|Other respirators including PAPRs|Other respirators such as PAPRs or elastomerics
shieldobtain|Eye protection including face shields or goggles|
shieldreuse|Eye protection including face shields or goggles|
shieldsupply|Eye protection including face shields or goggles|
surgmaskobtain|Surgical masks|
surgmaskreuse|Surgical masks|
surgmasksupply|Surgical masks|
ventobtain|Ventilator supplies (any, including tubing)|any supplies, including flow sensors, tubing, connectors, valves, filters, etc
ventreuse|Ventilator supplies (any, including tubing)|any supplies, including flow sensors, tubing, connectors, valves, filters, etc
ventsupply|Ventilator supplies (any, including tubing)|any supplies, including flow sensors, tubing, connectors, valves, filters, etc
# CDCHealthcareWorkerStaffingPathway

COVID-19 Healthcare Worker Staffing Pathway

SANER implementation of the CDC COVID-19 Healthcare Worker Staffing Pathway
## Related artifacts
* [CDC/NHSN COVID-19 Acute Care Module Home Page](https://www.cdc.gov/nhsn/acute-care-hospital/covid19/)
* [Facility - How to Upload COVID-19 CSV Data Files](https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf)
* [NHSN COVID-19 Module Analysis Reports](https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf)
* [Instructions for Completion of the COVID-19 Healthcare Worker Staffing Pathway (CDC 57.131)](https://www.cdc.gov/nhsn/pdfs/covid19/57.131-toi-508.pdf)
* [Healthcare Worker Staffing Pathway Form](https://www.cdc.gov/nhsn/pdfs/covid19/57.131-covid19-hwp-blank-p.pdf)
* [CDC/NHSN COVID-19 Acute Care Healthcare Supply Reporting CSV File Template](https://www.cdc.gov/nhsn/pdfs/covid19/facility-import-hcw.csv)


## Group Definitions
Group Code|Population Code|System
----------|---------------|------
shortenvsvc|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortnurse|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortrt|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortphar|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortphys|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shorttemp|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortoth|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortothlic|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
shortothsfy|<nobr/>|http://hl7.org/fhir/us/saner
posshortenvsvc|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortnurse|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortrt|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortphar|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortphys|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshorttemp|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortoth|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortothlic|<nobr/>|http://hl7.org/fhir/us/saner
<nobr/>|true|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
<nobr/>|false|http://hl7.org/fhir/us/saner/CodeSystem/BooleanAggregation
posshortothsfy|<nobr/>|http://hl7.org/fhir/us/saner

## Field Definitions
Field Name|Title|Description
----------|-----|-----------
posshortenvsvc|Environmental services|Front-line persons who clean patient rooms and all areas in a healthcare facility
posshortnurse|Nurses|Registered nurses and licensed practical nurses
posshortoth|Other HCP|Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above.
posshortothlic|Other licensed independent practitioners|Advanced practice nurses, physician assistants
posshortothsfy|Other HCP - Specify the groups|Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above.
posshortphar|Pharmacists and pharmacy techs|
posshortphys|Physicians|Attending physicians, fellows
posshortrt|Respiratory therapists|Certified medical professionals who specialize in knowledge and use of mechanical ventilation as well as other programs for respiratory care
posshorttemp|Temporary physicians, nurses, respiratory therapists, and pharmacists|'per diems', 'travelers', retired, or other seasonal or intermittently contracted persons
shortenvsvc|Environmental services|Front-line persons who clean patient rooms and all areas in a healthcare facility
shortnurse|Nurses|Registered nurses and licensed practical nurses
shortoth|Other HCP|Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above
shortothlic|Other licensed independent practitioners|Advanced practice nurses, physician assistants
shortothsfy|Other HCP - Specify the groups|Persons who work in the facility, regardless of clinical responsibility or patient contact not included in categories above
shortphar|Pharmacists and pharmacy techs|
shortphys|Physicians|Attending physicians, fellows
shortrt|Respiratory therapists|Certified medical professionals who specialize in knowledge and use of mechanical ventilation as well as other programs for respiratory care
shorttemp|Temporary physicians, nurses, respiratory therapists, and pharmacists|'per diems', 'travelers', retired, or other seasonal or intermittently contracted persons
# CDCPatientImpactAndHospitalCapacity

Patient Impact and Hospital Capacity

SANER implementation of the CDC COVID-19 Patient Impact & Hospital Capacity Module
## Related artifacts
* [CDC/NHSN COVID-19 Acute Care Module Home Page](https://www.cdc.gov/nhsn/acute-care-hospital/covid19/)
* [Facility - How to Upload COVID-19 CSV Data Files](https://www.cdc.gov/nhsn/pdfs/covid19/import-covid19-data-508.pdf)
* [NHSN COVID-19 Module Analysis Reports](https://www.cdc.gov/nhsn/pdfs/covid19/fac-analysis-qrg-508.pdf)
* [Instructions for Completion of the COVID-19 Patient Impact and Hospital Capacity Pathway (CDC 57.130)](https://www.cdc.gov/nhsn/pdfs/covid19/57.130-toi-508.pdf)
* [Patient Impact and Hospital Capacity Pathway Form](https://www.cdc.gov/nhsn/pdfs/covid19/57.130-covid19-pimhc-blank-p.pdf)
* [CDC/NHSN COVID-19 Acute Care Patient Impact Reporting CSV File Template](https://www.cdc.gov/nhsn/pdfs/covid19/covid19-test-csv-import.csv)


## Definitions
* Ventilator:
Any device used to support, assist or control respiration (inclusive of the weaning period) through the application of positive
pressure to the airway when delivered via an artificial airway, specifically an oral/nasal endotracheal or tracheostomy tube.
Note: Ventilation and lung expansion devices that deliver positive pressure to the airway (for example: CPAP, BiPAP, bi-level, IPPB and
PEEP) via non-invasive means (for example: nasal prongs, nasal mask, full face mask, total mask, etc.) are not considered ventilators
unless positive pressure is delivered via an artificial airway (oral/nasal endotracheal or tracheostomy tube).
* Beds:
Baby beds in mom's room count as 1 bed, even if there are multiple baby beds
Follow-up in progress if staffed is less than licensed.
Total includes all beds, even if with surge beds it exceeds licensed beds.
* ICU beds:
Include NICU (from CDC Webinar 31-Mar-2020) (outstanding question on burn unit)
## Group Definitions
Group Code|Population Code|System
----------|---------------|------
Beds|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|numTotBeds<br/>initial-population|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|numbeds|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numBedsOcc|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numICUBeds|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numICUBedsOcc|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
Ventilators|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|numVent<br/>initial-population|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem<br/>http://terminology.hl7.org/CodeSystem/measure-population
<nobr/>|numVentUse|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
Encounters|<nobr/>|http://hl7.org/fhir/us/saner/CodeSystem/MeasureGroupSystem
<nobr/>|numC19HospPats|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numC19MechVentPats|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numC19HOPats|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numC19OverflowPats|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numC19OFMechVentPats|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numC19Died|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numc19confnewadm|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numc19suspnewadm|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numc19honewpats|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem
<nobr/>|numc19prevdied|http://hl7.org/fhir/us/saner/CodeSystem/MeasurePopulationSystem

## Field Definitions
Field Name|Title|Description
----------|-----|-----------
numbeds|Hospital Inpatient Beds|Inpatient beds, including all staffed, licensed, and overflow(surge) beds used for inpatients.
numBedsOcc|Hospital Inpatient Bed Occupancy|Total number of staffed inpatient beds that are occupied.
numc19confnewadm|Previous Day’s Admissions with Confirmed COVID-19|New patients admitted to an inpatient bed who had confirmed COVID-19 at the time of admission.
numC19Died|COVID-19 Patient Deaths - Total for Calendar Dates 05/14/2020 and PRIOR|Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location on Calendar Dates 05/14/2020 and PRIOR.
numc19honewpats|New Hospital Onset|Current inpatients hospitalized for a condition other than COVID-19 with onset of suspected or confirmed COVID-19 on the previous day and the previous day is fourteen or more days since admission.
numC19HOPats|Hospital Onset COVID-19 Patients|Patients hospitalized in an NHSN inpatient care location with onset of suspected or confirmed COVID - 19 14 or more days after hospitalization.
numC19HospPats|Hospitalized COVID-19 Patients|Patients currently hospitalized in an inpatient care location who have suspected or confirmed COVID-19.
numC19MechVentPats|Hospitalized and Ventilated COVID-19 Patients|Patients hospitalized in an NHSN inpatient care location who have suspected or confirmed COVID-19 and are on a mechanical ventilator.
numC19OFMechVentPats|ED/Overflow and Ventilated COVID-19 Patients|Patients with suspected or confirmed COVID - 19 who are in the ED or any overflow location awaiting an inpatient bed and on a mechanical ventilator.
numC19OverflowPats|ED/Overflow COVID-19 Patients|Patients with suspected or confirmed COVID-19 who are in the ED or any overflow location awaiting an inpatient bed.
numc19prevdied|Previous Day's Deaths| Patients with suspected or confirmed COVID-19 who died in the hospital, ED, or any overflow location on the previous calendar day.
numc19suspnewadm|Previous Day’s Admissions with Suspected COVID-19|New patients admitted to an inpatient bed who had suspected COVID-19 at the time of admission.
numICUBeds|ICU Beds|Total number of staffed inpatient intensive care unit (ICU) beds.
numICUBedsOcc|ICU Bed Occupancy|Total number of staffed inpatient ICU beds that are occupied.
numTotBeds|All Hospital Beds|Total number of all Inpatient and outpatient beds, including all staffed, ICU, licensed, and overflow(surge) beds used for inpatients or outpatients.
numVent|Mechanical Ventilators|Total number of ventilators available.
numVentUse|Mechanical Ventilators in Use|Total number of ventilators in use.
