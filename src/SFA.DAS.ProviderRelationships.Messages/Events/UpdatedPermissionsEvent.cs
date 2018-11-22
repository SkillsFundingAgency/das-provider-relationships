using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Messages.Events
{
    public class UpdatedPermissionsEvent
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long Ukprn { get; }
        public Guid UserRef { get; }
        public HashSet<Operation> Operations { get; }
        public DateTime Updated { get;}

        public UpdatedPermissionsEvent(long accountId, long accountLegalEntityId, long accountProviderId, long ukprn, Guid userRef, HashSet<Operation> operations, DateTime updated)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            Ukprn = ukprn;
            UserRef = userRef;
            Operations = operations;
            Updated = updated;
        }
    }
}