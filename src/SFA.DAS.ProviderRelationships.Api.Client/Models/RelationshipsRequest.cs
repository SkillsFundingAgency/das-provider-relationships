using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client.Models
{
    public class RelationshipsRequest
    {
        public long Ukprn { get; set; }
        public Operation Operation { get; set; }
    }
}