Example measure report (pseudo-FHIR)

resourceType: MeasureReport
status: complete
type: summary (??)
measure: https://audaciousinquiry.github.io/saner-ig/Measure/bed-availability-measure
date: today
reporter: Organization by FHIR id
period: start/end can be "yesterday" (e.g., `2020-03-24`)
group[0]
    // unclear if we really need this code: display: "Bed Report"
    description: "Report of bed status for Hospita A, for 2020-03-24
    measureScore: total bed capacity (# of bed-type Locations in Hospital A)
    stratifier: 
        code[0]: 
            

