using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application.Commands.UdateRelationshipCommandHandlerTests
{
    internal class WhenItIsANewRelationship : FluentTest<WhenItIsANewRelationshipFixture>
    {
        [Test]
        public Task Handle_ThenShouldAddNewDocumentToRepository()
        {
            return RunAsync(f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Add(It.Is<Relationship>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.AccountId == f.AccountId &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.Operations.Equals(f.Operations) &&
                        p.Updated == f.Updated
                    ), null,
                    It.IsAny<CancellationToken>())));
        }
        [Test]
        public Task Handle_ThenShouldAddMessageIdToOutbox()
        {
            return RunAsync(f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Add(It.Is<Relationship>(p =>
                        p.OutboxData.Count(o=>o.MessageId == f.MessageId) == 1
                    ), null,
                    It.IsAny<CancellationToken>())));
        }
    }

    internal class WhenItIsANewRelationshipFixture
    {
        public string MessageId = "messageId";
        public long Ukprn = 11111;
        public int AccountProviderId = 55555;
        public long AccountId = 333333;
        public long AccountLegalEntityId = 44444;
        public HashSet<Operation> Operations = new HashSet<Operation> { Operation.CreateCohort };
        public DateTime Updated = DateTime.Now.AddMinutes(-1);

        public Mock<IRelationshipsRepository> RelationshipsRepository;

        public UpdateRelationshipCommand Command;
        public UpdatedRelationshipCommandHandler Handler;

        public WhenItIsANewRelationshipFixture()
        {
            RelationshipsRepository = new Mock<IRelationshipsRepository>();
            RelationshipsRepository.SetupCosmosCreateQueryToReturn(new List<Relationship>());

            Handler = new UpdatedRelationshipCommandHandler(RelationshipsRepository.Object);

            Command = new UpdateRelationshipCommand(Ukprn, AccountProviderId, AccountId, AccountLegalEntityId, 
                Operations, MessageId, Updated);
        }
    }
}
