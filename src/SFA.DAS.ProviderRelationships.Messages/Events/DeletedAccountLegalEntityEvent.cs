using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class DeletedAccountLegalEntityEvent
    {
        public long AccountLegalEntityId { get; }
        public long AccountId { get; }
        public DateTime Deleted { get;}

        public DeletedAccountLegalEntityEvent(long accountLegalEntityId, long accountId, DateTime deleted)
        {
            AccountLegalEntityId = accountLegalEntityId;
            AccountId = accountId;
            Deleted = deleted;
        }
    }
}