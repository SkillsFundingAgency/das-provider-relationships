using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Helpers;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification
{
    public class SendUpdatedPermissionsNotificationCommandHandler : AsyncRequestHandler<SendUpdatedPermissionsNotificationCommand>
    {
        private readonly IPasAccountApiClient _client;
        private readonly IProviderUrls _providerUrls;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private const string TemplateId = "UpdatedProviderPermissionsNotification";

        public SendUpdatedPermissionsNotificationCommandHandler(IPasAccountApiClient client, IProviderUrls providerUrls, Lazy<ProviderRelationshipsDbContext> db)
        {
            _client = client;
            _providerUrls = providerUrls;
            _db = db;
        }

        protected override async Task Handle(SendUpdatedPermissionsNotificationCommand request, CancellationToken cancellationToken)
        {
            var organisation = await _db.Value.AccountLegalEntities.SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

            var provider = await _db.Value.Providers.SingleAsync(p => p.Ukprn == request.Ukprn, cancellationToken);

            var manageNotificationsUrl = _providerUrls.ProviderManageRecruitEmails(request.Ukprn.ToString());

            var permissionUpdatesTokens = GetPermissionsUpdatedTokens(request.GrantedOperations);

            if (permissionUpdatesTokens.Any())
            {
                await _client.SendEmailToAllProviderRecipients(request.Ukprn, new ProviderEmailRequest {
                    TemplateId = TemplateId,
                    Tokens = new Dictionary<string, string>
                    {
                        { "training_provider_name", provider.Name },
                        { "organisation_name", organisation.Name },
                        { "manage_recruitment_emails_url", manageNotificationsUrl },
                    }
                    .Concat(permissionUpdatesTokens)
                    .ToDictionary(x => x.Key, x => x.Value)
                });
            }
        }

        private Dictionary<string, string> GetPermissionsUpdatedTokens(HashSet<Operation> grantedOperations)
        {
            var permissionsUpdatedTokens = new Dictionary<string, string>();
            var part2 = string.Empty;

            if (grantedOperations.Contains(Operation.CreateCohort))
            {
                part2 += "\u2022 add apprentice records" + Environment.NewLine;
            }
            else
            {
                part2 += "\u2022 cannot add apprentice records" + Environment.NewLine;
            }

            if (grantedOperations.Contains(Operation.RecruitmentRequiresReview))
            {
                part2 += "\u2022 create job adverts" + Environment.NewLine;
            }
            else if (grantedOperations.Contains(Operation.Recruitment))
            {
                part2 += "\u2022 create and publish job adverts" + Environment.NewLine;
            }
            else
            {
                part2 += "\u2022 cannot create job adverts" + Environment.NewLine;
            }

            permissionsUpdatedTokens.Add("part1_text", "set your apprenticeship service permissions to:");
            permissionsUpdatedTokens.Add("part2_text", part2);

            return permissionsUpdatedTokens;
        }
    }
}