using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AddedAccountProviderEvent : Event
    {
        public int AccountProviderId { get; }
        public long AccountId { get; }
        public long ProviderUkprn { get; }
        public Guid UserRef { get; }

        public AddedAccountProviderEvent(int accountProviderId, long accountId, long providerUkprn, Guid userRef, DateTime created)
        {
            AccountProviderId = accountProviderId;
            AccountId = accountId;
            ProviderUkprn = providerUkprn;
            UserRef = userRef;
            Created = created;
        }
    }
}