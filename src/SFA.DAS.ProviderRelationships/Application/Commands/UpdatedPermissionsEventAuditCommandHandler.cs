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
            var audit = new UpdatedPermissionsEventAudit(
                request.AccountId,
                request.AccountLegalEntityId,
                request.AccountProviderId,
                request.AccountProviderLegalEntityId,
                request.Ukprn,
                request.UserRef,
                request.GrantedOperations,
                request.Updated);
            
            _db.Value.UpdatedPermissionsEventAudits.Add(audit);
        }
    }
}