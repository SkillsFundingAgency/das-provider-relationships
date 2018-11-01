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
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class ReadOnlyDocumentRepositoryTests : FluentTest<DocumentReadOnlyRepositoryTestsFixture>
    {
        [Test]
        public Task DocumentReadOnlyRepository_WhenGettingASingleDocment_ThenShouldReturnObject()
        {
            var id = Guid.NewGuid();
            return RunAsync(
                f => f.ArrangeDocumentDbClientToReturnASingleDocumentForSpecificGuid(id),
                f => f.ReadOnlyDocumentRepository.GetById(id),
                (f, r) => r.Should().IsSameOrEqualTo(f.ListOfItems[0])
            );
        }

        [Test]
        public void DocumentReadOnlyRepository_WhenGettingANewQuery_ThenItShouldReturnIQueryableObject()
        {
            Run(
                f =>  f.ArrangeDocumentDbClientToReturnIQueryableObject(),
                f => f.ReadOnlyDocumentRepository.CreateQuery(),
                (f, r) => r.Should().NotBeNull()
            );
        }

        [Test]
        public void DocumentReadOnlyRepository_WhenGettingANewQuery_ThenItShouldReturnIQueryableObjectWithFeedOption()
        {
            Run(
                f => f.ArrangeDocumentDbClientToReturnIQueryableObject(new FeedOptions()),
                f => f.ReadOnlyDocumentRepository.CreateQuery(),
                (f, r) => r.Should().NotBeNull()
            );
        }

        //[Test]
        //public Task DocumentReadOnlyRepository_WhenExecutingAQuery_ThenItShouldReturnTheListOfItems()
        //{
        //    return RunAsync(
        //        f => f.ArrangeDocumentDbClientToReturnOneSetOfMatchingObjects(f.ListOfItems),
        //        f => f.ReadOnlyDocumentRepository.ExecuteQuery(f.ListOfItems.AsQueryable(), CancellationToken.None),
        //        (f, r) => r.Should().BeEquivalentTo(f.ListOfItems)
        //    );
        //}


        // Move to Shared Package
        public Task RunAsyncCheckException(Func<CosmosDbClientTestsFixture, Task> act,
            Action<CosmosDbClientTestsFixture, Func<Task>> assert)
        {
            return RunAsyncCheckException(null, act, assert);
        }

        public Task RunAsyncCheckException(Action<CosmosDbClientTestsFixture> arrange,
            Func<CosmosDbClientTestsFixture, Task> act,
            Action<CosmosDbClientTestsFixture, Func<Task>> assert)
        {
            var testFixture = new CosmosDbClientTestsFixture();

            arrange?.Invoke(testFixture);

            assert(testFixture, async () => await act(testFixture));

            return Task.CompletedTask;
        }
    }

    public class DocumentReadOnlyRepositoryTestsFixture
    {
        public string Collection { get; set; }
        public Mock<IDocumentDbClient<Dummy>> DocumentDbClient { get; set; }
        public Mock<IDocumentQueryWrapper<Dummy>> CosmosQueryWrapper { get; set; }
        //public Mock<IDocumentQuery<Dummy>> MockedDocumentQuery { get; set; }
        public ReadOnlyDocumentRepository<Dummy> ReadOnlyDocumentRepository { get; set; }
        public List<Dummy> ListOfItems;

        public DocumentReadOnlyRepositoryTestsFixture()
        {
            DocumentDbClient = new Mock<IDocumentDbClient<Dummy>>();
            //MockedDocumentQuery = new Mock<IDocumentQuery<Dummy>>();
            CosmosQueryWrapper = new Mock<IDocumentQueryWrapper<Dummy>>();

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


            ReadOnlyDocumentRepository = new ReadOnlyDocumentRepository<Dummy>(DocumentDbClient.Object, Collection);

        }

        public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentDbClientToReturnASingleDocumentForSpecificGuid(Guid guid)
        {
            DocumentDbClient.Setup(x => x.GetById(Collection, guid)).ReturnsAsync(ListOfItems[0]);
            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentDbClientToReturnIQueryableObject()
        {
            DocumentDbClient.Setup(x => x.CreateQuery(Collection)).Returns(ListOfItems.AsQueryable);
            return this;
        }

        public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentDbClientToReturnIQueryableObject(FeedOptions options)
        {
            DocumentDbClient.Setup(x => x.CreateQuery(Collection, options)).Returns(ListOfItems.AsQueryable);
            return this;
        }

        public void ArrangeDocumentDbClientToReturnOneSetOfMatchingObjects(List<Dummy> list)
        {
            CosmosQueryWrapper.Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(list.AsEnumerable);

            //MockedDocumentQuery.SetupSequence(x => x.HasMoreResults).Returns(true).Returns(false);

            //DocumentDbClient.Setup(x => x.GetEntities(MockedDocumentQuery.Object, CancellationToken.None))
            //    .ReturnsAsync(list.AsEnumerable());
        }

    }
}