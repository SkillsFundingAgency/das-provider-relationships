using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Api.Client.Models
{
    public class RelationshipsResponse
    {
        public IEnumerable<RelationshipResponse> Relationships { get; set; }
    }
}