using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventNotifyCommandHandler : RequestHandler<UpdatedPermissionsEventNotifyCommand>
    {
        private readonly IPasAccountApiClient _client;
        private const string TemplateId = "UpdatedPermissionsEventNotification";

        public UpdatedPermissionsEventNotifyCommandHandler(IPasAccountApiClient client)
        {
            _client = client;
        }

        protected override void Handle(UpdatedPermissionsEventNotifyCommand request)
        {
            _client.SendEmailToAllProviderRecipients(request.AccountProviderId, new ProviderEmailRequest {
                TemplateId = TemplateId,
                Tokens = new Dictionary<string, string> {
                    { "Ukprn", request.Ukprn.ToString() },
                    { "GrantedOperations", BuildOperationsString(request.GrantedOperations) }
                }
            });
        }

        public string BuildOperationsString(HashSet<Operation> grantedOperations)
        {
            var builder = new StringBuilder();
            var comma = false;
            foreach (var operation in grantedOperations)
            {
                if (comma) { builder.Append(", "); }
                builder.Append(operation.ToString());
                comma = true;
            }

            return builder.ToString();
        }
    }
}