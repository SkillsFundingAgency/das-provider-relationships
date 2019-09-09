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
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Commands.Ping;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.Client.UnitTests.ReadStore.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class PingCommandHandlerTests : FluentTest<PingCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenDatabasePingFails_ThenShouldThrowException()
        {
            return RunAsync(
                f => f.SetPingFailure(),
                f => f.Handle(),
                (f, r) => r.Should().Throw<Exception>().WithMessage("Read store database ping failed"));
        }
    }

    public class PingCommandHandlerTestsFixture
    {
        internal PingCommand Command { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public Mock<IDocumentClient> DocumentClient { get; set; }
        internal IRequestHandler<PingCommand, Unit> Handler { get; set; }
        public Mock<IDocumentClientFactory> DocumentClientFactory { get; set; }
        public List<Database> Databases { get; set; }

        public PingCommandHandlerTestsFixture()
        {
            DocumentClient = new Mock<IDocumentClient>();
            Command = new PingCommand();
            CancellationToken = CancellationToken.None;
            DocumentClientFactory = new Mock<IDocumentClientFactory>();
            
            DocumentClientFactory.Setup(f => f.CreateDocumentClient()).Returns(DocumentClient.Object);
            
            Handler = new PingCommandHandler(DocumentClientFactory.Object);
            Databases = new List<Database> { new Database { Id = DocumentSettings.DatabaseName }, new Database() };
            
            DocumentClient.Setup(c => c.CreateDatabaseQuery(null)).Returns(Databases.AsQueryable().OrderBy(d => d.Id));
        }

        public Task Handle()
        {
            return Handler.Handle(Command, CancellationToken);
        }

        public PingCommandHandlerTestsFixture SetPingFailure()
        {
            Databases.Clear();
            
            return this;
        }
    }
}