using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class UpdatedAccountNameEvent
    {
        public long AccountId { get; }
        public string Name { get; }
        public DateTime Updated { get;}

        public UpdatedAccountNameEvent(long accountId, string name, DateTime updated)
        {
            AccountId = accountId;
            Name = name;
            Updated = updated;
        }
    }
}