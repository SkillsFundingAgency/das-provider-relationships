using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    internal class AccountProviderLegalEntityDeletedEventHandlerTests : FluentTest<AccountProviderLegalEntityDeletedEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenNoRelationshipIsFound_ThenThrowException()
        {
            return RunAsync(
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                (f,r) => r.Should().Throw<Exception>());
        }

        [Test]
        public Task Handle_WhenDeletes_ThenRelationshipShouldHaveNoOperationsAndSetDeletedDate()
        {
            return RunAsync(f => f.AddMatchingRelationship().SetMessageIdInContext(f.DeletedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Deleted == f.Message.Created &&
                        p.Operations.Any() == false
                    )
                    , null, It.IsAny<CancellationToken>())));
        }
        [Test]
        public Task Handle_WhenDeletes_ThenTheRelationshipShouldAddMessageToOutbox()
        {
            return RunAsync(f => f.AddMatchingRelationship().SetMessageIdInContext(f.DeletedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.DeletedMessageId && o.Created == f.Message.Created)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenDeleteIsCalledWithADuplicateMessage_ThenTheRelationshipShouldMakeNoChanges()
        {
            return RunAsync(f => f.AddMatchingRelationship().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p => 
                        p.OutboxData.Count() == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenDeleteOnRelationshipAlreadyDeleted_ThenSwallowEvent()
        {
            return RunAsync(f => f.AddMatchingUpdatedRelationship().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.OutboxData.Count(o => o.MessageId == f.MessageId) == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }
    }

    internal class AccountProviderLegalEntityDeletedEventHandlerTestsFixture : 
        ReadStoreEventHandlerTestsFixture<AccountProviderLegalEntityDeletedEvent>
    {
        public long Ukprn = 11111;
        public long AccountProviderLegalEntityId = 222222;
        public string MessageId = "messageId";
        public string DeletedMessageId = "deletedMessageId";
        public DateTime Created = DateTime.Now.AddMinutes(-12);
        public DateTime Deleted = DateTime.Now.AddMinutes(-10);
        public DateTime Updated = DateTime.Now.AddMinutes(-8);

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityDeletedEventHandler(repo))

        {
            Message = new AccountProviderLegalEntityDeletedEvent(Ukprn, AccountProviderLegalEntityId, Deleted);
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingRelationship()
        {
            var relationship = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxMessage(MessageId, Created))
                .WithOperation(Operation.CreateCohort)
                .Build();
            Relationships.Add(relationship);

            return this;
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingDeletedRelationship()
        {
            var relationship = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithDeleted(Deleted)
                .Build();
            Relationships.Add(relationship);

            return this;
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingUpdatedRelationship()
        {
            var relationship = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithUpdated(Updated)
                .Build();
            Relationships.Add(relationship);

            return this;
        }
    }
}