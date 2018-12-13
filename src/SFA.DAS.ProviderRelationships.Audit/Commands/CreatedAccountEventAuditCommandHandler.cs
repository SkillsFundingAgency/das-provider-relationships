using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
{
    public class CreatedAccountEventAuditCommandHandler : RequestHandler<CreatedAccountEventAuditCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public CreatedAccountEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(CreatedAccountEventAuditCommand request)
        {
            var entity = MapToEntity(request);
            _db.Value.CreatedAccountEventAudits.Add(entity);
        }

        private static CreatedAccountEventAudit MapToEntity(CreatedAccountEventAuditCommand command)
        {
            return new CreatedAccountEventAudit {
                Name = command.Name,
                AccountId = command.AccountId,
                PublicHashedId = command.PublicHashedId,
                UserName = command.UserName,
                UserRef = command.UserRef,
                TimeLogged = DateTime.UtcNow,
                HashedId = command.HashedId
            };
        }
    }
}