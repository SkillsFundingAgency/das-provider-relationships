using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
{
    public class DeletedPermissionsEventAuditCommand : IRequest
    {
        public long AccountProviderLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public DateTime Deleted { get; set; }
    }
}
