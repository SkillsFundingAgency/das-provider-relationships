using System;
using System.Collections.Generic;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityUserUpdatedPermissionsEvent : Event
    {
        public long Ukprn { get; }
        public long AccountProviderLegalEntityId { get; }
        public HashSet<Operation> Operations { get; }
        public Guid UserRef { get; }


        public AccountProviderLegalEntityUserUpdatedPermissionsEvent(long ukprn, long accountProviderLegalEntityId, Guid userRef, HashSet<Operation> operations, DateTime created)
        {
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            UserRef = userRef;
            Operations = operations;
            Created = created;
        }
    }
}