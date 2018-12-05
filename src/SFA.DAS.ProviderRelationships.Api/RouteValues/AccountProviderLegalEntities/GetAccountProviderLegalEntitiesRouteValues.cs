using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.AccountProviderLegalEntities
{
    public class GetAccountProviderLegalEntitiesRouteValues
    {
        public long? Ukprn { get; set; }
        public Operation? Operation { get; set; }
    }
}