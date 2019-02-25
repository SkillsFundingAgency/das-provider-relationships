using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdatedPermissionsEventNotify
{
    public class UpdatedPermissionsEventNotifyCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountId { get; set; }

        public UpdatedPermissionsEventNotifyCommand
        (
            long ukprn,
            long accountId
        )
        {
            Ukprn = ukprn;
            AccountId = accountId;
        }
    }
}
