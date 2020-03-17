using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class AccountProviderDto
    {
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }
        public string FormattedProviderSuggestion { get; set; }
        public long Id { get; set; }
        public List<AccountLegalEntityDto> AccountLegalEntities { get; set; }
    }
}