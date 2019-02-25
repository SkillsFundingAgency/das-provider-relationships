using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdatedPermissionsEventNotify
{
    public class UpdatedPermissionsEventNotifyCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }

        public UpdatedPermissionsEventNotifyCommand
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
