using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class DeletedAccountLegalEntityEvent : Event
    {
        public long AccountLegalEntityId { get; }
        public long AccountId { get; }

        public DeletedAccountLegalEntityEvent(long accountLegalEntityId, long accountId, DateTime created)
        {
            AccountLegalEntityId = accountLegalEntityId;
            AccountId = accountId;
            Created = created;
        }
    }
}