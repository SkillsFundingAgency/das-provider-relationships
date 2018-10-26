using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests.Tests
{
    [TestFixture]
    [Parallelizable]
    public class CosmosClientFactoryTests : FluentTest<CosmosClientFactoryTestsFixture>
    {
        [Ignore("Trying to load DocumentClient requires System.Net.Http 4.1.0 and fails")]
        [Test]
        public void CosmosClientFactory_WhenCreatingDocumentClient_ThenShouldReturnDocumentClientObject()
        {
            Run(
                f => f.DocumentClientFactory.Create(f.DocumentConfguration),
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests.Should()
                        .Be(f.DocumentConfguration.MaxRetryAttemptsOnThrottledRequests);
                    r.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds.Should()
                        .Be(f.DocumentConfguration.MaxRetryWaitTimeInSeconds);
                });
        }
    }

    public class CosmosClientFactoryTestsFixture
    {
        public IDocumentConfiguration DocumentConfguration { get; set; }
        public IDocumentClientFactory DocumentClientFactory { get; set; }

        public CosmosClientFactoryTestsFixture()
        {
            DocumentClientFactory = new CosmosClientFactory();
            DocumentConfguration = new CosmosDbConfiguration {
                Uri = "http://test.com",
                DatabaseName = "DatabaseName",
                SecurityKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                MaxRetryAttemptsOnThrottledRequests = 3,
                MaxRetryWaitTimeInSeconds = 2
            };
        }
    }
}