using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventNotifyCommandHandler : AsyncRequestHandler<UpdatedPermissionsEventNotifyCommand>
    {
        private readonly IPasAccountApiClient _client;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private const string TemplateId = "UpdatedPermissionsEventNotification";

        public UpdatedPermissionsEventNotifyCommandHandler(IPasAccountApiClient client, Lazy<ProviderRelationshipsDbContext> db)
        {
            _client = client;
            _db = db;
        }

        protected override async Task Handle(UpdatedPermissionsEventNotifyCommand request, CancellationToken cancellationToken)
        {
            var organisation = await _db.Value.Accounts.SingleAsync(a => a.Id == request.AccountId, cancellationToken);

            await _client.SendEmailToAllProviderRecipients(request.Ukprn, new ProviderEmailRequest {
                TemplateId = TemplateId,
                Tokens = new Dictionary<string, string> {
                    { "organisation_name", organisation.Name }
                }
            });
        }
    }
}