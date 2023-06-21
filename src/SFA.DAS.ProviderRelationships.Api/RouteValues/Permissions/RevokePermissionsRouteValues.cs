using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions;

public class RevokePermissionsRouteValues
{
    public long? Ukprn { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public Operation[] OperationsToRevoke { get; set; }

    public RevokePermissionsRouteValues() { }

    public RevokePermissionsRouteValues(long? ukprn, string accountLegalEntityPublicHashedId, Operation[] operationsToRevoke) : this()
    {
        Ukprn = ukprn;
        AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
        OperationsToRevoke = operationsToRevoke;
    }

}