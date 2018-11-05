using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProvidersQueryResponse
    {
        public class AccountProvider
        {
            public int Id { get; set; }
            public string ProviderName { get; set; }
        }
        
        public IEnumerable<AccountProvider> AccountProviders { get; }

        public GetAddedProvidersQueryResponse(IEnumerable<AccountProvider> accountProviders)
        {
            AccountProviders = accountProviders;
        }
    }
}