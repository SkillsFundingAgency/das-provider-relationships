using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderLegalEntitiesWithPermissionQueryResult
    {
        // we could c&p the RelationshipDto into this projects Dtos and then map (for clean separation), but not sure that gives us anything atm
        public IEnumerable<AccountProviderLegalEntityDto> Relationships { get; }

        public GetAccountProviderLegalEntitiesWithPermissionQueryResult(IEnumerable<AccountProviderLegalEntityDto> relationships)
        {
            Relationships = relationships;
        }
    }
}