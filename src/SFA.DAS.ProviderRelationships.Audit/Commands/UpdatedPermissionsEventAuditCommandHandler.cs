using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
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
            var entity = request.MapToEntity();
            _db.Value.UpdatedPermissionsEventAudits.Add(entity);
        }
    }
}