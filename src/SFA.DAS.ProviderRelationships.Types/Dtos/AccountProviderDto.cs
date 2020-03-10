using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class AccountProviderDto : ProviderDto
    {
        public long Id { get; set; }
        public List<AccountLegalEntityDto> AccountLegalEntities { get; set; }
    }
}