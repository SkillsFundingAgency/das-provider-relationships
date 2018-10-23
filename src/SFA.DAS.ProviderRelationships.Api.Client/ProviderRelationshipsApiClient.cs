using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        public Task<bool> HasRelationshipWithPermission(ProviderRelationshipRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ProviderRelationshipResponse> ListRelationshipsWithPermission(ProviderRelationshipRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}