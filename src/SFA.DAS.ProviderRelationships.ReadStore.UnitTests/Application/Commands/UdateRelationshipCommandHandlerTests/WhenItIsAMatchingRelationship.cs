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
    internal class WhenItIsAMatchingRelationship : FluentTest<WhenItIsAMatchingRelationshipFixture>
    {
        [Test]
        public Task Handle_AValidCommand_ThenShouldUpdateExistingDocumentOperations()
        {
            return RunAsync(f => f.FindMatchingRelationship(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.AccountId == f.AccountId &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.Operations.Equals(f.UpdateOperations) &&
                        p.Updated == f.Updated
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_AValidCommand_ThenShouldAddUpdateMessageIdToOutbox()
        {
            return RunAsync(f => f.FindMatchingRelationship(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdateMessageId) == 1
                    ), null,

                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_ADuplicateMessageId_ThenShouldSimplyIgnoreTheUpdate()
        {
            return RunAsync(f => f.FindMatchingRelationshipWithUpdateMessageAlreadyProcessed(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Operations.Contains(Operation.CreateCohort) == false
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_AnOldMessageId_ThenShouldSimplySwallowTheMessageAndAddItToTheOutbox()
        {
            return RunAsync(f => f.FindMatchingRelationshipWhichWasUpdatedLaterThanNewMessage(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Operations.Contains(Operation.CreateCohort) == false &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdateMessageId) == 1
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_AnOldMessageIdToADeletedRelationship_ThenShouldSimplySwallowTheMessageAndAddItToTheOutbox()
        {
            return RunAsync(f => f.FindMatchingRelationshipWhichWasDeletedLaterThanNewMessage(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Operations.Contains(Operation.CreateCohort) == false &&
                        p.Deleted != null &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdateMessageId) == 1
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_ANewerMessageToADeletedRelationship_ThenShouldUndeleteAndAddItToTheOutbox()
        {
            return RunAsync(f => f.FindMatchingRelationshipWhichWasDeletedEarlierThanNewMessage(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Operations.Contains(Operation.CreateCohort) &&
                        p.Deleted == null &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdateMessageId) == 1
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

    }

    internal class WhenItIsAMatchingRelationshipFixture
    {
        public string CreateMessageId = "createMessageId";
        public string UpdateMessageId = "updateMessageId";
        public long Ukprn = 11111;
        public int AccountProviderId = 55555;
        public long AccountId = 333333;
        public long AccountLegalEntityId = 44444;
        public HashSet<Operation> UpdateOperations = new HashSet<Operation> { Operation.CreateCohort };
        public DateTime Created = DateTime.Now.AddMinutes(-10);
        public DateTime Updated = DateTime.Now.AddMinutes(-1);

        public Mock<IRelationshipsRepository> RelationshipsRepository;
        public List<Relationship> Relationships;

        public UpdateRelationshipCommand Command;
        public UpdateRelationshipCommandHandler Handler;

        public WhenItIsAMatchingRelationshipFixture()
        {
            RelationshipsRepository = new Mock<IRelationshipsRepository>();
            Relationships = new List<Relationship>();

            RelationshipsRepository.SetupCosmosCreateQueryToReturn(Relationships);

            Handler = new UpdateRelationshipCommandHandler(RelationshipsRepository.Object);

            Command = new UpdateRelationshipCommand(Ukprn, AccountProviderId, AccountId, AccountLegalEntityId, 
                UpdateOperations, UpdateMessageId, Updated);
        }

        public WhenItIsAMatchingRelationshipFixture FindMatchingRelationship()
        {
            Relationships.Add(CreateBasicRelationship()
                .Build());
            return this;
        }

        public WhenItIsAMatchingRelationshipFixture FindMatchingRelationshipWhichWasDeletedEarlierThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithDeleted(Updated.AddHours(-1))
                .Build());
            return this;
        }

        public WhenItIsAMatchingRelationshipFixture FindMatchingRelationshipWhichWasDeletedLaterThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithDeleted(Updated.AddHours(1))
                .Build());
            return this;
        }

        public WhenItIsAMatchingRelationshipFixture FindMatchingRelationshipWhichWasUpdatedLaterThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithUpdated(Updated.AddHours(1))
                .Build());
            return this;
        }

        public WhenItIsAMatchingRelationshipFixture FindMatchingRelationshipWithUpdateMessageAlreadyProcessed()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithOutboxMessage(new OutboxMessage(UpdateMessageId, Updated))
                .Build());
            return this;
        }

        private RelationshipBuilder CreateBasicRelationship()
        {
            return new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderId(AccountProviderId)
                .WithAccountId(AccountId)
                .WithAccountLegalEntityId(AccountLegalEntityId)
                .WithOutboxMessage(new OutboxMessage(CreateMessageId, Created));
        }
    }
}
