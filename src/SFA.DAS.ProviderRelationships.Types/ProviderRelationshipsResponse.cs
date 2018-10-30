using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types
{
    public class ProviderRelationshipsResponse
    {
        public IEnumerable<ProviderRelationshipResponse> ProviderRelationships { get; set; }
    }
}