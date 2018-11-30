using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IRestClient _restClient;
        private readonly IReadStoreMediator _mediator;

        public ProviderRelationshipsApiClient(IRestClient restClient, IReadStoreMediator mediator)
        {
            _restClient = restClient;
            _mediator = mediator;
        }

        public async Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission(
                GetAccountProviderLegalEntitiesWithPermissionRequest withPermissionRequest, CancellationToken cancellationToken = default)
        {
            return await _restClient.Get<GetAccountProviderLegalEntitiesWithPermissionResponse>("accountproviderlegalentities", withPermissionRequest, cancellationToken);
        }

        public Task<bool> HasPermission(HasPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasPermissionQuery(request.Ukprn, request.AccountLegalEntityId, request.Operation), cancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission(HasRelationshipWithPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken);
        }

        public Task HealthCheck()
        {
            return _restClient.Get("healthcheck");
        }
    }
}