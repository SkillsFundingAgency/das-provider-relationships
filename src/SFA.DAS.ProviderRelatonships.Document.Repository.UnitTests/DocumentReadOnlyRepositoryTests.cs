using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests
{
    [TestFixture]
    public class DocumentReadOnlyRepositoryTests : FluentTest<DocumentReadOnlyRepositoryTestsFixture>
    {
        [Test]
        public Task DocumentReadOnlyRepository_WhenGettingASingleDocment_ThenShouldReturnObject()
        {
            Guid id = Guid.NewGuid();
            return RunAsync(
                f => f.ArrangeDocumentDbClientToReturnASingleDocumentForSpecificGuid(id),
                f => f.DocumentReadOnlyRepository.GetById(id),
                (f, r) => r.Should().IsSameOrEqualTo(f.ListOfItems[0])
            );
        }

        //[Test]
        //public Task CosmosDbClient_WhenGettingASingleDocmentWhichDoesNotExists_ThenShouldReturnNull()
        //{
        //    return RunAsync(
        //        f => f.ArrangeDocumentClientToThrowNotFoundException(),
        //        f => f.CosmosDbClient.GetById("Collection", new Guid()),
        //        (f, r) => r.Should().BeNull()
        //    );
        //}

        [Test]
        public void DocumentReadOnlyRepository_WhenGettingANewQuery_ThenItShouldReturnIQueryableObject()
        {
            Run(
                f =>  f.ArrangeDocumentDbClientToReturnIQueryableObject(),
                f => f.DocumentReadOnlyRepository.CreateQuery(),
                (f, r) => r.Should().NotBeNull()
            );
        }

        [Test]
        public void DocumentReadOnlyRepository_WhenGettingANewQuery_ThenItShouldReturnIQueryableObjectWithFeedOption()
        {
            Run(
                f => f.ArrangeDocumentDbClientToReturnIQueryableObject(new FeedOptions()),
                f => f.DocumentReadOnlyRepository.CreateQuery(),
                (f, r) => r.Should().NotBeNull()
            );
        }

        [Test]
        public void DocumentReadOnlyRepository_WhenExecutingAQuery_ThenItShouldReturnIQueryableObjectWithFeedOption()
        {
            Run(
                f => f.ArrangeDocumentDbClientToReturnOneSetOfMatchingObjects(f.ListOfItems),
                f => f.DocumentReadOnlyRepository.ExecuteQuery(f.ListOfItems.AsQueryable(), CancellationToken.None),
                (f, r) => r.Should().NotBeNull()
            );
        }


        //[Test]
        //public Task CosmosDbClient_WhenSearchingForObjectsInNoneExistentPartition_ThenShouldThrowDocumentException()
        //{
        //    return RunAsyncCheckException(
        //        f => f.ArrangeDocumentClientToThrowNotFoundExceptionWhenSearchingForDocuments(),
        //        f => f.CosmosDbClient.Search("Collection", _ => true),
        //        (f, r) => r.Should().Throw<DocumentException>().And.HttpStatusCode.Should().Be(HttpStatusCode.NotFound)
        //    );
        //}

        //[Test]
        //public Task CosmosDbClient_WhenSearchingForObjectsAccrossMultiplePartition_ThenShouldThrowDocumentException()
        //{
        //    return RunAsyncCheckException(
        //        f => f.ArrangeDocumentClientToThrowBadRequestExceptionWhenSearchingForDocuments(),
        //        f => f.CosmosDbClient.Search("Collection", _ => true),
        //        (f, r) => r.Should().Throw<DocumentException>()//.And.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest)
        //       );
        //}


        //[Test]
        //public Task CosmosDbClient_WhenSearchngForExistingDocuments_ThenShouldReturnAListOfObjects()
        //{
        //    return RunAsync(
        //        f => f.ArrangeDocumentClientToReturnAListOfObjects(),
        //        f => f.CosmosDbClient.Search("Collection", _ => true),
        //        (f, r) => r.IsSameOrEqualTo(f.ListOfItems)
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
        public Mock<IDocumentQuery<Dummy>> MockedDocumentQuery { get; set; }
        public DocumentReadOnlyRepository<Dummy> DocumentReadOnlyRepository { get; set; }
        public List<Dummy> ListOfItems;

        public DocumentReadOnlyRepositoryTestsFixture()
        {
            DocumentDbClient = new Mock<IDocumentDbClient<Dummy>>();
            MockedDocumentQuery = new Mock<IDocumentQuery<Dummy>>();

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


            DocumentReadOnlyRepository = new DocumentReadOnlyRepository<Dummy>(DocumentDbClient.Object, Collection);

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


        //public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentClientToReturnAListOfObjects()
        //{
        //    Expression<Func<Dummy, bool>> predicate = t => true;

        //    var dataSource = ListOfItems.AsQueryable();
        //    var expected = dataSource.Where(predicate);

        //    var response = expected.ToFeedResponse();

        //    var mockDocumentQuery = new Mock<IFakeDocumentQuery<Dummy>>();
        //    mockDocumentQuery
        //        .SetupSequence(_ => _.HasMoreResults)
        //        .Returns(true)
        //        .Returns(false);

        //    mockDocumentQuery
        //        .Setup(_ => _.ExecuteNextAsync<Dummy>(It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(response);

        //    var provider = new Mock<IQueryProvider>();
        //    provider
        //        .Setup(_ => _.CreateQuery<Dummy>(It.IsAny<Expression>()))
        //        .Returns(mockDocumentQuery.Object);

        //    mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.Provider).Returns(provider.Object);
        //    mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.Expression).Returns(dataSource.Expression);
        //    mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.ElementType).Returns(dataSource.ElementType);
        //    mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.GetEnumerator()).Returns(dataSource.GetEnumerator);

        //    DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(),
        //            It.IsAny<FeedOptions>()))
        //        .Returns(mockDocumentQuery.Object);

        //    return this;
        //}

        //public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentClientToThrowNotFoundExceptionWhenSearchingForDocuments()
        //{
        //    DocumentClientException excepton = CosmosDbHelper.CreateDocumentClientExceptionForTesting(new Error(), HttpStatusCode.NotFound);
        //    DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(), It.IsAny<FeedOptions>())).Throws(excepton);
        //    return this;
        //}

        //public DocumentReadOnlyRepositoryTestsFixture ArrangeDocumentClientToThrowBadRequestExceptionWhenSearchingForDocuments()
        //{
        //    DocumentClientException excepton = CosmosDbHelper.CreateDocumentClientExceptionForTesting(new Error {
        //        Message = "Cross partition query is required but disabled. Please....."
        //    }, HttpStatusCode.BadRequest);
        //    DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(), It.IsAny<FeedOptions>())).Throws(excepton);
        //    return this;
        //}

        //public void ArrangeDocumentClientToReturnOrderedQueryableInterface()
        //{
        //    DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(),
        //            It.IsAny<FeedOptions>()))
        //        .Returns(OrderedQuery);
        //}

        public void ArrangeDocumentDbClientToReturnOneSetOfMatchingObjects(List<Dummy> list)
        {
            DocumentDbClient.Setup(x => x.ConvertToDocumentQuery(list.AsQueryable()))
                .Returns(MockedDocumentQuery.Object);

            MockedDocumentQuery.SetupSequence(x => x.HasMoreResults).Returns(true).Returns(false);
            //MockedDocumentQuery.Setup(x=>x.ExecuteNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(list)
            //throw new NotImplementedException();
        }
    }

    public class AnotherDummy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


}