example.pypf

Example measure report (pseudo-yaml-pseydo-FHIR)(TM)
----

resourceType: MeasureReport
status: complete
type: summary (??)
measure: https://audaciousinquiry.github.io/saner-ig/Measure/bed-availability-measure
date: today
period: start/end can be "yesterday" (e.g., `2020-03-24`)
reporter: Organization by FHIR id
group[0 -- I think we only ever need one, even though this repeats]
    // unclear if we really need this code: display: "Bed Report"
    description: "Report of bed status for Hospita A, for 2020-03-24
    measureScore: total bed capacity (# of bed-type Locations in Hospital A), say 250
    stratifier: 
        stratum[0]
            measureScore: 27 
            component[0] == 
                code  {
                  "system": "http://hl7.org/fhir/R4/StructureDefinition/Location",
                  "code": "Location.status"
                }
                value  {
                    "system": "http://hl7.org/fhir/ValueSet/location-status",
                    "code": "active",
                    "display": "The location is operational."
                  }
            component[1] == 
                code {
                  "system": "http://hl7.org/fhir/R4/StructureDefinition/Location",
                  "code": "Location.operationalStatus"
                }
                value  {
                    "system": "http://terminology.hl7.org/CodeSystem/v2-0116",
                    "code": "U",
                    "display": "Unoccupied"
                  }
            component[2] ==  
                code {
                  "system": "http://hl7.org/fhir/R4/StructureDefinition/Location",
                  "code": "Location.type"
                }
                value   {
                    "system": "http://terminology.hl7.org/CodeSystem/v3-RoleCode",
                    "code": "ICU",
                    "display": "Intensive care unit"
                  }
            component[3] ==
                code {
                  "system": "http://hl7.org/fhir/R4/StructureDefinition/Location",
                  "code": "Location.Feature"
                }
                value  {
                    "system": "http://ainq.com/fhir/us/saner/CodeSystem/SanerBedType",
                    "code": "NONISO",
                    "display": "Non-isolating unit"
                  }
            component[4] == 
                code {
                  "system": "http://hl7.org/fhir/R4/StructureDefinition/Location",
                  "code": "Location.partOf"
                }
                valueCodeableConcept {
                    (!!! warning, this is a hack. Might use an extension.valueReference intead)
                    "system": https://audaciousinquiry.github.io/saner-ig/bed-location
                    "code": "Location/hospital-a-ward-1"
                }
            
        stratum[1] just like stratum[0] but operationalStatus = Occupied
        stratum[2] just like stratum[0] but type=ED
        stratum[3] just like stratum[0] but type=Hospital
        stratum[4] just like stratum[0] but operationalStatus = Occupied, type=ED
        stratum[5] just like stratum[0] but operationalStatus = Occupied, type=Hospital
        stratum[6] just like stratum[0] but Feature = Isolating
        stratum[7] just like stratum[1] but Feature = Isolating
        stratum[8] just like stratum[2] but Feature = Isolating
        stratum[9] just like stratum[3] but Feature = Isolating
        stratum[10] just like stratum[4] but Feature = Isolating
        stratum[11] just like stratum[5] but Feature = Isolating


    ## Notes
    * Can report "leaf-level" strata (pinning down all variables as components) when available, and can roll up values into coarser-grained strata if the detailed sub-counts aren't known

    * Keeps semantics of an entire report together, and handles reporting periods cleanly

    * No good way to convey a stratifier component for valueReferences; the whole mechanism is focused on CodeableConcepts, but we can "hack it" (As proposed here) or use an extension.valueReference, or we use distinction top-level MeasureReport.groups per-ward.
