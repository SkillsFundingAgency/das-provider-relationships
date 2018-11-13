using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Jobs.StartupJobs;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Jobs.UnitTests.StartupJobs
{
    [TestFixture]
    [Parallelizable]
    public class CreateReadStoreDatabaseJobTests : FluentTest<CreateReadStoreDatabaseJobTestsFixture>
    {
        [Test]
        public Task Run_WhenRunningCreateReadStoreDatabaseJob_ThenShouldCreateReadStoreDatabase()
        {
            return RunAsync(f => f.Run(), f => f.DocumentClient.Verify(c => c.CreateDatabaseIfNotExistsAsync(It.Is<Database>(d => d.Id == "SFA.DAS.ProviderRelationships.ReadStore.Database"), null), Times.Once));
        }
        
        [Test]
        public Task Run_WhenRunningCreateReadStoreDatabaseJob_ThenShouldCreateReadStorePermissionsCollection()
        {
            return RunAsync(f => f.Run(), f => f.DocumentClient.Verify(c => c.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("SFA.DAS.ProviderRelationships.ReadStore.Database"),
                It.Is<DocumentCollection>(d => 
                    d.Id == "permissions" &&
                    d.PartitionKey.Paths.Contains("/ukprn") &&
                    d.UniqueKeyPolicy.UniqueKeys[0].Paths.Contains("/employerAccountLegalEntityId") &&
                    d.UniqueKeyPolicy.UniqueKeys[0].Paths.Contains("/ukprn")
                ), null), Times.Once));
        }
    }

    public class CreateReadStoreDatabaseJobTestsFixture
    {
        public Mock<IDocumentClient> DocumentClient { get; set; }
        public CreateReadStoreDatabaseJob CreateReadStoreDatabaseJob { get; set; }
        
        public CreateReadStoreDatabaseJobTestsFixture()
        {
            DocumentClient = new Mock<IDocumentClient>();
            CreateReadStoreDatabaseJob = new CreateReadStoreDatabaseJob(DocumentClient.Object);
        }

        public Task Run()
        {
            return CreateReadStoreDatabaseJob.Run(null);
        }
    }
}