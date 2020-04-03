using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification
{
    public class SendUpdatedPermissionsNotificationCommandHandler : AsyncRequestHandler<SendUpdatedPermissionsNotificationCommand>
    {
        private readonly IPasAccountApiClient _client;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private const string TemplateId = "UpdatedProviderPermissionsNotification";

        public SendUpdatedPermissionsNotificationCommandHandler(IPasAccountApiClient client, Lazy<ProviderRelationshipsDbContext> db)
        {
            _client = client;
            _db = db;
        }

        protected override async Task Handle(SendUpdatedPermissionsNotificationCommand request, CancellationToken cancellationToken)
        {
            var organisation = await _db.Value.AccountLegalEntities.SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

            var provider = await _db.Value.Providers.SingleAsync(p => p.Ukprn == request.Ukprn, cancellationToken);

            var permissionUpdatesTokens = GetPermissionsUpdatedTokens(request.PreviousOperations, request.GrantedOperations, organisation.Name);

            if (permissionUpdatesTokens.Any())
            {
                await _client.SendEmailToAllProviderRecipients(request.Ukprn, new ProviderEmailRequest {
                    TemplateId = TemplateId,
                    Tokens = new Dictionary<string, string>
                    {
                        { "training_provider_name", provider.Name },
                        { "organisation_name", organisation.Name },
                    }
                    .Concat(permissionUpdatesTokens)
                    .ToDictionary(x => x.Key, x => x.Value)
                });
            }
        }

        private Dictionary<string, string> GetPermissionsUpdatedTokens(HashSet<Operation> previousOperations, HashSet<Operation> grantedOperations, string organisationName)
        {
            var permissionsUpdatedTokens = new Dictionary<string, string>();
            var removedOperations = previousOperations.Except(grantedOperations);
            var newOperations = grantedOperations.Except(previousOperations);

            if (removedOperations.Any())
            {
                if (!newOperations.Any())
                {
                    var remainingOperationsText = grantedOperations.Count > 0
                        ? $"You can still {GetOperationText(grantedOperations)} on their behalf."
                        : $"You cannot do anything in the apprenticeship service on their behalf at the moment.";

                    permissionsUpdatedTokens.Add("part1_text", $"removed your permission to {GetOperationText(removedOperations)}.");
                    permissionsUpdatedTokens.Add("part2_text", $"{remainingOperationsText}");
                }
                else
                {
                    permissionsUpdatedTokens.Add("part1_text", ":");
                    permissionsUpdatedTokens.Add(
                        "part2_text",
                        $"\u2022 given you permission to {GetOperationText(newOperations)}" +
                        $"{Environment.NewLine}" +
                        $"\u2022 removed your permission to {GetOperationText(removedOperations)}");
                }
            }
            else
            {
                if (newOperations.Any())
                {
                    permissionsUpdatedTokens.Add("part1_text", "changed your apprenticeship service permissions.");
                    permissionsUpdatedTokens.Add("part2_text", $"You can now { GetOperationText(grantedOperations)} on their behalf.");
                }
            }

            return permissionsUpdatedTokens;
        }

        private string GetOperationText(IEnumerable<Operation> operations)
        {
            return  string.Join(" and ", operations?.OrderBy(x => x)
              .Where(ope => !string.IsNullOrEmpty(ope.ToString()))
              .Select(ope => ope.GetDisplayName().ToLower()));
        }
    }
}