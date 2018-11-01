using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderRelationshipsApiClient
    {
        Task<RelationshipsResponse> GetRelationshipsWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default);
        Task<bool> HasPermission(PermissionRequest request, CancellationToken cancellationToken = default);
        Task<bool> HasRelationshipWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default);
    }
}