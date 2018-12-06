using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
{
    public class UpdatedPermissionsEventAuditCommand : IRequest
    {
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public long AccountProviderId { get; set; }
        public long AccountProviderLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public Guid UserRef { get; set; }
        public List<Operation> GrantedOperations { get; set; }
        public DateTime Updated { get; set; }
    }

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