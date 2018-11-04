using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ProviderRelationships.Jobs.ScheduledJobs
{
    public class CreateReadStoreDatabaseJob
    {
        private readonly IDocumentClient _documentClient;

        public CreateReadStoreDatabaseJob(IDocumentClient documentClient)
        {
            _documentClient = documentClient;
        }

        public async Task Run([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
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