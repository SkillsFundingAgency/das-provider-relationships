using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class UpdatedPermissionsEventAudit : Entity
    {
        public Guid Id { get; set; }
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public Guid UserRef { get; }
        public string GrantedOperations { get; }
        public DateTime Updated { get; }
        public DateTime TimeLogged { get; }

        public UpdatedPermissionsEventAudit(DateTime timeLogged, DateTime updated, string grantedOperations, Guid userRef, long ukprn, long accountProviderLegalEntityId, long accountProviderId, long accountLegalEntityId, long accountId)
        {
            TimeLogged = timeLogged;
            Updated = updated;
            GrantedOperations = grantedOperations;
            UserRef = userRef;
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountId = accountId;
        }
    }
}