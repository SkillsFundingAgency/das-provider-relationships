using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class UpdatedLegalEntityEvent : Event
    {
        public long AccountLegalEntityId { get; }
        public string Name { get; }

        public UpdatedLegalEntityEvent(long accountLegalEntityId, string name, DateTime created)
        {
            AccountLegalEntityId = accountLegalEntityId;
            Name = name;
            Created = created;
        }
    }
}