using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class UpdatedPermissionsEventAudit : Entity
    {
        public long Id { get; private set; }
        public long AccountId { get; private set; }
        public long AccountLegalEntityId { get; private set; }
        public long AccountProviderId { get; private set; }
        public long AccountProviderLegalEntityId { get; private set; }
        public long Ukprn { get; private set; }
        public Guid? UserRef { get; private set; }
        public string GrantedOperations { get; private set; }
        public DateTime Updated { get; private set; }
        public DateTime Logged { get; private set; }
        
        public UpdatedPermissionsEventAudit(long accountId, long accountLegalEntityId, long accountProviderId, long accountProviderLegalEntityId, long ukprn, Guid? userRef, HashSet<Operation> grantedOperations, DateTime updated)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            UserRef = userRef;
            GrantedOperations = string.Join(",", grantedOperations.Cast<short>());
            Updated = updated;
            Logged = DateTime.UtcNow;
        }

        private UpdatedPermissionsEventAudit()
        {
        }
    }
}