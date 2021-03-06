using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class AccountProviderDto
    {
        public long Id { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public List<AccountLegalEntityDto> AccountLegalEntities { get; set; }
    }
}