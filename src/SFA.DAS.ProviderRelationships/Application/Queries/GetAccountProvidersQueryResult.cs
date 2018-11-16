using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProvidersQueryResult
    {
        public IEnumerable<AccountProviderDto> AccountProviders { get; }

        public GetAccountProvidersQueryResult(IEnumerable<AccountProviderDto> accountProviders)
        {
            AccountProviders = accountProviders;
        }
    }
}