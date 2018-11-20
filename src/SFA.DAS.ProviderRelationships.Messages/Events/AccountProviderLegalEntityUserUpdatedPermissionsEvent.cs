using System;
using System.Collections.Generic;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class AccountProviderLegalEntityUserUpdatedPermissionsEvent : Event
    {
        public long Ukprn { get; }
        public int AccountProviderId { get; }
        public long AccountLegalEntityId { get; }
        public HashSet<Operation> Operations { get; }
        public Guid UserRef { get; }


        public AccountProviderLegalEntityUserUpdatedPermissionsEvent(long ukprn, int accountProviderId, long accountLegalEntityId, Guid userRef, HashSet<Operation> operations, DateTime created)
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
            UserRef = userRef;
            Operations = operations;
            Created = created;
        }
    }
}