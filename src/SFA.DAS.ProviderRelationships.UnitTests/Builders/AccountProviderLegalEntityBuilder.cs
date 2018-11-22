using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public class AccountProviderLegalEntityBuilder
    {
        private readonly Mock<AccountProviderLegalEntity> _accountProviderLegalEntity = new Mock<AccountProviderLegalEntity> { CallBase = true };

        public AccountProviderLegalEntityBuilder WithId(long id)
        {
            _accountProviderLegalEntity.SetupProperty(p => p.Id, id);
            
            return this;
        }

        public AccountProviderLegalEntityBuilder WithAccountProviderId(long accountProviderId)
        {
            _accountProviderLegalEntity.SetupProperty(p => p.AccountProviderId, accountProviderId);
            
            return this;
        }

        public AccountProviderLegalEntityBuilder WithAccountLegalEntityId(long accountLegalEntityId)
        {
            _accountProviderLegalEntity.SetupProperty(p => p.AccountLegalEntityId, accountLegalEntityId);
            
            return this;
        }

        public AccountProviderLegalEntityBuilder WithAccountProvider(AccountProvider accountProvider)
        {
            _accountProviderLegalEntity.SetupProperty(p => p.AccountProvider, accountProvider);

            return this;
        }

        public AccountProviderLegalEntity Build()
        {
            return _accountProviderLegalEntity.Object;
        }

        public static implicit operator AccountProviderLegalEntity(AccountProviderLegalEntityBuilder builder)
        {
            return builder.Build();
        }
    }
}