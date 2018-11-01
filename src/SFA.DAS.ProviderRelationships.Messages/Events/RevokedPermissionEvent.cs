using System;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class RevokedPermissionEvent : Event
    {
        public long Id { get; }
        public long AccountLegalEntityId { get; }
        public long Ukprn { get; }
        public short Type { get; }
        public string UserName { get; }
        public Guid UserRef { get; }

        public RevokedPermissionEvent(long id, long accountLegalEntityId, long ukprn, short type, string userName, Guid userRef, DateTime created)
        {
            Id = id;
            AccountLegalEntityId = accountLegalEntityId;
            Ukprn = ukprn;
            Type = type;
            UserName = userName;
            UserRef = userRef;
            Created = created;
        }
    }
}