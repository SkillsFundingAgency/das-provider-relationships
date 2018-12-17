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
            var audit = new AddedAccountProviderEventAudit(request.AccountProviderId, request.AccountId, request.ProviderUkprn, request.UserRef, request.Added);
            
            _db.Value.AddedAccountProviderEventAudits.Add(audit);
        }
    }
}