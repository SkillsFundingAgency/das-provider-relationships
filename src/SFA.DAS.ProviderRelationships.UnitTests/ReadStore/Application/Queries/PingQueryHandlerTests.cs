using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Azure.Documents;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries.Ping;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.ReadStore.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class PingQueryHandlerTests : FluentTest<PingQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenDatabasePingFails_ThenShouldThrowException()
        {
            return TestExceptionAsync(
                f => f.SetPingFailure(),
                f => f.Handle(),
                (f, r) => r.Should().ThrowAsync<Exception>().WithMessage("Read store database ping failed"));
        }
    }

    public class PingQueryHandlerTestsFixture
    {
        internal PingQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public Mock<IDocumentClient> DocumentClient { get; set; }
        internal IRequestHandler<PingQuery> Handler { get; set; }
        public Mock<IDocumentClientFactory> DocumentClientFactory { get; set; }
        public List<Database> Databases { get; set; }

        public PingQueryHandlerTestsFixture()
        {
            DocumentClient = new Mock<IDocumentClient>();
            Query = new PingQuery();
            CancellationToken = CancellationToken.None;
            DocumentClientFactory = new Mock<IDocumentClientFactory>();
            
            DocumentClientFactory.Setup(f => f.CreateDocumentClient()).Returns(DocumentClient.Object);
            
            Handler = new PingQueryHandler(DocumentClientFactory.Object);
            Databases = new List<Database> { new Database { Id = DocumentSettings.DatabaseName }, new Database() };
            
            DocumentClient.Setup(c => c.CreateDatabaseQuery(null)).Returns(Databases.AsQueryable().OrderBy(d => d.Id));
        }

        public Task Handle()
        {
            return Handler.Handle(Query, CancellationToken);
        }

        public PingQueryHandlerTestsFixture SetPingFailure()
        {
            Databases.Clear();
            
            return this;
        }
    }
}