using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.Azure.Documents.Client;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class ReadOnlyDocumentRepositoryTests : FluentTest<DocumentReadOnlyRepositoryTestsFixture>
    {
        [Test]
        public Task GetById_WhenGettingDocumentById_ThenShouldReturnDocument()
        {
            var id = Guid.NewGuid();
            
            return RunAsync(
                f => f.ArrangeDocumentDbClientToReturnASingleDocumentForSpecificGuid(id),
                f => f.ReadOnlyDocumentRepository.GetById(id),
                (f, r) => r.Should().IsSameOrEqualTo(f.Documents[0])
            );
        }

        [Test]
        public void CreateQuery_WhenCreatingQuery_ThenShouldReturnIQueryable()
        {
            Run(
                f =>  f.ArrangeDocumentDbClientToReturnIQueryableObject(),
                f => f.ReadOnlyDocumentRepository.CreateQuery(),
                (f, r) => r.Should().NotBeNull()
            );
        }

        [Test]
        public void CreateQuery_WhenCreatingQuery_ThenShouldReturnIQueryableWithFeedOptions()
        {
            Run(
                f => f.ArrangeDocumentDbClientToReturnIQueryableObject(new FeedOptions()),
                f => f.ReadOnlyDocumentRepository.CreateQuery(),
                (f, r) => r.Should().NotBeNull()
            );
        }
    }

    public class DocumentReadOnlyRepositoryTestsFixture
    {
        public string CollectionName { get; set; }
        public Mock<IDocumentDbClient<Dummy>> DocumentDbClient { get; set; }
        public ReadOnlyDocumentRepository<Dummy> ReadOnlyDocumentRepository { get; set; }
        public List<Dummy> Documents { get; set; }

        public DocumentReadOnlyRepositoryTestsFixture()
        {
            DocumentDbClient = new Mock<IDocumentDbClient<Dummy>>();

            Documents = new List<Dummy>
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

            ReadOnlyDocumentRepository = new ReadOnlyDocumentRepository<Dummy>(DocumentDbClient.Object, CollectionName);

        }

        public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentDbClientToReturnASingleDocumentForSpecificGuid(Guid guid)
        {
            DocumentDbClient.Setup(c => c.GetById(CollectionName, guid)).ReturnsAsync(Documents[0]);
            
            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentDbClientToReturnIQueryableObject()
        {
            DocumentDbClient.Setup(c => c.CreateQuery(CollectionName)).Returns(Documents.AsQueryable);
            
            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentDbClientToReturnIQueryableObject(FeedOptions options)
        {
            DocumentDbClient.Setup(c => c.CreateQuery(CollectionName, options)).Returns(Documents.AsQueryable);
            
            return this;
        }
    }
}