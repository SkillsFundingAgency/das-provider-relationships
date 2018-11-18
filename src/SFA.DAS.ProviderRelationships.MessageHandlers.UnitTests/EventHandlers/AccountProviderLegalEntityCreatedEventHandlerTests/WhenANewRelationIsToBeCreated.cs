using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.AccountProviderLegalEntityCreatedEventHandlerTests
{
    [TestFixture]
    [Parallelizable]
    internal class WhenANewRelationIsToBeCreated : FluentTest<AccountProviderLegalEntityCreatedEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenTheRequestIsValid_ThenTheRelationshipShouldBeCreated()
        {
            return RunAsync(
                f => f.SetMessageIdInContext(f.MessageId),
                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.RelationshipsRepository.Verify(x => x.Add(It.Is<Relationship>(p =>
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
                        p.OutboxData.Count() == 1 &&
                        p.OutboxData.First().MessageId == f.MessageId
                        ),
                    null, It.IsAny<CancellationToken>())));
        }
    }

    internal class AccountProviderLegalEntityCreatedEventHandlerTestsFixture :
        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityCreatedEvent>
    {
        public long Ukprn = 11111;
        public long AccountProviderLegalEntityId = 222222;
        public long AccountId = 333333;
        public string AccountPublicHashedId = "HASHED33";
        public string AccountName = "AccountName";
        public long AccountLegalEntityId = 44444;
        public string AccountLegalEntityPublicHashedId = "HASHED4444";
        public string AccountLegalEntityName = "LegalEntityName";
        public int AccountProviderId = 55555;
        public string ProviderName = "Provider 55555";
        public string MessageId = "messageId";
        public DateTime Created = DateTime.Now.AddMinutes(-1);

        public AccountProviderLegalEntityCreatedEventHandlerTestsFixture()
            : base((repo) => new AccountProviderLegalEntityCreatedEventHandler(repo))

        {
            Message = new AccountProviderLegalEntityCreatedEvent(Ukprn, AccountProviderLegalEntityId, AccountId,
                AccountPublicHashedId, AccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName, AccountProviderId,
                ProviderName, Created);
        }

    }
}