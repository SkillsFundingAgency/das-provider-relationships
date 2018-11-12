using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
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
        public Task Handle_WhenDeletes_ThenPermissionShouldHaveNoOperationsAndSetDeletedDate()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.DeletedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Deleted == f.Message.Created &&
                        p.Operations.Any() == false
                    )
                    , null, It.IsAny<CancellationToken>())));
        }
        [Test]
        public Task Handle_WhenDeletes_ThenThePermissionShouldAddMessageToOutbox()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.DeletedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.DeletedMessageId && o.Created == f.Message.Created)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenDeleteIsCalledWithADuplicateMessage_ThenThePermissionShouldMakeNoChanges()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Relationship>(p => 
                        p.OutboxData.Count() == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenDeleteOnPermissionAlreadyDeleted_ThenThrowError()
        {
            return RunAsync(f => f.AddMatchingUpdatedPermission().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                (f,r) => r.Should().Throw<InvalidOperationException>());
        }
    }

    internal class AccountProviderLegalEntityDeletedEventHandlerTestsFixture : 
        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityDeletedEvent>
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

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingPermission()
        {
            var permission = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxMessage(MessageId, Created))
                .WithOperation(Operation.CreateCohort)
                .Build();
            Permissions.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingDeletedPermission()
        {
            var permission = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithDeleted(Deleted)
                .Build();
            Permissions.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingUpdatedPermission()
        {
            var permission = new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithUpdated(Updated)
                .Build();
            Permissions.Add(permission);

            return this;
        }
    }
}