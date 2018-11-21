using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityUdatedPermissionsEvent
    {
        public long Ukprn { get; }
        public int AccountProviderId { get; }
        public string AccountProviderName { get; }

        public long AccountId { get; }
        public string AccountPublicHashedId { get; }
        public string AccountName { get; }

        public long AccountLegalEntityId { get; }
        public string AccountLegalEntityPublicHashedId { get;  }
        public string AccountLegalEntityName { get;  }

        public HashSet<Operation> Operations { get; }

        public DateTime Created { get;}
        
        public AccountProviderLegalEntityUdatedPermissionsEvent(long ukprn, int accountProviderId, string accountProviderName, 
            long accountId, string accountPublicHashedId, string accountName,
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName,
            HashSet<Operation> operations, DateTime created)
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

            Operations = operations;
            Created = created;
        }
    }
}