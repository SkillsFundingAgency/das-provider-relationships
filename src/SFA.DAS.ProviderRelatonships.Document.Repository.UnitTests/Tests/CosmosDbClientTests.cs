using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests.Tests
{
    [TestFixture]
    public class CosmosDbClientTests : FluentTest<CosmosDbClientTestsFixture>
    {
        [Test]
        public Task CosmosDbClient_WhenGettingASingleDocmentWhichExists_ThenShouldReturnObject()
        {
            return RunAsync(
                f => f.ArrangeDocumentClientToReturnASingleDocument(),
                f => f.CosmosDbClient.GetById("Collection", new Guid()),
                (f, r) => r.Should().NotBeNull()
            );
        }

        [Ignore("Creatng the exception DocumentClientException via reflection requires System.Net.Http 4.1.0 and fails")]
        [Test]
        public Task CosmosDbClient_WhenGettingASingleDocmentWhichDoesNotExists_ThenShouldReturnNull()
        {
            return RunAsync(
                f => f.ArrangeDocumentClientToThrowNotFoundException(),
                f => f.CosmosDbClient.GetById("Collection", new Guid()),
                (f, r) => r.Should().BeNull()
            );
        }

        [Test]
        public void CosmosDbClient_WhenCreatingAQuery_ThenShouldReturnQueryableInterface()
        {
            Run(
                f =>  f.ArrangeDocumentClientToReturnOrderedQueryableInterface(),
                f => f.CosmosDbClient.CreateQuery("Collection"),
                (f, r) => r.Should().NotBeNull()
            );
        }
    }

    public class CosmosDbClientTestsFixture
    {
        public Mock<IDocumentClientFactory> DocumentClientFactory { get; set; }
        public Mock<IDocumentClient> DocumentClient { get; set; }
        public IDocumentConfiguration DocumentConfguration { get; set; }
        public IDocumentDbClient<Dummy> CosmosDbClient { get; set; }

        public List<Dummy> ListOfItems;

        public IOrderedQueryable<Dummy> OrderedQuery{ get; set; }


        public CosmosDbClientTestsFixture()
        {
            DocumentClientFactory = new Mock<IDocumentClientFactory>();
            DocumentConfguration = new CosmosDbConfiguration {
                Uri = "Uri",
                DatabaseName = "DatabaseName",
                SecurityKey = "SecurityKey"
            };

            ListOfItems = new List<Dummy>
            {
                new Dummy
                {
                    Id = Guid.NewGuid(),
                    Name = "TestA"
                },
                new Dummy
                {
                    Id = Guid.NewGuid(),
                    Name = "TestB"
                }
            };

            OrderedQuery = ListOfItems.AsQueryable() as IOrderedQueryable<Dummy>;

            DocumentClient = new Mock<IDocumentClient>();
            DocumentClientFactory.Setup(x => x.Create(DocumentConfguration)).Returns(DocumentClient.Object);
            CosmosDbClient = new CosmosDbClient<Dummy>(DocumentClientFactory.Object, DocumentConfguration);

        }

        public CosmosDbClientTestsFixture ArrangeDocumentClientToReturnASingleDocument()
        {
            var document = new Microsoft.Azure.Documents.Document();
            var resourcce = new ResourceResponse<Microsoft.Azure.Documents.Document>(document);
            DocumentClient.Setup(x => x.ReadDocumentAsync(It.IsAny<Uri>(), null, CancellationToken.None)).ReturnsAsync(resourcce);
            return this;
        }

        public void ArrangeDocumentClientToReturnOrderedQueryableInterface()
        {
            DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(),
                    It.IsAny<FeedOptions>()))
                .Returns(OrderedQuery);
        }

        public void ArrangeDocumentClientToThrowNotFoundException()
        {
            var e = CosmosDbHelper.CreateDocumentClientExceptionForTesting(new Error(), HttpStatusCode.NotFound);
            DocumentClient.Setup(x => x.ReadDocumentAsync(It.IsAny<Uri>(), It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>())).ThrowsAsync(e);
        }
    }
}