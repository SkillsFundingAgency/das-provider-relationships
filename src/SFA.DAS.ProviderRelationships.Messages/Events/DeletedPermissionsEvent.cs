using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class DeletedPermissionsEvent
    {
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public DateTime Deleted { get; }

        public DeletedPermissionsEvent(long accountProviderLegalEntityId, long ukprn, DateTime deleted)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Deleted = deleted;
        }
    }
}