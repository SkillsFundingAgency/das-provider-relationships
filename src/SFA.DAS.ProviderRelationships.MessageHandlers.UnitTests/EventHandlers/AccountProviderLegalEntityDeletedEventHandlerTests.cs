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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    internal class AccountProviderLegalEntityDeletedEventHandlerTests : FluentTest<AccountProviderLegalEntityDeletedEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenNRelationshipIsFound_ThenThrowException()
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
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.Created == f.Created &&
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
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p =>
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
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p => 
                        p.OutboxData.Count() == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenDeleteOnPermissionAlreadyDeleted_ThenThrowError()
        {
            return RunAsync(f => f.AddMatchingUpdatedPermission().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                (f,r) => r.Should().Throw<Exception>());
        }


    }

    internal class AccountProviderLegalEntityDeletedEventHandlerTestsFixture : 
        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityDeletedEvent>
    {
        internal long Ukprn = 11111;
        internal long AccountProviderLegalEntityId = 222222;
        internal long AccountId = 333333;
        internal string AccountPublicHashedId = "HASHED33";
        internal string AccountName = "AccountName";
        internal string ReActivatedAccountName = "ReActivatedAccountName";
        internal long AccountLegalEntityId = 44444;
        internal string AccountLegalEntityPublicHashedId = "HASHED4444";
        internal string AccountLegalEntityName = "LegalEntityName";
        internal int AccountProviderId = 55555;
        internal string ProviderName = "Provider 55555";
        internal string MessageId = "messageId";
        internal string DeletedMessageId = "reactivatedMessageId";
        internal DateTime Created = DateTime.Now.AddMinutes(-12);
        internal DateTime Deleted = DateTime.Now.AddMinutes(-10);
        internal DateTime Updated = DateTime.Now.AddMinutes(-8);

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityDeletedEventHandler(repo))

        {
            Message = new AccountProviderLegalEntityDeletedEvent(Ukprn, AccountProviderLegalEntityId, Deleted);
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingPermission()
        {
            var permission = new PermissionBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxDataItem(MessageId, Created))
                .WithOperation(Operation.CreateCohort)
                .Build();
            Permissions.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityDeletedEventHandlerTestsFixture AddMatchingDeletedPermission()
        {
            var permission = new PermissionBuilder()
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
            var permission = new PermissionBuilder()
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