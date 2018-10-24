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
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelatonships.Document.Repository.UnitTests
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

        [Test]
        public Task CosmosDbClient_WhenGettingASingleDocmentWhichDoesNotExists_ThenShouldReturnNull()
        {
            return RunAsync(
                f => f.ArrangeDocumentClientToThrowNotFoundException(),
                f => f.CosmosDbClient.GetById("Collection", new Guid()),
                (f, r) => r.Should().BeNull()
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


        //public void RunCheckException(Action<CosmosDbClientTestsFixture> arrange, Action<CosmosDbClientTestsFixture> act, Func<CosmosDbClientTestsFixture, Action, AndConstraint<ObjectAssertions>> assert)
        //{
        //    var testFixture = new CosmosDbClientTestsFixture();

        //    arrange?.Invoke(testFixture);
        //    assert(testFixture, () => act.Invoke(testFixture));
        //}

        //public void RunCheckException<TException>(Action<CosmosDbClientTestsFixture> arrange, Action<CosmosDbClientTestsFixture> act, Func<CosmosDbClientTestsFixture, Action, ExceptionAssertions<TException>> assert) where TException : Exception
        //{
        //    var testFixture = new CosmosDbClientTestsFixture();

        //    arrange?.Invoke(testFixture);
        //    assert(testFixture, () => act.Invoke(testFixture));
        //}


        // This needs to go into the shared Class
        //public Task RunAsync(Action<CosmosDbClientTestsFixture> arrange, 
        //    Func<CosmosDbClientTestsFixture, Task> act, 
        //    Func<CosmosDbClientTestsFixture, Func<Task>, AndConstraint<ObjectAssertions>> assert)
        //{
        //    var testFixture = new CosmosDbClientTestsFixture();

        //    arrange?.Invoke(testFixture);

        //    assert(testFixture, async () =>
        //    {
        //        if (act != null)
        //        {
        //            await act(testFixture);
        //        }
        //    });

        //    return Task.CompletedTask;
        //}


        //public void RunCheckException(Action<CosmosDbClientTestsFixture> arrange,
        //    Action<CosmosDbClientTestsFixture> act,
        //    Action<CosmosDbClientTestsFixture, Func<Action>> assert)
        //{

        //    if (act == null) throw new ArgumentNullException(nameof(act));
        //    if (assert == null) throw new ArgumentNullException(nameof(assert));

        //    var testFixture = new CosmosDbClientTestsFixture();

        //    arrange?.Invoke(testFixture);

        //    assert(testFixture, act() =>
        //    {
        //        act.Invoke(testFixture);
        //        return;
        //    });

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

    public class CosmosDbClientTestsFixture
    {
        public Mock<IDocumentClientFactory> DocumentClientFactory { get; set; }
        public Mock<IDocumentClient> DocumentClient { get; set; }
        public IDocumentConfiguration DocumentConfguration { get; set; }
        public CosmosDbClient<Dummy> CosmosDbClient { get; set; }

        public List<Dummy> ListOfItems;

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

        public CosmosDbClientTestsFixture ArrangeDocumentClientToThrowNotFoundException()
        {
            DocumentClientException excepton = CosmosDbHelper.CreateDocumentClientExceptionForTesting(new Error(), HttpStatusCode.NotFound);
            DocumentClient.Setup(x => x.ReadDocumentAsync(It.IsAny<Uri>(), null, CancellationToken.None)).Throws(excepton);
            return this;
        }

        public CosmosDbClientTestsFixture ArrangeDocumentClientToReturnAListOfObjects()
        {
            Expression<Func<Dummy, bool>> predicate = t => true;

            var dataSource = ListOfItems.AsQueryable();
            var expected = dataSource.Where(predicate);

            var response = expected.ToFeedResponse();

            var mockDocumentQuery = new Mock<IFakeDocumentQuery<Dummy>>();
            mockDocumentQuery
                .SetupSequence(_ => _.HasMoreResults)
                .Returns(true)
                .Returns(false);

            mockDocumentQuery
                .Setup(_ => _.ExecuteNextAsync<Dummy>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var provider = new Mock<IQueryProvider>();
            provider
                .Setup(_ => _.CreateQuery<Dummy>(It.IsAny<Expression>()))
                .Returns(mockDocumentQuery.Object);

            mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.Provider).Returns(provider.Object);
            mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.Expression).Returns(dataSource.Expression);
            mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.ElementType).Returns(dataSource.ElementType);
            mockDocumentQuery.As<IQueryable<Dummy>>().Setup(x => x.GetEnumerator()).Returns(dataSource.GetEnumerator);

            DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(),
                    It.IsAny<FeedOptions>()))
                .Returns(mockDocumentQuery.Object);

            return this;
        }

        public CosmosDbClientTestsFixture ArrangeDocumentClientToThrowNotFoundExceptionWhenSearchingForDocuments()
        {
            DocumentClientException excepton = CosmosDbHelper.CreateDocumentClientExceptionForTesting(new Error(), HttpStatusCode.NotFound);
            DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(), It.IsAny<FeedOptions>())).Throws(excepton);
            return this;
        }

        public CosmosDbClientTestsFixture ArrangeDocumentClientToThrowBadRequestExceptionWhenSearchingForDocuments()
        {
            DocumentClientException excepton = CosmosDbHelper.CreateDocumentClientExceptionForTesting(new Error {
                Message = "Cross partition query is required but disabled. Please....."
            }, HttpStatusCode.BadRequest);
            DocumentClient.Setup(x => x.CreateDocumentQuery<Dummy>(It.IsAny<Uri>(), It.IsAny<FeedOptions>())).Throws(excepton);
            return this;
        }
    }

    public class Dummy
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


}