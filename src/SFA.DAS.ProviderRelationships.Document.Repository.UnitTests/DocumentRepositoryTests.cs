﻿using System;
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
        public void Add_WhenAddingDocument_ThenShouldAddDocument()
        {
            Run(f => f.Add(), f => f.DocumentClient.Verify(c => c.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(f.DatabaseName, f.CollectionName), f.Document, f.RequestOptions, false, CancellationToken.None), Times.Once));
        }
        
        [Test]
        public void CreateQuery_WhenCreatingQuery_ThenShouldReturnIQueryable()
        {
            Run(f => f.SetDocuments(), f => f.CreateQuery(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.DocumentsQuery));
        }

        [Test]
        public void CreateQuery_WhenCreatingQueryWithFeedOptions_ThenShouldReturnIQueryableWithFeedOptions()
        {
            Run(f => f.SetDocuments(), f => f.CreateQueryWithFeedOptions(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.DocumentsQuery));
        }
        
        [Test]
        public Task GetById_WhenDocumentExists_ThenShouldReturnDocument()
        {
            return RunAsync(f => f.SetDocument(), f => f.GetById(), (f, r) => r.Should().IsSameOrEqualTo(f.Document));
        }
        
        [Test]
        public Task GetById_WhenDocumentExistsWithRequestOptions_ThenShouldReturnDocument()
        {
            return RunAsync(f => f.SetDocument(), f => f.GetByIdWithRequestOptions(), (f, r) => r.Should().IsSameOrEqualTo(f.Document));
        }

        [Test]
        public Task GetById_WhenDocumentDoesNotExist_ThenShouldReturnNull()
        {
            return RunAsync(f => f.SetDocumentNotFound(), f => f.GetById(), (f, r) => r.Should().BeNull());
        }

        [Test]
        public void Remove_WhenRemovingDocument_ThenShouldRemoveDocument()
        {
            Run(f => f.Remove(), f => f.DocumentClient.Verify(c => c.DeleteDocumentAsync(UriFactory.CreateDocumentUri(f.DatabaseName, f.CollectionName, f.Document.Id.ToString()), f.RequestOptions, CancellationToken.None), Times.Once));
        }

        [Test]
        public void Update_WhenUpdatingDocument_ThenShouldUpdateDocument()
        {
            Run(f => f.Update(), f => f.DocumentClient.Verify(c => c.ReplaceDocumentAsync(UriFactory.CreateDocumentCollectionUri(f.DatabaseName, f.CollectionName), f.Document, f.RequestOptions, CancellationToken.None), Times.Once));
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
        public IOrderedQueryable<DocumentStub> DocumentsQuery { get; set; }
        public FeedOptions FeedOptions { get; set; }
        public RequestOptions RequestOptions { get; set; }

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

            DocumentsQuery = Documents.AsQueryable().OrderBy(d => d.Id);
        }

        public IQueryable<DocumentStub> CreateQuery()
        {
            return DocumentRepository.CreateQuery();
        }

        public IQueryable<DocumentStub> CreateQueryWithFeedOptions()
        {
            FeedOptions = new FeedOptions();
            
            return DocumentRepository.CreateQuery(FeedOptions);
        }

        public Task<DocumentStub> GetById()
        {
            return DocumentRepository.GetById(Document.Id);
        }

        public Task<DocumentStub> GetByIdWithRequestOptions()
        {
            RequestOptions = new RequestOptions();
            
            return DocumentRepository.GetById(Document.Id, RequestOptions);
        }

        public Task Add()
        {
            return DocumentRepository.Add(Document, RequestOptions);
        }

        public Task Remove()
        {
            return DocumentRepository.Remove(Document.Id, RequestOptions);
        }

        public Task Update()
        {
            return DocumentRepository.Update(Document, RequestOptions);
        }

        public DocumentReadOnlyRepositoryTestsFixture SetDocument()
        {
            DocumentClient.Setup(c => c.ReadDocumentAsync<DocumentStub>(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, Document.Id.ToString()), It.Is<RequestOptions>(r => r == RequestOptions), CancellationToken.None))
                .ReturnsAsync(new DocumentResponse<DocumentStub>(Document));
            
            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture SetDocumentNotFound()
        {
            DocumentClient.Setup(c => c.ReadDocumentAsync<DocumentStub>(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, Document.Id.ToString()), It.Is<RequestOptions>(r => r == RequestOptions), CancellationToken.None))
                .ThrowsAsync(DocumentClientExceptionBuilder.Build(new Error(), HttpStatusCode.NotFound));

            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture SetDocuments()
        {
            DocumentClient.Setup(c => c.CreateDocumentQuery<DocumentStub>(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), It.Is<FeedOptions>(f => f == FeedOptions)))
                .Returns(DocumentsQuery);
            
            return this;
        }
    }
}