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

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.AccountProviderLegalEntityCreatedEventHandlerTests
{
    [TestFixture]
    [Parallelizable]
    internal class WhenANewRelationIsToBeRecreated : FluentTest<AccountProviderLegalEntityRecreatedEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenItsAnExistingDeletedRelationship_ThenTheRelationshipShouldBeRecreated()
        {
            return RunAsync(f => f.AddDeletedMatchingRelationship().SetMessageIdInContext(f.ReactivatedMessageId),
                f => f.Handle(),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.AccountProvider.Ukprn == f.Ukprn &&
                        p.AccountProviderLegalEntity.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.AccountProvider.AccountId == f.AccountId &&
                        p.AccountProvider.AccountPublicHashedId == f.AccountPublicHashedId &&
                        p.AccountProvider.AccountName == f.AccountName &&
                        p.AccountProviderLegalEntity.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.AccountProviderLegalEntity.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
                        p.AccountProviderLegalEntity.AccountLegalEntityName == f.AccountLegalEntityName &&
                        p.AccountProvider.AccountProviderId == f.AccountProviderId &&
                        p.Created == f.Created &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.ReactivatedMessageId)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCalledOnNonDeletedRelationship_ThenShouldThrowException()
        {
            return RunAsync<InvalidOperationException>(f => f.AddActiveMatchingRelationship().SetMessageIdInContext(f.ReactivatedMessageId), 
                f => f.Handle(), (f,r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public Task Handle_WhenCalledOnFutureDeletedRelationship_ThenShouldSwallowMessageAndNotUpdate()
        {
            return RunAsync(f => f.AddFutureDeletedMatchingRelationship().SetMessageIdInContext(f.ReactivatedMessageId),
                f => f.Handle(),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.AccountProvider.AccountName != f.AccountName &&
                        p.AccountProviderLegalEntity.AccountLegalEntityName != f.AccountLegalEntityName &&
                        p.AccountProvider.AccountProviderId == f.AccountProviderId &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.ReactivatedMessageId)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCalledWithADuplicateMessage_ThenShouldSwallowMessageAndNotUpdate()
        {
            return RunAsync(f => f.AddActiveMatchingRelationship().SetMessageIdInContext(f.MessageId),
                f => f.Handle(),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.AccountProvider.AccountName != f.AccountName &&
                        p.AccountProviderLegalEntity.AccountLegalEntityName != f.AccountLegalEntityName &&
                        p.AccountProvider.AccountProviderId == f.AccountProviderId &&
                        p.OutboxData.Count() == 1
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCalledOnFutureUpdatedPermissions_ThenShouldSwallowMessageAndNotUpdate()
        {
            return RunAsync(f => f.AddFutureUpdatedPermissionsMatchingRelationship().SetMessageIdInContext(f.ReactivatedMessageId),
                f => f.Handle(),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.AccountProvider.AccountName != f.AccountName &&
                        p.AccountProviderLegalEntity.AccountLegalEntityName != f.AccountLegalEntityName &&
                        p.AccountProvider.AccountProviderId == f.AccountProviderId &&
                        p.Deleted == null &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.ReactivatedMessageId)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

    }

    internal class AccountProviderLegalEntityRecreatedEventHandlerTestsFixture :
        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityCreatedEvent>
    {
        public long Ukprn = 11111;
        public long AccountProviderLegalEntityId = 222222;
        public long AccountId = 333333;
        public string AccountPublicHashedId = "HASHED33";
        public string AccountName = "AccountName";
        public string ReActivatedAccountName = "ReActivatedAccountName";
        public long AccountLegalEntityId = 44444;
        public string AccountLegalEntityPublicHashedId = "HASHED4444";
        public string AccountLegalEntityName = "LegalEntityName";
        public int AccountProviderId = 55555;
        public string ProviderName = "Provider 55555";
        public string MessageId = "messageId";
        public string ReactivatedMessageId = "reactivatedMessageId";
        public DateTime Created = DateTime.Now.AddMinutes(-1);
        public DateTime Deleted = DateTime.Now.AddMinutes(-10);

        public AccountProviderLegalEntityRecreatedEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityCreatedEventHandler(repo))

        {
            Message = new AccountProviderLegalEntityCreatedEvent(Ukprn, AccountProviderLegalEntityId, AccountId,
                AccountPublicHashedId, AccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName, AccountProviderId,
                ProviderName, Created);
        }

        public AccountProviderLegalEntityRecreatedEventHandlerTestsFixture AddActiveMatchingRelationship()
        {
            var permission = CreateBasicRelationshipBuilder()
                .Build();
            Relationships.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityRecreatedEventHandlerTestsFixture AddFutureDeletedMatchingRelationship()
        {
            var permission = CreateBasicRelationshipBuilder()
                .WithDeleted(Deleted.AddMonths(1))
                .Build();
            Relationships.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityRecreatedEventHandlerTestsFixture AddFutureUpdatedPermissionsMatchingRelationship()
        {
            var permission = CreateBasicRelationshipBuilder()
                .WithPermissionsOperator(Operation.CreateCohort, Created.AddMonths(1))
                .Build();
            Relationships.Add(permission);

            return this;
        }

        public AccountProviderLegalEntityRecreatedEventHandlerTestsFixture AddDeletedMatchingRelationship()
        {
            var permission = CreateBasicRelationshipBuilder()
                .WithDeleted(Deleted)
                .Build();
            Relationships.Add(permission);

            return this;
        }
        public RelationshipBuilder CreateBasicRelationshipBuilder()
        {
            return new RelationshipBuilder()
                .WithAccountProvider(new AccountProvider(Ukprn, AccountId, AccountPublicHashedId, "Old Account Name", AccountProviderId))
                .WithAccountProviderLegalEntity(new AccountProviderLegalEntity(AccountProviderLegalEntityId, AccountLegalEntityId, AccountLegalEntityPublicHashedId, "Old LE Name"))
                .WithCreated(Created.AddMinutes(-1))
                .WithOutboxMessage(new OutboxMessage(MessageId, Created));
        }
    }
}