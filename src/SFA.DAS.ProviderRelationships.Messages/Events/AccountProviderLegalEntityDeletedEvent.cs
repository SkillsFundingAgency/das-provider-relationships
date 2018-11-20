using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityDeletedEvent : Event
    {
        public long Ukprn { get; }
        public long AccountProviderId { get; }
        public long AccountLegalEntityId { get; }

        public AccountProviderLegalEntityDeletedEvent(long ukprn, long accountProviderId, long accountLegalEntityId, DateTime created)
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
            Created = created;
        }
    }
}