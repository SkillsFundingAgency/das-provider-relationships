using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityDeletedEvent
    {
        public long Ukprn { get; }
        public long AccountProviderLegalEntityId { get; }
        public DateTime Created { get;}

        public AccountProviderLegalEntityDeletedEvent(long ukprn, long accountProviderLegalEntityId, DateTime created)
        {
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Created = created;
        }
    }
}