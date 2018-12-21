using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider.Dtos
{
    public class AccountProviderDto
    {
        public long Id { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public List<AccountLegalEntityDto> AccountLegalEntities { get; set; }
    }
}