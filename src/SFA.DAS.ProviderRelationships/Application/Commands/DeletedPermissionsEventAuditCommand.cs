using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class DeletedPermissionsEventAuditCommand : IRequest
    {
        public DeletedPermissionsEventAuditCommand(long accountProviderLegalEntityId, long ukprn, DateTime deleted)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Deleted = deleted;
        }

        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public DateTime Deleted { get; }
    }
}
