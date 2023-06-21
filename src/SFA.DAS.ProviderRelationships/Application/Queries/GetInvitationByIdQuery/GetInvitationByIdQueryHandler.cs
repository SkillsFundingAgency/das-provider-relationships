using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;

public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>
{
    private readonly IRegistrationApiClient _registrationApiClient;
    private readonly ILogger<GetInvitationByIdQueryHandler> _logger;

    public GetInvitationByIdQueryHandler(IRegistrationApiClient registrationApiClient, ILogger<GetInvitationByIdQueryHandler> logger)
    {
        _registrationApiClient = registrationApiClient;
        _logger = logger;
    }

    public async Task<GetInvitationByIdQueryResult> Handle(GetInvitationByIdQuery message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Invitations for {CorrelationId}", message.CorrelationId);

        var json = await _registrationApiClient.GetInvitations(message.CorrelationId.ToString(), cancellationToken);

        _logger.LogInformation("Request sent Get Invitations for {CorrelationId} {Json}", message.CorrelationId, json);

        return new GetInvitationByIdQueryResult(json == null ? null : JsonConvert.DeserializeObject<InvitationDto>(json)) { };
    }
}