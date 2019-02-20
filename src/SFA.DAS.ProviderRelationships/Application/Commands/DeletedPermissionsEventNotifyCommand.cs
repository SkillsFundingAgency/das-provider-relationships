using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class DeletedPermissionsEventNotifyCommand : IRequest
    {
        public long AccountProviderId { get; set; }
        public long AccountId { get; set; }

        public DeletedPermissionsEventNotifyCommand
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