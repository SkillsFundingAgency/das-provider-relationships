using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IRestHttpClient _restHttpClient;
        private readonly IMediator _mediator;

        public ProviderRelationshipsApiClient(IRestHttpClient restHttpClient, IMediator mediator)
        {
            _restHttpClient = restHttpClient;
            _mediator = mediator;
        }

        public async Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission(
                GetAccountProviderLegalEntitiesWithPermissionRequest withPermissionRequest, CancellationToken cancellationToken = default)
        {
            return await _restHttpClient.Get<GetAccountProviderLegalEntitiesWithPermissionResponse>("accountproviderlegalentities", withPermissionRequest, cancellationToken);
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
            return _restHttpClient.Get("healthcheck");
        }
    }
}