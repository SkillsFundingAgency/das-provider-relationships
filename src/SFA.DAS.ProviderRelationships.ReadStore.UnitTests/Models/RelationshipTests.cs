using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class RelationshipTests : FluentTest<RelationshipTestsFixture>
    {
        [Test]
        public void Create_WhenCalled_ThenCorrectlySetsAllProperties()
        {
            Run(f => f.Create(), (f, r) => r.Should().NotBeNull()
                .And.Match<Relationship>(p =>
                    p.Ukprn == f.Ukprn && p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                    p.AccountId == f.AccountId && p.AccountPublicHashedId == f.AccountPublicHashedId &&
                    p.AccountName == f.AccountName &&
                    p.AccountLegalEntityId == f.AccountLegalEntityId &&
                    p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
                    p.AccountLegalEntityName == f.AccountLegalEntityName &&
                    p.AccountProviderId == f.AccountProviderId && p.Created == f.Created && p.Deleted == null));
        }

        [Test]
        public void Create_WhenCalled_ThenAddsAMessageToTheOutboxData()
        {
            Run(f => f.Create(), (f, r) => r.OutboxData.First().Should().Match<OutboxMessage>(i=>i.MessageId == f.MessageId && i.Created == f.Created));
        }

        [Test]
        public void Recreate_WhenCalledOnSoftDeletedPermission_ThenResetsProperties()
        {
            Run(f => f.SetPermissionToSoftDeleted(f.Deleted), f => f.Recreate(), (f, r) => r.Should().NotBeNull().And
                .Match<Relationship>(p =>
                    p.Ukprn == f.Ukprn && p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                    p.AccountId == f.AccountId && p.AccountPublicHashedId == f.AccountPublicHashedId &&
                    p.AccountName == f.ReActivatedAccountName &&
                    p.AccountLegalEntityId == f.AccountLegalEntityId &&
                    p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
                    p.AccountLegalEntityName == f.AccountLegalEntityName &&
                    p.AccountProviderId == f.AccountProviderId && p.Created == f.ReActivateDate && p.Deleted == null));
        }

        [Test]
        public void Recreate_WhenCalledOnSoftDeletedPermission_ThenAddsAMessageToTheOutboxData()
        {
            Run(f => f.SetPermissionToSoftDeleted(f.Deleted), f => f.Recreate(), (f, r) => r.OutboxData.First().Should()
                .Match<OutboxMessage>(i => i.MessageId == f.ReActivateMessageId && i.Created == f.ReActivateDate));
        }

        [Test]
        public void Recreate_WhenCalledOnActivePermission_ThenShouldThrowException()
        {
            Run(f => f.SetPermissionToActive(), f => f.Recreate(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public void Recreate_WhenCalledOnPermissionDeletedInTheFuture_ThenShouldThrowException()
        {
            Run(f => f.SetPermissionToSoftDeleted(f.FutureDate), f => f.Recreate(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public void Recreate_WhenCalledWithDuplicateMessageId_ThenShouldSwallowMessageAndNotExtendTheOutbox()
        {
            Run(f => f.SetPermissionToBeSoftDeletedAndWithMessageInOutbox(), f => f.Recreate(), (f, r) => r.OutboxData.Count().Should().Be(1));
        }

        [Test]
        public void Recreate_WhenCalledWithDuplicateMessageId_ThenShouldSwallowMessageAndNotSetProperties()
        {
            Run(f => f.SetPermissionToBeSoftDeletedAndWithMessageInOutbox(), f => f.Recreate(), (f, r) => r.AccountName.Should().NotBe(f.ReActivatedAccountName));
        }
    }

    public class RelationshipTestsFixture
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
        internal string MessageId = "messageId";
        internal DateTime Created = DateTime.Now.AddMinutes(-1);
        internal DateTime Deleted = DateTime.Now.AddMinutes(1);
        internal DateTime ReActivateDate = DateTime.Now.AddMinutes(2);
        internal string ReActivateMessageId = "reActivateMessageId";
        internal DateTime FutureDate = DateTime.Now.AddMinutes(60);

        internal Relationship Relationship;

        internal Relationship Create()
        {
            return new Relationship(Ukprn, AccountProviderLegalEntityId, AccountId, AccountPublicHashedId, AccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName,
                AccountProviderId, Created, MessageId);
        }

        internal Relationship Recreate()
        {
            Relationship.Recreate(Ukprn, AccountProviderLegalEntityId, AccountId, AccountPublicHashedId, ReActivatedAccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName,
                AccountProviderId, ReActivateDate, ReActivateMessageId);
            return Relationship;
        }

        internal RelationshipTestsFixture SetPermissionToActive()
        {
            Relationship = CreateBasicPermission().Build();
            return this;
        }

        internal RelationshipTestsFixture SetPermissionToSoftDeleted(DateTime deleted)
        {
            Relationship = CreateBasicPermission().WithDeleted(deleted).Build();
            return this;
        }

        internal RelationshipTestsFixture SetPermissionToBeSoftDeletedAndWithMessageInOutbox()
        {
            Relationship = CreateBasicPermission().WithDeleted(Deleted).WithOutboxMessage(new OutboxMessage(ReActivateMessageId, ReActivateDate)).Build();
            return this;
        }

        private RelationshipBuilder CreateBasicPermission()
        {
            return new RelationshipBuilder()
                .WithUkprn(Ukprn)
                .WithAccountProviderLegalEntityId(AccountProviderLegalEntityId)
                .WithAccountId(AccountId)
                .WithAccountPublicHashedId(AccountPublicHashedId)
                .WithAccountName(AccountName)
                .WithAccountLegalEntityId(AccountLegalEntityId)
                .WithAccountLegalEntityPublicHashedId(AccountLegalEntityPublicHashedId)
                .WithAccountLegalEntityName(AccountLegalEntityName)
                .WithAccountProviderId(AccountProviderId)
                .WithCreated(Created);
        }
    }
}