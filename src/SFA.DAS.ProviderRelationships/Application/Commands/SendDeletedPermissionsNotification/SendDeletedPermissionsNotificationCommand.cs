using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification
{
    public class SendDeletedPermissionsNotificationCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }

        public SendDeletedPermissionsNotificationCommand
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