using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
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
            var entity = request.MapToEntity();
            _db.Value.DeletedPermissionsEventAudits.Add(entity);
        }
    }
}