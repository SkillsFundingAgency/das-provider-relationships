using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class GetAccountProviderLegalEntitiesWithPermissionResponse
    {
        public IEnumerable<AccountProviderLegalEntityDto> AccountProviderLegalEntities { get; set; }
    }
}