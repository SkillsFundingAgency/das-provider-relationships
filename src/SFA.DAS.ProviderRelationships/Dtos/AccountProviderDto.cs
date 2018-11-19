using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class AccountProviderDto
    {
        public long Id { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}