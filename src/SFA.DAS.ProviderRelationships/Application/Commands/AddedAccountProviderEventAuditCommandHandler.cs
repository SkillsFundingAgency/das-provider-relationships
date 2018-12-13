using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddedAccountProviderEventAuditCommandHandler : RequestHandler<AddedAccountProviderEventAuditCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddedAccountProviderEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(AddedAccountProviderEventAuditCommand request)
        {
            var entity = MapToEntity(request);
            _db.Value.AddedAccountProviderEventAudits.Add(entity);
        }

        private static AddedAccountProviderEventAudit MapToEntity(AddedAccountProviderEventAuditCommand command)
        {
            return new AddedAccountProviderEventAudit {
                AccountProviderId = command.AccountProviderId,
                AccountId = command.AccountId,
                ProviderUkprn = command.ProviderUkprn,
                UserRef = command.UserRef,
                Added = command.Added,
                TimeLogged = DateTime.UtcNow
            };
        }
    }
}