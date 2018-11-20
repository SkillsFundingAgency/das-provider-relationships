using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class UpdatedAccountLegalEntityNameEvent : Event
    {
        public long AccountLegalEntityId { get; }
        public long AccountId { get; }
        public string Name { get; }

        public UpdatedAccountLegalEntityNameEvent(long accountLegalEntityId, long accountId, string name, DateTime created)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            Name = name;
            Created = created;
        }
    }
}