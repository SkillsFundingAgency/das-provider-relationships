using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class HasPermissionRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }
        public Operation Operation { get; set; }
    }
}