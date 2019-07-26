using System;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.Providers
{
    public class RemoveProviderPermissionsFromAccountLegalEntityRouteValues
    {
        public long? Ukprn { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public Operation[] OperationsToRemove { get; set; }
    }
}