using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class DeletedPermissionsEventNotifyCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountId { get; set; }

        public DeletedPermissionsEventNotifyCommand
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