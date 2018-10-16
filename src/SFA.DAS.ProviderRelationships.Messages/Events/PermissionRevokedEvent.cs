using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class PermissionRevokedEvent : Event
    {
        public long PermissionId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public int Type { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }
}