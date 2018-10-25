using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
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

        public Task<bool> HasRelationshipWithPermission(ProviderRelationshipRequest request) =>
            _mediator.Send(new GetHasRelationshipWithPermissionQuery {
                Ukprn = request.Ukprn,
                Permission = request.Permission
            });

        public Task<ProviderRelationshipResponse> ListRelationshipsWithPermission(ProviderRelationshipRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}