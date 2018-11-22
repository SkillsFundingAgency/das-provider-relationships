using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class DeletedPermissionsEvent
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long Ukprn { get; }
        public DateTime Deleted { get; }

        public DeletedPermissionsEvent(long accountId, long accountLegalEntityId, long accountProviderId, long ukprn, DateTime deleted)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            Ukprn = ukprn;
            Deleted = deleted;
        }
    }
}