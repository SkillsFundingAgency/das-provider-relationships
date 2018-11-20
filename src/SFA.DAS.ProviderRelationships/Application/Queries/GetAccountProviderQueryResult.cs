using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderQueryResult
    {
        public AccountProviderDto AccountProvider { get; }
        public List<AccountLegalEntityDto> AccountLegalEntities { get; }

        public GetAccountProviderQueryResult(AccountProviderDto accountProvider, List<AccountLegalEntityDto> accountLegalEntities)
        {
            AccountProvider = accountProvider;
            AccountLegalEntities = accountLegalEntities;
        }
    }
}