using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderLegalEntitiesWithPermissionQueryResult
    {
        public IEnumerable<AccountProviderLegalEntityDto> AccountProviderLegalEntities { get; }

        public GetAccountProviderLegalEntitiesWithPermissionQueryResult(IEnumerable<AccountProviderLegalEntityDto> accountProviderLegalEntities)
        {
            AccountProviderLegalEntities = accountProviderLegalEntities;
        }
    }
}