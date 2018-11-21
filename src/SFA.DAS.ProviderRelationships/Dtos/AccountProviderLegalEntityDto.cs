using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class AccountProviderLegalEntityDto
    {
        public long Id { get; set; }
        public long AccountLegalEntityId { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}