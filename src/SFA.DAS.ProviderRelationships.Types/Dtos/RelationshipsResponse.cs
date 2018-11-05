using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types.Dtos
{
    public class RelationshipsResponse
    {
        public IEnumerable<RelationshipDto> Relationships { get; set; }
    }
}