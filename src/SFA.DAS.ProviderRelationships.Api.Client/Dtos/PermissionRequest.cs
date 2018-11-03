using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.Dtos
{
    public class PermissionRequest
    {
        public long Ukprn { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public Operation Operation { get; set; }
    }
}