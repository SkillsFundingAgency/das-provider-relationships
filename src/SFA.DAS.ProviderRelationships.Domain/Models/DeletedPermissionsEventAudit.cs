using System;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class DeletedPermissionsEventAudit : Entity
    {
        public long Id { get; private set;  }
        public long AccountProviderLegalEntityId { get; private set; }
        public long Ukprn { get; private set; }
        public DateTime Deleted { get; private set; }
        public DateTime Logged { get; private set; }
        
        public DeletedPermissionsEventAudit(long accountProviderLegalEntityId, long ukprn, DateTime deleted)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Deleted = deleted;
            Logged = DateTime.UtcNow;
        }

        private DeletedPermissionsEventAudit()
        {
        }
    }
}