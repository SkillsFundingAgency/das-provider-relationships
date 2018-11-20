using System;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class UpdatedAccountLegalEntityNameEvent
    {
        public long AccountLegalEntityId { get; }
        public long AccountId { get; }
        public string Name { get; }
        public DateTime Updated { get;}

        public UpdatedAccountLegalEntityNameEvent(long accountLegalEntityId, long accountId, string name, DateTime updated)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            Name = name;
            Updated = updated;
        }
    }
}