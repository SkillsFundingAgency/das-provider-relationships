using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class UpdatedPermissionsEventAudit : Entity
    {
        public UpdatedPermissionsEventAudit(DateTime logged, DateTime updated, string grantedOperations, Guid userRef, long ukprn, long accountProviderLegalEntityId, long accountProviderId, long accountLegalEntityId, long accountId)
        {
            Logged = logged;
            Updated = updated;
            GrantedOperations = grantedOperations;
            UserRef = userRef;
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountId = accountId;
        }

        public long Id { get; set; }
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public Guid UserRef { get; }
        public string GrantedOperations { get; }
        public DateTime Updated { get; }
        public DateTime Logged { get; }
    }
}