using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class PermissionRequest
    {
        public long Ukprn { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public Operation Operation { get; set; }
    }
}