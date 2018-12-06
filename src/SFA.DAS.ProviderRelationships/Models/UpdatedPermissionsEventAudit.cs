using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class UpdatedPermissionsEventAudit : Entity
    {
        public Guid Id { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long AccountProviderId { get; set; }
        public long AccountProviderLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public Guid UserRef { get; set; }
        public string GrantedOperations { get; set; }
        public DateTime Updated { get; set; }
        public DateTime TimeLogged { get; set; }
    }
}