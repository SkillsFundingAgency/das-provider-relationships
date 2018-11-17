using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class ChangedAccountNameEvent : Event
    {
        public long AccountId { get; }
        public string Name { get; }

        public ChangedAccountNameEvent(long accountId, string name, DateTime created)
        {
            AccountId = accountId;
            Name = name;
            Created = created;
        }
    }
}