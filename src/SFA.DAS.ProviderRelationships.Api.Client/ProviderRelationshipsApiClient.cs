using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.Ping;
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

        public async Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission(GetAccountProviderLegalEntitiesWithPermissionRequest withPermissionRequest, CancellationToken cancellationToken = default)
        {
            return await _restHttpClient.Get<GetAccountProviderLegalEntitiesWithPermissionResponse>("accountproviderlegalentities", withPermissionRequest, cancellationToken);
        }

        public async Task<InvitationDto> GetInvitation(string correlationId, CancellationToken cancellationToken = default)
        {
            return await _restHttpClient.Get<InvitationDto>($"invitations/{correlationId}", null, cancellationToken);
        }

        public Task<bool> HasPermission(HasPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasPermissionQuery(request.Ukprn, request.AccountLegalEntityId, request.Operation), cancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission(HasRelationshipWithPermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken);
        }

        public Task Ping(CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(_restHttpClient.Get("ping", null, cancellationToken), _mediator.Send(new PingQuery(), cancellationToken));
        }

        public Task RevokePermissions(RevokePermissionsRequest request, CancellationToken cancellationToken = default)
        {
            return _restHttpClient.PostAsJson("permissions/revoke", request, cancellationToken);
        }
    }
}