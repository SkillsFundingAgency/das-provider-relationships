using System;
using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests
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

        public AccountLegalEntityBuilder WithName(string name)
        {
            _accountLegalEntity.SetupProperty(a => a.Name, name);
            return this;
        }
        
        public AccountLegalEntityBuilder WithUpdated(DateTime updated)
        {
            _accountLegalEntity.SetupProperty(a => a.Updated, updated);
            return this;
        }
        
        public AccountLegalEntity Build()
        {
            return _accountLegalEntity.Object;
        }
    }
}