using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Stubs;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class DocumentRepositoryTests : FluentTest<DocumentReadOnlyRepositoryTestsFixture>
    {
        [Test]
        public Task GetById_WhenDocumentExists_ThenShouldReturnDocument()
        {
            return RunAsync(f => f.SetDocument(), f => f.DocumentRepository.GetById(f.Document.Id), (f, r) => r.Should().IsSameOrEqualTo(f.Documents[0]));
        }

        [Test]
        public Task GetById_WhenDocumentDoesNotExist_ThenShouldReturnNull()
        {
            return RunAsync(f => f.SetDocumentNotFound(), f => f.DocumentRepository.GetById(f.Document.Id), (f, r) => r.Should().BeNull());
        }

        [Test]
        public void CreateQuery_WhenCreatingQuery_ThenShouldReturnIQueryable()
        {
            Run(f =>  f.SetDocuments(), f => f.DocumentRepository.CreateQuery(), (f, r) => r.Should().NotBeNull());
        }

        [Test]
        public void CreateQuery_WhenCreatingQueryWithFeedOptions_ThenShouldReturnIQueryableWithFeedOptions()
        {
            Run(f => f.SetDocuments(new FeedOptions()), f => f.DocumentRepository.CreateQuery(f.FeedOptions), (f, r) => r.Should().NotBeNull());
        }
    }

    public class DocumentReadOnlyRepositoryTestsFixture
    {
        public DocumentRepository<DocumentStub> DocumentRepository { get; set; }
        public Mock<IDocumentClient> DocumentClient { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public DocumentStub Document { get; set; }
        public List<DocumentStub> Documents { get; set; }
        public FeedOptions FeedOptions { get; set; }

        public DocumentReadOnlyRepositoryTestsFixture()
        {
            DocumentClient = new Mock<IDocumentClient>();
            DatabaseName = "test";
            CollectionName = "stubs";
            DocumentRepository = new DocumentRepositoryStub(DocumentClient.Object, DatabaseName, CollectionName);
            
            Document = new DocumentStub
            {
                Id = Guid.NewGuid(),
                Name = "Test"
            };
            
            Documents = new List<DocumentStub>
            {
                new DocumentStub
                {
                    Id = Guid.NewGuid(),
                    Name = "TestA"
                },
                new DocumentStub
                {
                    Id = Guid.NewGuid(),
                    Name = "TestB"
                }
            };
        }

        public DocumentReadOnlyRepositoryTestsFixture SetDocument()
        {
            DocumentClient.Setup(c => c.ReadDocumentAsync<DocumentStub>(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, Document.Id.ToString()), null, CancellationToken.None))
                .ReturnsAsync(new DocumentResponse<DocumentStub>(Document));
            
            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture SetDocumentNotFound()
        {
            DocumentClient.Setup(c => c.ReadDocumentAsync<DocumentStub>(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, Document.Id.ToString()), null, CancellationToken.None))
                .ThrowsAsync(DocumentClientExceptionBuilder.Build(new Error(), HttpStatusCode.NotFound));

            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture SetDocuments(FeedOptions feedOptions = null)
        {
            FeedOptions = feedOptions;
            
            DocumentClient.Setup(c => c.CreateDocumentQuery<DocumentStub>(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), FeedOptions))
                .Returns(Documents.AsQueryable().OrderBy(d => d.Id));
            
            return this;
        }
    }
}