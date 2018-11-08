using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityDeletedEvent : Event
    {
        public long Ukprn { get; }
        public long AccountProviderLegalEntityId { get; }

        public AccountProviderLegalEntityDeletedEvent(long ukprn, long accountProviderLegalEntityId, DateTime created)
        {
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Created = created;
        }
    }
}