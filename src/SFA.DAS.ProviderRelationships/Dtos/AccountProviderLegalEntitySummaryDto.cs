using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class AccountProviderLegalEntitySummaryDto
    {
        public long Id { get; set; }
        public long AccountProviderId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}