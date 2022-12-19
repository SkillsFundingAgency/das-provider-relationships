using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery
{
    public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>
    {
        private IRegistrationApiClient _registrationService;
        private readonly ILogger<GetInvitationByIdQueryHandler> _logger;

        public GetInvitationByIdQueryHandler(IRegistrationApiClient registrationService, ILogger<GetInvitationByIdQueryHandler> logger)
        {
            _registrationService = registrationService;
            _logger = logger;
        }

        public async Task<GetInvitationByIdQueryResult> Handle(GetInvitationByIdQuery message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get Invitations for {message.CorrelationId}");
            var json = await _registrationService.GetInvitations(message.CorrelationId.ToString(), cancellationToken);
            _logger.LogInformation($"Request sent Get Invitations for {message.CorrelationId} {json}");
            return new GetInvitationByIdQueryResult(json == null ? null : JsonConvert.DeserializeObject<InvitationDto>(json)) { };
        }
    }
}