using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions;

public class HasRelationshipWithPermissionRouteValues
{
    public long? Ukprn { get; set; }
    public Operation? Operation { get; set; }
}