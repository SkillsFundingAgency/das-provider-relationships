using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderQueryResult
    {
        public AccountProviderDto AccountProvider { get; }
        public List<AccountLegalEntityBasicDto> AccountLegalEntities { get; }

        public GetAccountProviderQueryResult(AccountProviderDto accountProvider, List<AccountLegalEntityBasicDto> accountLegalEntities)
        {
            AccountProvider = accountProvider;
            AccountLegalEntities = accountLegalEntities;
        }
    }
}