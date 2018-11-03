using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client.Models
{
    public class PermissionRequest
    {
        public long EmployerAccountLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public Operation Operation { get; set; }
    }
}