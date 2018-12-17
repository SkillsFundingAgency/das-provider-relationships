using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class DeletedPermissionsEventAuditCommandHandler : RequestHandler<DeletedPermissionsEventAuditCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public DeletedPermissionsEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(DeletedPermissionsEventAuditCommand request)
        {
            var audit = new DeletedPermissionsEventAudit(request.AccountProviderLegalEntityId, request.Ukprn, request.Deleted);
            
            _db.Value.DeletedPermissionsEventAudits.Add(audit);
        }
    }
}