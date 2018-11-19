using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public class AccountProviderBuilder
    {
        private readonly Mock<AccountProvider> _accountProvider = new Mock<AccountProvider> { CallBase = true };

        public AccountProviderBuilder WithId(long id)
        {
            _accountProvider.SetupProperty(ap => ap.Id, id);
            
            return this;
        }

        public AccountProviderBuilder WithAccountId(long accountId)
        {
            _accountProvider.SetupProperty(ap => ap.AccountId, accountId);
            
            return this;
        }

        public AccountProviderBuilder WithProviderUkprn(long providerUkprn)
        {
            _accountProvider.SetupProperty(ap => ap.ProviderUkprn, providerUkprn);
            
            return this;
        }

        public AccountProvider Build()
        {
            return _accountProvider.Object;
        }
        
        public static implicit operator AccountProvider(AccountProviderBuilder builder)
        {
            return builder.Build();
        }
    }
}