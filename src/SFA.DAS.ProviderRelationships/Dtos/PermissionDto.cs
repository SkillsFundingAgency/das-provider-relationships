using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Dtos
{
    public class PermissionDto
    {
        public long Id { get; set; }
        public Operation Operation { get; set; }
    }
}