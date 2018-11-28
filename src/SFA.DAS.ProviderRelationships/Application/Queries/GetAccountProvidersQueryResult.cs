using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProvidersQueryResult
    {
        public List<AccountProviderSummaryDto> AccountProviders { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetAccountProvidersQueryResult(List<AccountProviderSummaryDto> accountProviders, int accountLegalEntitiesCount)
        {
            AccountProviders = accountProviders;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}