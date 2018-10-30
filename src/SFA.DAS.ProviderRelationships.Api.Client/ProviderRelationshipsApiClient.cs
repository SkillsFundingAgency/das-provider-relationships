using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IProviderRelationshipService _service;

        public ProviderRelationshipsApiClient(IProviderRelationshipService service)
        {
            _service = service;
        }

        public Task<ProviderRelationshipsResponse> GetRelationshipsWithPermission(ProviderRelationshipsRequest request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasPermission(ProviderPermissionRequest request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasRelationshipWithPermission(ProviderRelationshipsRequest request, CancellationToken cancellationToken = default) =>
            _service.HasRelationshipWithPermission(request.Ukprn, request.Operation, cancellationToken);
    }
}