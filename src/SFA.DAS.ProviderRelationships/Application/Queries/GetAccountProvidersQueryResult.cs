using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProvidersQueryResult
    {
        public class AccountProvider
        {
            public int Id { get; set; }
            public string ProviderName { get; set; }
        }
        
        public IEnumerable<AccountProvider> AccountProviders { get; }

        public GetAccountProvidersQueryResult(IEnumerable<AccountProvider> accountProviders)
        {
            AccountProviders = accountProviders;
        }
    }
}