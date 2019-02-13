using System.Collections.Generic;
using MediatR;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventNotifyCommandHandler : RequestHandler<UpdatedPermissionsEventNotifyCommand>
    {
        private readonly IAccountApiClient _client;
        private const string TemplateId = "UpdatedPermissionsEventNotification";

        public UpdatedPermissionsEventNotifyCommandHandler(IAccountApiClient client)
        {
            _client = client;
        }

        protected override void Handle(UpdatedPermissionsEventNotifyCommand request)
        {
            _client.SendEmailToAllProviderRecipients(request.Ukprn, new ProviderEmailRequest {
                TemplateId = TemplateId,
                Tokens = new Dictionary<string, string> {
                    { "AccountProvider", request.Ukprn.ToString() },
                    { "GrantedOperations", request.GrantedOperations.ToString() }
                }
            });
        }
    }
}