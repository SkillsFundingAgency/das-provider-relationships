using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class AddedAccountProviderEventAudit
    {
        public AddedAccountProviderEventAudit(long accountProviderId, long accountId, long providerUkprn, Guid userRef, DateTime added, DateTime logged)
        {
            AccountProviderId = accountProviderId;
            AccountId = accountId;
            ProviderUkprn = providerUkprn;
            UserRef = userRef;
            Added = added;
            Logged = logged;
        }

        public long Id { get; set; }
        public long AccountProviderId { get; }
        public long AccountId { get; }
        public long ProviderUkprn { get; }
        public Guid UserRef { get; }
        public DateTime Added { get; }
        public DateTime Logged { get; }
    }
}