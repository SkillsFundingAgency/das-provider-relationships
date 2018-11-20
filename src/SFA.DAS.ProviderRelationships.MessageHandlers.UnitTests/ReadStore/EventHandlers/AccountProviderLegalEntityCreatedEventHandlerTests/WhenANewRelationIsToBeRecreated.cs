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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;
using RelationshipBuilder = SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.Builders.RelationshipBuilder;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.ReadStore.EventHandlers.AccountProviderLegalEntityCreatedEventHandlerTests
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
                        p.Provider.Ukprn == f.Ukprn &&
                        p.Account.Id == f.AccountId &&
                        p.Account.AccountPublicHashedId == f.AccountPublicHashedId &&
                        p.Account.AccountName == f.AccountName &&
                        p.AccountLegalEntity.Id == f.AccountLegalEntityId &&
                        p.AccountLegalEntity.PublicHashedId == f.AccountLegalEntityPublicHashedId &&
                        p.AccountLegalEntity.Name == f.AccountLegalEntityName &&
                        p.AccountProvider.Id == f.AccountProviderId &&
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
                f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public Task Handle_WhenCalledOnFutureDeletedRelationship_ThenShouldSwallowMessageAndNotUpdate()
        {
            return RunAsync(f => f.AddFutureDeletedMatchingRelationship().SetMessageIdInContext(f.ReactivatedMessageId),
                f => f.Handle(),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
                        p.Account.AccountName != f.AccountName &&
                        p.AccountLegalEntity.Name != f.AccountLegalEntityName &&
                        p.AccountProvider.Id == f.AccountProviderId &&
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
                        p.Account.AccountName != f.AccountName &&
                        p.AccountLegalEntity.Name != f.AccountLegalEntityName &&
                        p.AccountProvider.Id == f.AccountProviderId &&
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
                        p.Account.AccountName != f.AccountName &&
                        p.AccountLegalEntity.Name != f.AccountLegalEntityName &&
                        p.AccountProvider.Id == f.AccountProviderId &&
                        p.Deleted == null &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Any(o => o.MessageId == f.ReactivatedMessageId)
                    )
                    , null, It.IsAny<CancellationToken>())));
        }

    }

    internal class AccountProviderLegalEntityRecreatedEventHandlerTestsFixture :
        ReadStoreEventHandlerTestsFixture<AccountProviderLegalEntityCreatedEvent>
    {
        public long Ukprn = 11111;
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
            Message = new AccountProviderLegalEntityCreatedEvent(Ukprn, AccountId,
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
                .WithExplicitOperator(Operation.CreateCohort, Created.AddMonths(1))
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
                .WithUkprn(Ukprn)
                .WithAccount(new Account(AccountId, AccountPublicHashedId, "Old Account Name"))
                .WithAccountProvider(new AccountProvider(AccountProviderId, new HashSet<Operation>()))
                .WithAccountLegalEntity(new AccountLegalEntity(AccountLegalEntityId, AccountLegalEntityPublicHashedId, "Old LE Name"))
                .WithCreated(Created.AddMinutes(-1))
                .WithOutboxMessage(new OutboxMessage(MessageId, Created));
        }
    }
}