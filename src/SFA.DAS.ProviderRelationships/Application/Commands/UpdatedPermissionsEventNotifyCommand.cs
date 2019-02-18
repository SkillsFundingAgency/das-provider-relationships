using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventNotifyCommand : IRequest
    {
        public long AccountProviderId { get; set; }
        public long AccountId { get; set; }

        public UpdatedPermissionsEventNotifyCommand
        (
            long accountProviderId,
            long accountId
        )
        {
            AccountProviderId = accountProviderId;
            AccountId = accountId;
        }
    }
}
