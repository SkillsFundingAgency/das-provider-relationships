using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.AccountProviderLegalEntities
{
    public class GetAccountProviderLegalEntitiesRouteValues
    {
        public string AccountHashedId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public long? Ukprn { get; set; }
        public Operation? Operation { get; set; }
    }
}