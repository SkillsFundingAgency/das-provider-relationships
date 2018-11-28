using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships
{
    //todo: move this to pr...
    // we could use RelationshipsRequest as the incoming parameter, but we want the interface to support the filters as optional
    // we could also cross-cut optional params that are not implemented by decorating the properties with required and changing sfa.das.validation to auto handle the validation (unless e.g. a 404 should be returned because the resource/collection is missing, rather than returning an error code to indicate a validation error on the filters?)

    public class GetAccountProviderLegalEntitiesParameters
    {
        public long? Ukprn { get; set; }
        public Operation? Operation { get; set; }
    }
}