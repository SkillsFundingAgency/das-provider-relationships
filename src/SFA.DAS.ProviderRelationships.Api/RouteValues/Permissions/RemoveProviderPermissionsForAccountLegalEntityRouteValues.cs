using System;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.Permissions
{
    public class RevokePermissionsRouteValues
    {
        public long? Ukprn { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public Operation[] OperationsToRemove { get; set; }

        public RevokePermissionsRouteValues() { }

        public RevokePermissionsRouteValues(long? ukprn, string accountLegalEntityPublicHashedId, Operation[] operationsToRemove) : this()
        {
            Ukprn = ukprn;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            OperationsToRemove = operationsToRemove;
        }

    }
}