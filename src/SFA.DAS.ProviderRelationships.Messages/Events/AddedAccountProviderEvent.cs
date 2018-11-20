using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AddedAccountProviderEvent
    {
        public long AccountProviderId { get; }
        public long AccountId { get; }
        public long ProviderUkprn { get; }
        public Guid UserRef { get; }
        public DateTime Added { get;}

        public AddedAccountProviderEvent(long accountProviderId, long accountId, long providerUkprn, Guid userRef, DateTime added)
        {
            AccountProviderId = accountProviderId;
            AccountId = accountId;
            ProviderUkprn = providerUkprn;
            UserRef = userRef;
            Added = added;
        }
    }
}