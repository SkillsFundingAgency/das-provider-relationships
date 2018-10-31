using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.ReadStore.Application;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IMediator _mediator;

        public ProviderRelationshipsApiClient(IMediator mediator)
        {
            _mediator = mediator;
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
            _mediator.Send(new HasRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken);
    }
}