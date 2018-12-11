using System;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions
{
    public class DeletePermissionsCommand : IReadStoreRequest<Unit>
    {
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public DateTime Deleted { get; }
        public string MessageId { get; }

        public DeletePermissionsCommand(long accountProviderLegalEntityId, long ukprn, DateTime deleted, string messageId)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Deleted = deleted;
            MessageId = messageId;
        }
    }
}