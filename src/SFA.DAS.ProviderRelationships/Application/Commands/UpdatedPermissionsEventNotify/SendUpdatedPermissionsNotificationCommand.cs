using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdatedPermissionsEventNotify
{
    public class SendUpdatedPermissionsNotificationCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }

        public SendUpdatedPermissionsNotificationCommand
        (
            long ukprn,
            long accountLegalEntityId
        )
        {
            Ukprn = ukprn;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}
