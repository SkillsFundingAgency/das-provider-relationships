using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

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
                Id = "SFA.DAS.ProviderRelationships.ReadStore.Database"
            };
            
            var documentCollection = new DocumentCollection
            {
                Id = "permissions",
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
                            Paths = new Collection<string> { "/employerAccountLegalEntityId", "/ukprn" }
                        }
                    }
                }
            };
            
            await _documentClient.CreateDatabaseIfNotExistsAsync(database);
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(database.Id), documentCollection);
        }
    }
}