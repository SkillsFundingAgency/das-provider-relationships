using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventAuditCommandHandler : RequestHandler<UpdatedPermissionsEventAuditCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public UpdatedPermissionsEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(UpdatedPermissionsEventAuditCommand request)
        {
            _db.Value.UpdatedPermissionsEventAudits.Add(new UpdatedPermissionsEventAudit(DateTime.UtcNow,
                request.Updated, request.GrantedOperations, request.UserRef, request.Ukprn,
                request.AccountProviderLegalEntityId, request.AccountProviderId, request.AccountLegalEntityId,
                request.AccountId));
        }
    }
}