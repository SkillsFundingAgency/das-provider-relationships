using System;
using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public class AccountLegalEntityBuilder
    {
        private readonly Mock<AccountLegalEntity> _accountLegalEntity = new Mock<AccountLegalEntity> { CallBase = true };

        public AccountLegalEntityBuilder()
        {
            _accountLegalEntity.SetupAllProperties();
        }

        public AccountLegalEntityBuilder WithId(long id)
        {
            _accountLegalEntity.SetupProperty(a => a.Id, id);
            
            return this;
        }

        public AccountLegalEntityBuilder WithAccountId(long accountId)
        {
            _accountLegalEntity.SetupProperty(a => a.AccountId, accountId);
            
            return this;
        }

        public AccountLegalEntityBuilder WithName(string name)
        {
            _accountLegalEntity.SetupProperty(a => a.Name, name);
            
            return this;
        }

        public AccountLegalEntityBuilder WithCreated(DateTime created)
        {
            _accountLegalEntity.SetupProperty(a => a.Created, created);
            
            return this;
        }

        public AccountLegalEntityBuilder WithUpdated(DateTime updated)
        {
            _accountLegalEntity.SetupProperty(a => a.Updated, updated);
            
            return this;
        }

        public AccountLegalEntityBuilder WithDeleted(DateTime deleted)
        {
            _accountLegalEntity.SetupProperty(a => a.Deleted, deleted);
            
            return this;
        }

        public AccountLegalEntity Build()
        {
            return _accountLegalEntity.Object;
        }

        public static implicit operator AccountLegalEntity(AccountLegalEntityBuilder builder)
        {
            return builder.Build();
        }
    }
}