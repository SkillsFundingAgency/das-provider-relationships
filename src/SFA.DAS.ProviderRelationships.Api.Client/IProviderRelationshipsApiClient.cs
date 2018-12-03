using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderRelationshipsApiClient
    {
        Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission(GetAccountProviderLegalEntitiesWithPermissionRequest withPermissionRequest, CancellationToken cancellationToken = default);
        Task<bool> HasPermission(HasPermissionRequest request, CancellationToken cancellationToken = default);
        Task<bool> HasRelationshipWithPermission(HasRelationshipWithPermissionRequest request, CancellationToken cancellationToken = default);
        Task HealthCheck();
    }
}