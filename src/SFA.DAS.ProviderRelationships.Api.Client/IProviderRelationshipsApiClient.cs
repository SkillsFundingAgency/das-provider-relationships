using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderRelationshipsApiClient
    {
        Task<bool> HasRelationshipWithPermission(ProviderRelationshipRequest request, CancellationToken cancellationToken = default);
        Task<ProviderRelationshipResponse> GetRelationshipsWithPermission(ProviderRelationshipRequest request);
    }
}