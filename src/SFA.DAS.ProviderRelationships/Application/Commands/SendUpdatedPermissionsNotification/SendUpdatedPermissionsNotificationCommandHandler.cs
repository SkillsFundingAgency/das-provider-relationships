using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification
{
    public class SendUpdatedPermissionsNotificationCommandHandler : AsyncRequestHandler<SendUpdatedPermissionsNotificationCommand>
    {
        private readonly IPasAccountApiClient _client;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private const string TemplateId = "UpdatedPermissionsEventNotification";

        public SendUpdatedPermissionsNotificationCommandHandler(IPasAccountApiClient client, Lazy<ProviderRelationshipsDbContext> db)
        {
            _client = client;
            _db = db;
        }

        protected override async Task Handle(SendUpdatedPermissionsNotificationCommand request, CancellationToken cancellationToken)
        {
            var organisation = await _db.Value.AccountLegalEntities.SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

            var provider = await _db.Value.Providers.SingleAsync(p => p.Ukprn == request.Ukprn, cancellationToken);

            var accountProviderLegalEntity = await _db.Value.AccountProviderLegalEntities
               .Include(x => x.AccountProvider)
               .Include(x => x.AccountLegalEntity)
               .Include(x => x.Permissions)               
               .Where(x => x.AccountProvider.ProviderUkprn == request.Ukprn)
               .Where(x => x.AccountLegalEntity.Id == request.AccountLegalEntityId)
               .SingleOrDefaultAsync(cancellationToken);

            var permissions = accountProviderLegalEntity?.Permissions.Select(x => x.Operation);            

            var operations = string.Join(" and ", permissions?
              .Where(ope => !string.IsNullOrEmpty(ope.ToString()))
              .Select(ope => ope.GetDisplayName()));
           

            await _client.SendEmailToAllProviderRecipients(request.Ukprn, new ProviderEmailRequest {
                TemplateId = TemplateId,
                Tokens = new Dictionary<string, string> {
                    { "training_provider_name", provider.Name },
                    { "organisation_name", organisation.Name },
                    { "permissions_set", operations }
                }
            });
        }
    }
}