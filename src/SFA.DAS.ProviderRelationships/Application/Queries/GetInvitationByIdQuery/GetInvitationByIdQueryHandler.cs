using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery
{
    public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>
    {
        private IRegistrationService _registrationService;
     
        public GetInvitationByIdQueryHandler(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<GetInvitationByIdQueryResult> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
        {
            var invitation = await _registrationService.GetInvitationById(request.CorrelationId);
            return invitation == null ? null : new GetInvitationByIdQueryResult(invitation);
        }
    }
}