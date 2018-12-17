using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AddedAccountProviderEventAudit : Entity
    {
        public long Id { get; private set; }
        public long AccountProviderId { get; private set;  }
        public long AccountId { get; private set;  }
        public long ProviderUkprn { get; private set;  }
        public Guid UserRef { get; private set;  }
        public DateTime Added { get; private set;  }
        public DateTime Logged { get; private set;  }
        
        public AddedAccountProviderEventAudit(long accountProviderId, long accountId, long providerUkprn, Guid userRef, DateTime added)
        {
            AccountProviderId = accountProviderId;
            AccountId = accountId;
            ProviderUkprn = providerUkprn;
            UserRef = userRef;
            Added = added;
            Logged = DateTime.UtcNow;
        }

        private AddedAccountProviderEventAudit()
        {
        }
    }
}