using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.Testing;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    internal class AccountProviderLegalEntityCreatedEventHandlerTests : FluentTest<AccountProviderLegalEntityCreatedEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenItsANewRelationship_ThenTheMessageShouldBeTransformedIntoThePermission()
        {
            return RunAsync(
                f => f.SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Add(It.Is<Permission>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.AccountId == f.AccountId &&
                        p.AccountPublicHashedId == f.AccountPublicHashedId &&
                        p.AccountName == f.AccountName &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
                        p.AccountLegalEntityName == f.AccountLegalEntityName &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.Created == f.Created &&
                        p.OutboxData.Count() == 1 &&
                        p.OutboxData.First().MessageId == f.MessageId),
                    null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenItsAReActivatedRelationship_ThenTheMessageShouldBeTransformedIntoThePermission()
        {
            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.ReactivatedMessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.PermissionsRepository.Verify(x => x.Update(It.Is<Permission>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.AccountId == f.AccountId &&
                        p.AccountPublicHashedId == f.AccountPublicHashedId &&
                        p.AccountName == f.AccountName &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
                        p.AccountLegalEntityName == f.AccountLegalEntityName &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.Created == f.Created &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.ReactivatedMessageId)
                    )
                    , It.IsAny<CancellationToken>())));
        }
    }

    internal class AccountProviderLegalEntityCreatedEventHandlerTestsFixture : 
        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityCreatedEvent>
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
        internal string ReactivatedMessageId = "reactivatedMessageId";
        internal DateTime Created = DateTime.Now.AddMinutes(-1);
        internal DateTime Deleted = DateTime.Now.AddMinutes(-10);

        public AccountProviderLegalEntityCreatedEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityCreatedEventHandler(repo))

        {
            Message = new AccountProviderLegalEntityCreatedEvent(Ukprn, AccountProviderLegalEntityId, AccountId,
                AccountPublicHashedId, AccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName, AccountProviderId,
                ProviderName, Created);
        }

        public AccountProviderLegalEntityCreatedEventHandlerTestsFixture AddMatchingPermission()
        {
            var permission = new PermissionBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithDeleted(Deleted)
                .WithOutboxMessage(new OutboxDataItem(MessageId, Created))
                .Build();
            Permissions.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityCreatedEventHandlerTestsFixture SetMessageIdInContext(string messageId)
        {
            MessageHandlerContext.Setup(x => x.MessageId).Returns(messageId);
            return this;
        }

    }







}