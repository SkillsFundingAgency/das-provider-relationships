using System.Collections.Generic;
using MediatR;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventNotifyCommand : IRequest
    {
        public long AccountProviderId { get; set; }
        public long Ukprn { get; }
        public HashSet<Operation> GrantedOperations { get; }

        public UpdatedPermissionsEventNotifyCommand
        (
            long accountProviderId,
            long ukprn,
            HashSet<Operation> grantedOperations
        )
        {
            AccountProviderId = accountProviderId;
            Ukprn = ukprn;
            GrantedOperations = grantedOperations;
        }
    }

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
