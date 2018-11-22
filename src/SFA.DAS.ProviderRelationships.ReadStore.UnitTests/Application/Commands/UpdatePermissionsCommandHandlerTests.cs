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

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    internal class UpdatePermissionsCommandHandlerTests : FluentTest<UpdatePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenDocumentDoesNotExist_ThenShouldCreateDocument()
        {
            return RunAsync(f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Add(It.Is<Relationship>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.AccountId == f.AccountId &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.Operations.Equals(f.Operations) &&
                        p.Created == f.Updated &&
                        p.Updated == null
                    ), null,
                    It.IsAny<CancellationToken>())));
        }
        
        [Test]
        public Task Handle_WhenDocumentExists_ThenShouldUpdateDocument()
        {
            return RunAsync(f => f.FindMatchingRelationship(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
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
        public Task Handle_WhenCommandIsNotDuplicateAndIsHandledChronologically_ThenShouldAddUpdateMessageIdToOutbox()
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
        public Task Handle_WhenCommandIsDuplicate_ThenShouldSimplyIgnoreTheUpdate()
        {
            return RunAsync(f => f.FindMatchingRelationshipWithUpdateMessageAlreadyProcessed(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Operations.Contains(Operation.CreateCohort) == false
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCommandIsNotHandledChronologically_ThenShouldSimplySwallowTheMessageAndAddItToTheOutbox()
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
    }

    internal class UpdatePermissionsCommandHandlerTestsFixture
    {
        public string CreateMessageId = "createMessageId";
        public string UpdateMessageId = "updateMessageId";
        public long Ukprn = 11111;
        public int AccountProviderId = 55555;
        public long AccountId = 333333;
        public long AccountLegalEntityId = 44444;
        public long AccountProviderLegalEntityId = 6666;
        public HashSet<Operation> Operations = new HashSet<Operation> { Operation.CreateCohort };
        public DateTime Created = DateTime.Now.AddMinutes(-10);
        public DateTime Updated = DateTime.Now.AddMinutes(-1);
        public Mock<IRelationshipsRepository> RelationshipsRepository;
        public List<Relationship> Relationships;
        public UpdatePermissionsCommand Command;
        public UpdatePermissionsCommandHandler Handler;
        
        public UpdatePermissionsCommandHandlerTestsFixture()
        {
            RelationshipsRepository = new Mock<IRelationshipsRepository>();
            Relationships = new List<Relationship>();

            RelationshipsRepository.SetupCreateQueryToReturn(Relationships);

            Handler = new UpdatePermissionsCommandHandler(RelationshipsRepository.Object);
            Command = new UpdatePermissionsCommand(AccountId, AccountLegalEntityId, AccountProviderId, AccountProviderLegalEntityId, Ukprn, Operations, Updated, UpdateMessageId);
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationship()
        {
            Relationships.Add(CreateBasicRelationship()
                .Build());
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWhichWasDeletedEarlierThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithDeleted(Updated.AddHours(-1))
                .Build());
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWhichWasDeletedLaterThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithDeleted(Updated.AddHours(1))
                .Build());
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWhichWasUpdatedLaterThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship()
                .WithUpdated(Updated.AddHours(1))
                .Build());
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWithUpdateMessageAlreadyProcessed()
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
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithOutboxMessage(new OutboxMessage(CreateMessageId, Created));
        }
    }
}
