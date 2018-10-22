using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class RevokedPermissionEvent : Event
    {
        public long Id { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public short Type { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }
}