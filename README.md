# SFA.DAS.ProviderRelationships

[![Build status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/Manage%20Apprenticeships/das-provider-relationships)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1183)

#### Requirements

1. Install [Visual Studio] with these workloads:
    * ASP.NET and web development
    * Azure development
    * Data storage and processing
    * .NET Core cross-platform development 
    * .NET desktop development
2. Install [Azure Storage Explorer]
3. Install [Cosmos DB Emulator]

#### Setup

##### Publish database

* Open the solution in Visual Studio.
* Right click on the `SFA.DAS.ProviderRelationships.Database` project in solution explorer.
* Click Publish.
* Click Edit.
* Click Browse.
* Select Local > MSSQLLocalDB.
* Click OK.
* Enter `SFA.DAS.ProviderRelationships.Database` as the database name.
* Click Publish.

> If you have previously published the database you may need to drop it first, to avoid the script aborting after a data truncation error.

##### Add configuration to Azure Storage Emulator

* Clone the [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) repository.
* Clone the [das-employer-config-updater](https://github.com/SkillsFundingAgency/das-employer-config-updater) repository.
* Run Azure Storage Emulator.
* Open the `das-employer-config-updater` solution in Visual Studio.
* Press F5 and follow the instructions to import the config from the directory that you cloned the `das-employer-config repository` to.

> The two repositories above are private. If the links appear to be dead make sure that you are logged into GitHub with an account that has access to these i.e. that you are part of the Skills Funding Agency Team organization.

##### Add Certificates

Execute DevInstall.ps1 as an admin to import required certificates into their appropriate store locations.

##### Create Azure Service Bus dead letter queue (optional)

If you intend on using your own development service bus (LearningTransport is used by default), create a queue named `deadletters`.

##### Seed data

* Right click `SFA.DAS.ProviderRelationships.Jobs`.
* Click Debug > Start new instance.
* Wait until you see "Executed 'ImportProvidersJob.Run'" in the console window. 
* Close the console window.

##### Run the website

* Set `SFA.DAS.ProviderRelationships.Web` as the startup project.
* Press F5.

[Azure Storage Explorer]: http://storageexplorer.com
[Choclatey]: https://chocolatey.org
[Cosmos DB Emulator]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
[Docker]: https://www.docker.com
[Elastic Search]: https://www.elastic.co/products/elasticsearch
[SQL Server Management Studio]: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms
[Visual Studio]: https://www.visualstudio.com