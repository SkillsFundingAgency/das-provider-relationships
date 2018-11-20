using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityCreatedEvent : Event
    {
        public long Ukprn { get; }
        public long AccountId { get; }
        public string AccountPublicHashedId { get; }
        public string AccountName { get; }


        public long AccountLegalEntityId { get; }
        public string AccountLegalEntityPublicHashedId { get;  }
        public string AccountLegalEntityName { get;  }


        public int AccountProviderId { get;  }
        public string AccountProviderName { get; }

        public AccountProviderLegalEntityCreatedEvent(long ukprn, long accountId, string accountPublicHashedId, string accountName,
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName,
            int accountProviderId, string accountProviderName, DateTime created)
        {
            Ukprn = ukprn;

            AccountId = accountId;
            AccountPublicHashedId = accountPublicHashedId;
            AccountName = accountName;

            AccountLegalEntityId = accountLegalEntityId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            AccountLegalEntityName = accountLegalEntityName;

            AccountProviderId = accountProviderId;
            AccountProviderName = accountProviderName;

            Created = created;
        }

    }
}