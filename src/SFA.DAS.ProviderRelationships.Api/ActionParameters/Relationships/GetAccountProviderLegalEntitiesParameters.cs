using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships
{
    public class GetAccountProviderLegalEntitiesParameters
    {
        public long? Ukprn { get; set; }
        public Operation? Operation { get; set; }
    }
}