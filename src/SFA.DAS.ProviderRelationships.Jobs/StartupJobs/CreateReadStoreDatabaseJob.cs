using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Jobs.StartupJobs
{
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
                Id = DocumentSettings.CollectionName,
                PartitionKey = new PartitionKeyDefinition
                {
                    Paths = new Collection<string>
                    {
                        "/ap/ukprn"
                    }
                },
                UniqueKeyPolicy = new UniqueKeyPolicy
                {
                    UniqueKeys = new Collection<UniqueKey>
                    {
                        new UniqueKey
                        {
                            Paths = new Collection<string> { "/ap/accountProviderId", "/ale/accountLegalEntityId" }
                        }
                    }
                }
            };
            
            await _documentClient.CreateDatabaseIfNotExistsAsync(database);
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(database.Id), documentCollection);
        }
    }
}