using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class HasRelationshipWithPermissionRequest
    {
        public long Ukprn { get; set; }
        public Operation Operation { get; set; }
    }
}