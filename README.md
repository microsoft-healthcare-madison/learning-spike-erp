# Data Generation

* [`generator-cli`](https://github.com/microsoft-healthcare-madison/learning-spike-erp/tree/master/generator-cli) is a tool for simulating low-fidelity, high-volume bed availability data. Currently it simulates data for **a single day** ("t0"), but we plan to extend it to simulate a time-course of data.

* [`generated`](https://github.com/microsoft-healthcare-madison/learning-spike-erp/tree/master/generated) contains a small amount of generated data, to make it easy to explore and compare formats. The generator simulates data for a (configurable) number of organizations, and for each organization it creates four JSON files (the tool can also output XML, but we'll focus on JSON for documentation below):

  * `Org-###.json` (see [example](https://github.com/microsoft-healthcare-madison/learning-spike-erp/blob/master/generated/t0/Org-1067.json)): This file models a hospital, using a single FHIR Organization and a single Location, which corresponds to a physical site (e.g., a hospital building). These resources are simulated based on data from https://hifld-geoplatform.opendata.arcgis.com/datasets/hospitals, which includes hospital names and geocoordiantes.
  
  * `Org-###-beds.json` (see [example](https://github.com/microsoft-healthcare-madison/learning-spike-erp/blob/master/generated/t0/Org-1067-beds.json)): This `-beds` file models a set of "raw" bed status data for a given hospital. The simulation knows how many beds to simulate for a given hospital, and randomly assigns each bed to some combination of these attributes {location status, physical type, operational status, isolation status, and site}. Based on our defaults, 36 distinct combinations of these attributes are possible.

  * `Org-###-groups.json` (see [example](https://github.com/microsoft-healthcare-madison/learning-spike-erp/blob/master/generated/t0/Org-1067-groups.json)): This `-groups` file summarizes the `-beds` data by creating a number of Groups, following the "saner BedGroup" profile. For each combination with more than zero beds assigned, the simulation creates a saner BedGroup to include in the "-groups" file (up to 36 combinations in the currenty simulation, which uses only a small subset of allowed attribute values; this would grow much larger if we used the full range of bed types statuses, and features).
  
  * `Org-###-measureReports.json` (see [example](https://github.com/microsoft-healthcare-madison/learning-spike-erp/blob/master/generated/t0/Org-1067-measureReports.json)): This `-measureReports` file summarizes the `-beds` data using a single MeasureReport. Instead of creating a new group for each facet, we create a new `stratum` within this MeasureReport, which means we  way, we get one MeasureReport per organization, with up to 36 strata contained inside. (Note taht even though there's only a single MeasureReport, we still wrap it in a Bundle here for consistency across our four main outputs and ease in loading.)

These simulations are rough and quick - please let us know if you spot any bugs, either as as a [GitHub issue](https://github.com/microsoft-healthcare-madison/learning-spike-erp/issues) or [on zulip](https://chat.fhir.org/#narrow/stream/226195-Covid-19-Response).

## Notes on Group vs MeasureReport


### Experience Notes:

MeasureReport "feels" nicer when you start including time periods for which a report is generated. Group doesn't have great semantics around time, particularly in the context of updates/trends. So in a Group-based reporting system, we'd need to decide one something like:

a) generate a new Group for each combination for each day (and put it in name?)
b) reuse/update the same set of Groups each day (if the server supports this, and versioning)
c) something else?


#### Payload size / complexity

The MeasureReport- and Group-based approaches are are similar in terms of line count and file size. When pretty-printed, MeasureReport bundles are shorte in line-count but ~25% larger on-disk; in compacted JSON, MeasureReport bundles are slightly smaller on-disk.


### Coming soon:

Change datasets for time periods. Will allow for variation over time (hence the t0 folder). Upload scripts.

# Getting started

```
git clone https://github.com/microsoft-healthcare-madison/learning-spike-erp
cd learning-spike-erp
dotnet  build generator-cli/generator-cli.sln
./generator-cli/bin/Debug/netcoreapp3.1/generator-cli  --output-directory generated --seed 100
```

# Data Loading
## Loader Setup

Create a new virtual environment and install the script requirements before using the loader script.

```
cd python
python3 -m venv env
source env/bin/activate
pip install -r requirements.txt
```

## Custom Server Endpoint

You can specify your own server endpoint via a command line flag.

```
python3 server-loader.py --server-url http://your.server.here/base/
```

## Deleting Loaded Data
All loaded data is tagged to make it easier to delete before reloading.  To delete tagged data

```
python3 server-loader.py --delete-all
```

## Loading Generated Data

```
python3 server-loader.py --files ../generated
```


# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
