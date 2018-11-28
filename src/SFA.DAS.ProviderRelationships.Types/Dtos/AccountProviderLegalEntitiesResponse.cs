using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class AccountProviderLegalEntitiesResponse
    {
        public IEnumerable<AccountProviderLegalEntityDto> Relationships { get; set; }
    }
}