using System;
using System.Collections.Generic;
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
    internal class AccountProviderLegalEntityUpdatedEventHandlerTests : FluentTest<AccountProviderLegalEntityUpdatedEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenNoRelationshipIsFound_ThenThrowException()
        {
            return RunAsync(
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                (f,r) => r.Should().Throw<Exception>());
        }

        [Test]
        public Task Handle_WhenUpdates_ThenPermissionShouldHaveOneOperationsAndSetUpdatedDate()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p =>
                        p.Updated == f.Message.Created &&
                        p.Operations.FirstOrDefault() == Operation.CreateCohort
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenUpdates_ThenThePermissionShouldAddMessageToOutbox()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.UpdatedMessageId && o.Created == f.Message.Created)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenUpdateIsCalledWithADuplicateMessage_ThenThePermissionShouldMakeNoChanges()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p => 
                        p.OutboxData.Count() == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenUpdateIsCalledOnPermissionAlreadyDeleted_ThenThrowError()
        {
            return RunAsync(f => f.AddMatchingDeletedPermission().SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                (f,r) => r.Should().Throw<Exception>());
        }
        [Test]
        public Task Handle_WhenAnOldUpdateIsCalledOnAPermissionWithAMoreRecentUpdate_ThenSwallowMessage()
        {
            return RunAsync(f => f.AddMatchingUpdatedPermission().SetMessageIdInContext(f.UpdatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.Operations.Any() == false
                    )
                    , null, It.IsAny<CancellationToken>())));
        }
    }

    internal class AccountProviderLegalEntityUpdatedEventHandlerTestsFixture : 
        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityUpdatedEvent>
    {
        internal long Ukprn = 11111;
        internal long AccountProviderLegalEntityId = 222222;
        internal string MessageId = "messageId";
        internal string UpdatedMessageId = "updatedMessageId";
        internal DateTime Created = DateTime.Now.AddMinutes(-12);
        internal DateTime Deleted = DateTime.Now.AddMinutes(-10);
        internal DateTime Updated = DateTime.Now.AddMinutes(-8);

        public AccountProviderLegalEntityUpdatedEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityUpdatedEventHandler(repo))
        {
            var operations = new HashSet<Operation> { Operation.CreateCohort};
            Message = new AccountProviderLegalEntityUpdatedEvent(Ukprn, AccountProviderLegalEntityId, Guid.NewGuid(), operations, Created);
        }

        public AccountProviderLegalEntityUpdatedEventHandlerTestsFixture AddMatchingPermission()
        {
            var permission = new PermissionBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxDataItem(MessageId, Created))
                .Build();
            Permissions.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityUpdatedEventHandlerTestsFixture AddMatchingUpdatedPermission()
        {
            var permission = new PermissionBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithCreated(Created)
                .WithOutboxMessage(new OutboxDataItem(MessageId, Created))
                .WithUpdated(Updated)
                .Build();
            Permissions.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityUpdatedEventHandlerTestsFixture AddMatchingDeletedPermission()
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

    }
}