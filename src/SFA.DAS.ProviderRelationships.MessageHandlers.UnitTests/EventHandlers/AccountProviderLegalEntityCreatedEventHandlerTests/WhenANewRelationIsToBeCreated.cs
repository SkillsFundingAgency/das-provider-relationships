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
                        p.Provider.Ukprn == f.Ukprn &&
                        p.Account.Id == f.AccountId &&
                        p.Account.AccountPublicHashedId == f.AccountPublicHashedId &&
                        p.Account.AccountName == f.AccountName &&
                        p.AccountLegalEntity.Id == f.AccountLegalEntityId &&
                        p.AccountLegalEntity.PublicHashedId == f.AccountLegalEntityPublicHashedId &&
                        p.AccountLegalEntity.Name == f.AccountLegalEntityName &&
                        p.AccountProvider.Id == f.AccountProviderId &&
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
            Message = new AccountProviderLegalEntityCreatedEvent(Ukprn, AccountId,
                AccountPublicHashedId, AccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName, AccountProviderId,
                ProviderName, Created);
        }

    }
}