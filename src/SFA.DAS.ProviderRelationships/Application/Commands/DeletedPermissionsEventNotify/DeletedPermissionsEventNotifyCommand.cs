using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.DeletedPermissionsEventNotify
{
    public class DeletedPermissionsEventNotifyCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }

        public DeletedPermissionsEventNotifyCommand
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