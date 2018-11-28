using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class AccountProviderSummaryDto
    {
        public long Id { get; set; }
        public string ProviderName { get; set; }
        public int AccountProviderLegalEntitiesWithPermissionsCount { get; set; }
    }
}