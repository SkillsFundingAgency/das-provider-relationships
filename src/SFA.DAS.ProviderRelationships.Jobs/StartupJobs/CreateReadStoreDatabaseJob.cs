using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Jobs.StartupJobs;

public class CreateReadStoreDatabaseJob
{
    private readonly IDocumentClient _documentClient;

    public CreateReadStoreDatabaseJob(IDocumentClient documentClient)
    {
        _documentClient = documentClient;
    }
        
    [NoAutomaticTrigger]
    public async Task Run(ILogger logger)
    {
        var database = new Database
        {
            Id = DocumentSettings.DatabaseName
        };
            
        var documentCollection = new DocumentCollection
        {
            Id = DocumentSettings.AccountProviderLegalEntitiesCollectionName,
            PartitionKey = new PartitionKeyDefinition
            {
                Paths = new Collection<string>
                {
                    "/ukprn"
                }
            },
            UniqueKeyPolicy = new UniqueKeyPolicy
            {
                UniqueKeys = new Collection<UniqueKey>
                {
                    new UniqueKey
                    {
                        Paths = new Collection<string> { "/accountProviderLegalEntityId" }
                    },
                    new UniqueKey
                    {
                        Paths = new Collection<string> { "/accountLegalEntityId" }
                    }
                }
            }
        };
            
        var requestOptions = new RequestOptions
        {
            OfferThroughput = 1000
        };
            
        await _documentClient.CreateDatabaseIfNotExistsAsync(database);
        await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(database.Id), documentCollection, requestOptions);
    }
}