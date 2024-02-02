# CluedIn.Enricher.CVR

CluedIn enricher for Danish CVR, which supplies information on companies registered in Denmark.
The CVR service allows searching by either a company's CVR number or name.

------

## Overview

This repository contains the code and associated tests for enriching Danish Companies of Entities and Clues.
The enricher creates searches for `Organization` entity properties which fulfill the following:

+ Its vocabulary is mapped to the `CluedInOrganization.CodesCVR` core vocabulary.
+ Its vocabulary is mapped to the `CluedInOrganization.OrganizationName` and another property mapped to `CluedInOrganization.AddressCountryCode` core vocabulary is either `"dk"`, `"danmark"` or `"denmark"`.

## Usage

In order to connect to the CVR API, you need to obtain a username and password for the service.
A guide in Danish for how to do this can be found at <https://data.virk.dk/datakatalog/erhvervsstyrelsen/system-til-system-adgang-til-cvr-data>.

The registration consists of writing a mail to cvrselvbetjening@erst.dk in which you request access to the service on behalf of the company that wishes to use the CVR data.
CluedIn has a login for development purposes, but for production it will ultimately be the customer's login that should be used.

Once this login is obtained, the app setting `Providers.ExternalSearch.CVR.LiveEndPoint` must be set to the endpoint of the CVR search API alongside your username and password.
The endpoint has the following format:

```
http://<username>:<password>@distribution.virk.dk/cvr-permanent/_search
```

### NuGet Packages

To use the `CVR` External Search with the `CluedIn` server you will have to add the CluedIn.Enricher.CVR nuget package to your environment.

### Running Tests

A mocked environment is required to run `integration` and `acceptance` tests. The mocked environment can be built and run using the following [Docker](https://www.docker.com/) command:

```Shell
docker-compose up --build -d
```

Use the following commands to run all `Unit` and `Integration` tests within the repository:

```Shell
dotnet test .\ExternalSearch.CVR.sln --filter Unit
dotnet test .\ExternalSearch.CVR.sln --filter Integration
```

To run [Pester](https://github.com/pester/Pester) `acceptance` tests

```PowerShell
invoke-pester
```

To review the [WireMock](http://wiremock.org/) HTTP proxy logs

```Shell
docker-compose logs wiremock
```

### Tooling

- [Docker](https://www.docker.com/)
- [Pester](https://github.com/pester/Pester)
- [WireMock](http://wiremock.org/)

  # About CluedIn
CluedIn is the Cloud-native Master Data Management Platform that brings data teams together enabling them to deliver the foundation of high-quality, trusted data that empowers everyone to make a difference. 

We're different because we use enhanced data management techniques like [Graph](https://www.cluedin.com/graph-versus-relational-databases-which-is-best) and [Zero Upfront Modelling](https://www.cluedin.com/upfront-versus-dynamic-data-modelling) to accelerate the time taken to prepare data to deliver insight by as much as 80%. Installed in as little as 20 minutes from the [Azure Marketplace](https://azuremarketplace.microsoft.com/en-gb/marketplace/apps/cluedin.azure_cluedin?tab=Overview), CluedIn is fully integrated with [Microsoft Purview](https://www.cluedin.com/product/microsoft-purview-mdm-integration?hsCtaTracking=461021ab-7a38-41a3-93dd-cfe2325dfd35%7Cb835efc0-e9b7-4385-a1b6-75cb7632527b) and the full [Microsoft Fabric](https://www.cluedin.com/microsoft-fabric) suite, making it the preferred choice for [Azure customers](https://www.cluedin.com/microsoft-intelligent-data-platform). 

To learn more about CluedIn, [contact the team](https://www.cluedin.com/discovery-call) today.

[https://www.cluedin.com](https://www.cluedin.com)
