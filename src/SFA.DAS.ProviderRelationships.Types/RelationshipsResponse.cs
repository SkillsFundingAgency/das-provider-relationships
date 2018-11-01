using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types
{
    public class RelationshipsResponse
    {
        public IEnumerable<RelationshipResponse> Relationships { get; set; }
    }
}