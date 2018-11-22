using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderRelationshipsApiClient
    {
        //todo: rename method & dto relationship => accountproviderlegalentity!
        Task<RelationshipsResponse> GetRelationshipsWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default);
        Task<bool> HasPermission(PermissionRequest request, CancellationToken cancellationToken = default);
        Task<bool> HasRelationshipWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default);
        Task HealthCheck();
    }
}