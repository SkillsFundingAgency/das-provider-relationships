using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IApiMediator _mediator;

        public ProviderRelationshipsApiClient(IApiMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RelationshipsResponse> GetRelationshipsWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken).ConfigureAwait(false);
            
            return new RelationshipsResponse
            {
                Relationships = result.Relationships
            };
        }

        public Task<bool> HasPermission(PermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasPermissionQuery(request.Ukprn, request.EmployerAccountLegalEntityId, request.Operation), cancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken);
        }
    }
}