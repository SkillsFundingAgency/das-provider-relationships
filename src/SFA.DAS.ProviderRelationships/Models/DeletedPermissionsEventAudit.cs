using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class DeletedPermissionsEventAudit
    {
        public DeletedPermissionsEventAudit(long accountProviderLegalEntityId, long ukprn, DateTime deleted, DateTime logged)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Deleted = deleted;
            Logged = logged;
        }

        public long Id { get; set; }
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public DateTime Deleted { get; }
        public DateTime Logged { get; }
    }
}