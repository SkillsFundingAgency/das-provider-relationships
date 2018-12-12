using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders
{
    public class GetAccountProvidersQueryResult
    {
        public List<AccountProviderDto> AccountProviders { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetAccountProvidersQueryResult(List<AccountProviderDto> accountProviders, int accountLegalEntitiesCount)
        {
            AccountProviders = accountProviders;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}