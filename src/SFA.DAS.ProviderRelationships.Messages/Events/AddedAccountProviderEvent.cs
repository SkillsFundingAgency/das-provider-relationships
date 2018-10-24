using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AddedAccountProviderEvent : Event
    {
        public int AccountProviderId { get; set; }
        public long AccountId { get; set; }
        public long ProviderUkprn { get; set; }
        public Guid UserRef { get; set; }
    }
}