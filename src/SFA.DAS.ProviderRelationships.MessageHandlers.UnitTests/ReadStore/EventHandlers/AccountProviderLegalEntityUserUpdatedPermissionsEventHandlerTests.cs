using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.ReadStore.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.ReadStore.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    internal class AccountProviderLegalEntityUserUpdatedPermissionssEventHandlerTests : FluentTest<AccountProviderLegalEntityUserUpdatedPermissionsEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenNoRelationshipIsFound_ThenThrowException()
        {
            return RunAsync(
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                (f,r) => r.Should().Throw<Exception>());
        }

        [Test]
        public Task Handle_WhenUpdates_ThenRelationshipShouldHaveOneOperationsAndSetUpdatedDate()
        {
            return RunAsync(f => f.AddMatchingRelationship().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Updated == f.Message.Created &&
                        p.Operations.FirstOrDefault() == Operation.CreateCohort
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenUpdates_ThenTheRelationshipShouldAddMessageToOutbox()
        {
            return RunAsync(f => f.AddMatchingRelationship().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.UpdatedMessageId && o.Created == f.Message.Created)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenUpdateIsCalledWithADuplicateMessage_ThenTheRelationshipShouldMakeNoChanges()
        {
            return RunAsync(f => f.AddMatchingRelationship().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p => 
                        p.OutboxData.Count() == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenUpdateIsCalledOnRelationshipWhichHasBeenDeletedMoreRecently_ThenSwallowMessage()
        {
            return RunAsync(f => f.AddMatchingDeletedRelationship().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x=>x.Update(It.Is<Relationship>(p => 
                    p.OutboxData.Count() == 2 && 
                    p.OutboxData.Count(o=>o.MessageId == f.UpdatedMessageId) == 1
                )
                , null, It.IsAny<CancellationToken>())));
        }
        [Test]
        public Task Handle_WhenAnOldUpdateIsCalledOnARelationshipWithAMoreRecentUpdate_ThenSwallowMessage()
        {
            return RunAsync(f => f.AddMatchingUpdatedRelationship().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdatedMessageId) == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }
    }

    internal class AccountProviderLegalEntityUserUpdatedPermissionsEventHandlerTestsFixture : 
        ReadStoreEventHandlerTestsFixture<AccountProviderLegalEntityUserUpdatedPermissionsEvent>
    {
        public long Ukprn = 11111;
        public long AccountProviderLegalEntityId = 222222;
        public string MessageId = "messageId";
        public string UpdatedMessageId = "updatedMessageId";
        public DateTime Created = DateTime.Now.AddMinutes(-12);
        public DateTime Deleted = DateTime.Now.AddMinutes(-4);
        public DateTime Updated = DateTime.Now.AddMinutes(-8);

        public AccountProviderLegalEntityUserUpdatedPermissionsEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityUserUpdatedPermissionsEventHandler(repo))
        {
            var operations = new HashSet<Operation> { Operation.CreateCohort};
            Message = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(Ukprn, AccountProviderLegalEntityId, Guid.NewGuid(), operations, Updated);
        }

        public AccountProviderLegalEntityUserUpdatedPermissionsEventHandlerTestsFixture AddMatchingRelationship()
        {
            var relationship = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxMessage(MessageId, Created))
                .Build();
            Relationships.Add(relationship);

            return this;
        }

        public AccountProviderLegalEntityUserUpdatedPermissionsEventHandlerTestsFixture AddMatchingUpdatedRelationship()
        {
            var relationship = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxMessage(MessageId, Created))
                .WithUpdated(Updated)
                .Build();
            Relationships.Add(relationship);

            return this;
        }

        public AccountProviderLegalEntityUserUpdatedPermissionsEventHandlerTestsFixture AddMatchingDeletedRelationship()
        {
            var relationship = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxMessage(MessageId, Created))
                .WithDeleted(Deleted)
                .Build();
            Relationships.Add(relationship);

            return this;
        }

    }
}