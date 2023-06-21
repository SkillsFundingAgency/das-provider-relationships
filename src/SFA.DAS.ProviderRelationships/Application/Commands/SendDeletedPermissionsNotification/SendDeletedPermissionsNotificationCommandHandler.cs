using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PAS.Account.Api.ClientV2;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification;

public class SendDeletedPermissionsNotificationCommandHandler : IRequestHandler<SendDeletedPermissionsNotificationCommand>
{
    private readonly IPasAccountApiClient _client;
    private readonly Lazy<ProviderRelationshipsDbContext> _db;
    private const string TemplateId = "DeletedPermissionsEventNotification";

    public SendDeletedPermissionsNotificationCommandHandler(IPasAccountApiClient client, Lazy<ProviderRelationshipsDbContext> db)
    {
        _client = client;
        _db = db;
    }

    public async Task Handle(SendDeletedPermissionsNotificationCommand request, CancellationToken cancellationToken)
    {
        var organisation = await _db.Value.AccountLegalEntities.IgnoreQueryFilters().SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

        var providerEmailRequest = new ProviderEmailRequest {
            TemplateId = TemplateId,
            Tokens = new Dictionary<string, string> {
                { "organisation_name", organisation.Name }
            }
        };

        await _client.SendEmailToAllProviderRecipients(request.Ukprn, providerEmailRequest, cancellationToken);
    }
}