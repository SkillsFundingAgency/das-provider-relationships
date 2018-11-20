using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProvidersQueryResult
    {
        public IEnumerable<AccountProviderSummaryDto> AccountProviders { get; }

        public GetAccountProvidersQueryResult(IEnumerable<AccountProviderSummaryDto> accountProviders)
        {
            AccountProviders = accountProviders;
        }
    }
}