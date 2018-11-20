using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class DeleteAccountLegalEntityPermissionsCommand : IRequest
    {
        public long AccountLegalEntityId { get; }

        public DeleteAccountLegalEntityPermissionsCommand(long accountLegalEntityId)
        {
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}