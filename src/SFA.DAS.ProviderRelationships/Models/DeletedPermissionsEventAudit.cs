using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class DeletedPermissionsEventAudit
    {
        public Guid Id { get; set; }
        public long AccountProviderLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public DateTime Deleted { get; set; }
        public DateTime TimeLogged { get; set; }
    }
}