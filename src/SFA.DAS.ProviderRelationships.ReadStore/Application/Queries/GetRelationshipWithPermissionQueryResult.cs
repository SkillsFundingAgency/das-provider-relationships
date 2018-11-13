using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Queries
{
    internal class GetRelationshipWithPermissionQueryResult
    {
        public IEnumerable<RelationshipDto> Relationships { get; set; }
    }
}