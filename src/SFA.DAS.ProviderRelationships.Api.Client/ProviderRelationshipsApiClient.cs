using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IRestHttpClient _restHttpClient;

        public ProviderRelationshipsApiClient(IRestHttpClient restHttpClient)
        {
            _restHttpClient = restHttpClient;
        }

        public Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission(GetAccountProviderLegalEntitiesWithPermissionRequest withPermissionRequest, CancellationToken cancellationToken = default)
        {
            return _restHttpClient.Get<GetAccountProviderLegalEntitiesWithPermissionResponse>("accountproviderlegalentities", withPermissionRequest, cancellationToken);
        }

        public Task<bool> HasPermission(HasPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _restHttpClient.Get<bool>("permissions/has", request, cancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission(HasRelationshipWithPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _restHttpClient.Get<bool>("permissions/has-relationship-with", request, cancellationToken);
        }

        public Task Ping(CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(_restHttpClient.Get("ping", null, cancellationToken), _restHttpClient.Get("permissions/ping", null, cancellationToken));
        }

        public Task RevokePermissions(RevokePermissionsRequest request, CancellationToken cancellationToken = default)
        {
            return _restHttpClient.PostAsJson("permissions/revoke", request, cancellationToken);
        }
    }
}