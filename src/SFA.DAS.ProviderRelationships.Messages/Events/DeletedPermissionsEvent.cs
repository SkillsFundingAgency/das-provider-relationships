using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class DeletedPermissionsEvent
    {
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public long AccountId { get; }
        public long AccountProviderId { get; }
        public DateTime Deleted { get; }

        public DeletedPermissionsEvent(long accountProviderLegalEntityId, long ukprn, long accountId, long accountProviderId, DateTime deleted)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            AccountId = accountId;
            AccountProviderId = accountProviderId;
            Deleted = deleted;
        }
    }
}