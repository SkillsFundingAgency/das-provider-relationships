using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.Models;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IMediator _mediator;

        public ProviderRelationshipsApiClient(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<RelationshipsResponse> GetRelationshipsWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasPermission(PermissionRequest request, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasRelationshipWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default) =>
            _mediator.Send(new HasRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken);
    }
}