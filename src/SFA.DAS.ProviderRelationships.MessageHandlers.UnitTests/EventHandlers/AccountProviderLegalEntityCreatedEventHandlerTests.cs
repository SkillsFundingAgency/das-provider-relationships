//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Moq;
//using NUnit.Framework;
//using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
//using SFA.DAS.ProviderRelationships.Messages.Events;
//using SFA.DAS.ProviderRelationships.ReadStore.Models;
//using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
//using SFA.DAS.Testing;

//namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
//{
//    [TestFixture]
//    [Parallelizable]
//    internal class AccountProviderLegalEntityCreatedEventHandlerTests : FluentTest<AccountProviderLegalEntityCreatedEventHandlerTestsFixture>
//    {
//        [Test]
//        public Task Handle_WhenItsANewRelationship_ThenTheRelationshipShouldBeCreated()
//        {
//            return RunAsync(
//                f => f.SetMessageIdInContext(f.MessageId),
//                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
//                f => f.RelationshipsRepository.Verify(x => x.Add(It.Is<Relationship>(p =>
//                        p.Ukprn == f.Ukprn &&
//                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
//                        p.AccountId == f.AccountId &&
//                        p.AccountPublicHashedId == f.AccountPublicHashedId &&
//                        p.AccountName == f.AccountName &&
//                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
//                        p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
//                        p.AccountLegalEntityName == f.AccountLegalEntityName &&
//                        p.AccountProviderId == f.AccountProviderId &&
//                        p.Created == f.Created &&
//                        p.OutboxData.Count() == 1 &&
//                        p.OutboxData.First().MessageId == f.MessageId),
//                    null, It.IsAny<CancellationToken>())));
//        }

//        [Test]
//        public Task Handle_WhenItsAnExistingDeletedRelationship_ThenTheRelationshipShouldBeRecreated()
//        {
//            return RunAsync(f => f.AddMatchingPermission().SetMessageIdInContext(f.ReactivatedMessageId),
//                f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
//                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<Relationship>(p =>
//                        p.Ukprn == f.Ukprn &&
//                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
//                        p.AccountId == f.AccountId &&
//                        p.AccountPublicHashedId == f.AccountPublicHashedId &&
//                        p.AccountName == f.AccountName &&
//                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
//                        p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
//                        p.AccountLegalEntityName == f.AccountLegalEntityName &&
//                        p.AccountProviderId == f.AccountProviderId &&
//                        p.Created == f.Created &&
//                        p.OutboxData.Count() == 2 &&
//                        p.OutboxData.Any(o => o.MessageId == f.ReactivatedMessageId)
//                    )
//                    , null, It.IsAny<CancellationToken>())));
//        }
//    }

//    internal class AccountProviderLegalEntityCreatedEventHandlerTestsFixture :
//        DocumentEventHandlerTestsFixture<AccountProviderLegalEntityCreatedEvent>
//    {
//        public static long Ukprn = 11111;
//        public static long AccountProviderLegalEntityId = 2222;
//        public AccountProvider AccountProvider = new AccountProvider(Ukprn, 1, "HASH1", "AccountName", 2);
//        public AccountProviderLegalEntity AccountProviderLegalEntity = new AccountProviderLegalEntity(AccountProviderLegalEntityId, 44444, "HASHED4444", "LegalEntityName");


//        //public long Ukprn = 11111;
//        //public long AccountProviderLegalEntityId = 222222;
//        public long AccountId = 333333;
//        public string AccountPublicHashedId = "HASHED33";
//        public string AccountName = "AccountName";
//        public string ReActivatedAccountName = "ReActivatedAccountName";
//        public long AccountLegalEntityId = 44444;
//        public string AccountLegalEntityPublicHashedId = "HASHED4444";
//        public string AccountLegalEntityName = "LegalEntityName";
//        public int AccountProviderId = 55555;
//        public string ProviderName = "Provider 55555";
//        public string MessageId = "messageId";
//        public string ReactivatedMessageId = "reactivatedMessageId";
//        public DateTime Created = DateTime.Now.AddMinutes(-1);
//        public DateTime Deleted = DateTime.Now.AddMinutes(-10);

//        public AccountProviderLegalEntityCreatedEventHandlerTestsFixture()
//            : base((repo) => new AccountProviderLegalEntityCreatedEventHandler(repo))

//        {
//            Message = new AccountProviderLegalEntityCreatedEvent(Ukprn, AccountProviderLegalEntityId, AccountId,
//                AccountPublicHashedId, AccountName,
//                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName, AccountProviderId,
//                ProviderName, Created);
//        }

//        public AccountProviderLegalEntityCreatedEventHandlerTestsFixture AddMatchingPermission()
//        {
//            var permission = new RelationshipBuilder()
//                .WithAccountProvider(AccountProvider)
//                .WithAccountProviderLegalEntity(AccountProviderLegalEntity)
//                .WithDeleted(Deleted)
//                .WithOutboxMessage(new OutboxMessage(MessageId, Created))
//                .Build();
//            Relationships.Add(permission);

//            return this;
//        }
//    }
//}