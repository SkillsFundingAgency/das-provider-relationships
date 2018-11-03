using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client.Dtos
{
    public class RelationshipsResponse
    {
        public IEnumerable<RelationshipDto> Relationships { get; set; }
    }
}