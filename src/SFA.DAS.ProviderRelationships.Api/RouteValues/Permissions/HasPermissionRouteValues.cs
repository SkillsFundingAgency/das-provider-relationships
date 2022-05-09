using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions
{
    public class HasPermissionRouteValues
    {
        public long? Ukprn { get; set; }
        public long? AccountLegalEntityId { get; set; }
        public Operation? Operation { get; set; }
    }
}