using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.Testing;
using Permission = SFA.DAS.ProviderRelationships.ReadStore.Models.Permission;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class PermissionTests : FluentTest<PermissionTestsFixture>
    {
        [Test]
        public void Create_WhenCalled_ThenCorrectlySetsAllProperties()
        {
            Run(f => f.Create(), (f, r) => r.Should().NotBeNull()
                .And.Match<Permission>(p =>
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
        public void ReActivateRelationship_WhenCalledOnSoftDeletedPermission_ThenResetsProperties()
        {
            Run(f => f.SetPermissionToSoftDeleted(f.Deleted), f => f.ReactivateRelation(), (f, r) => r.Should().NotBeNull().And
                .Match<Permission>(p =>
                    p.Ukprn == f.Ukprn && p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                    p.AccountId == f.AccountId && p.AccountPublicHashedId == f.AccountPublicHashedId &&
                    p.AccountName == f.ReActivatedAccountName &&
                    p.AccountLegalEntityId == f.AccountLegalEntityId &&
                    p.AccountLegalEntityPublicHashedId == f.AccountLegalEntityPublicHashedId &&
                    p.AccountLegalEntityName == f.AccountLegalEntityName &&
                    p.AccountProviderId == f.AccountProviderId && p.Created == f.ReActivateDate && p.Deleted == null));
        }

        [Test]
        public void ReActivateRelationship_WhenCalledOnSoftDeletedPermission_ThenAddsAMessageToTheOutboxData()
        {
            Run(f => f.SetPermissionToSoftDeleted(f.Deleted), f => f.ReactivateRelation(), (f, r) => r.OutboxData.First().Should()
                .Match<OutboxMessage>(i => i.MessageId == f.ReActivateMessageId && i.Created == f.ReActivateDate));
        }

        [Test]
        public void ReActivateRelationship_WhenCalledOnActivePermission_ThenShouldThrowException()
        {
            Run(f => f.SetPermissionToActive(), f => f.ReactivateRelation(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public void ReActivateRelationship_WhenCalledOnPermissionDeletedInTheFuture_ThenShouldThrowException()
        {
            Run(f => f.SetPermissionToSoftDeleted(f.FutureDate), f => f.ReactivateRelation(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        [Test]
        public void ReActivateRelationship_WhenCalledWithDuplicateMessageId_ThenShouldSwallowMessageAndNotExtendTheOutbox()
        {
            Run(f => f.SetPermissionToBeSoftDeletedAndWithMessageInOutbox(), f => f.ReactivateRelation(), (f, r) => r.OutboxData.Count().Should().Be(1));
        }

        [Test]
        public void ReActivateRelationship_WhenCalledWithDuplicateMessageId_ThenShouldSwallowMessageAndNotSetProperties()
        {
            Run(f => f.SetPermissionToBeSoftDeletedAndWithMessageInOutbox(), f => f.ReactivateRelation(), (f, r) => r.AccountName.Should().NotBe(f.ReActivatedAccountName));
        }
    }

    public class PermissionTestsFixture
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

        internal Permission Permission;

        internal Permission Create()
        {
            return Permission.Create(Ukprn, AccountProviderLegalEntityId, AccountId, AccountPublicHashedId, AccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName,
                AccountProviderId, Created, MessageId);
        }

        internal Permission ReactivateRelation()
        {
            Permission.ReActivateRelationship(Ukprn, AccountProviderLegalEntityId, AccountId, AccountPublicHashedId, ReActivatedAccountName,
                AccountLegalEntityId, AccountLegalEntityPublicHashedId, AccountLegalEntityName,
                AccountProviderId, ReActivateDate, ReActivateMessageId);
            return Permission;
        }

        internal PermissionTestsFixture SetPermissionToActive()
        {
            Permission = CreateBasicPermission().Build();
            return this;
        }

        internal PermissionTestsFixture SetPermissionToSoftDeleted(DateTime deleted)
        {
            Permission = CreateBasicPermission().WithDeleted(deleted).Build();
            return this;
        }

        internal PermissionTestsFixture SetPermissionToBeSoftDeletedAndWithMessageInOutbox()
        {
            Permission = CreateBasicPermission().WithDeleted(Deleted).WithOutboxMessage(new OutboxMessage(ReActivateMessageId, ReActivateDate)).Build();
            return this;
        }

        private PermissionBuilder CreateBasicPermission()
        {
            return new PermissionBuilder()
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