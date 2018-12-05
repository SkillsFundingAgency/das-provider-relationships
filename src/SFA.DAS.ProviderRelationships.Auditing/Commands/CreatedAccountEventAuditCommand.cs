using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.ProviderRelationships.Auditing.DataAccess.Entities;
using SFA.DAS.ProviderRelationships.Auditing.DataAccess.MapperExtensions;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Auditing.Commands
{
    public class CreatedAccountEventAuditCommand : IRequest
    {
        public long AccountId { get; set; }
        public string PublicHashedId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }

    public class CreatedAccountEventAuditCommandHandler : RequestHandler<CreatedAccountEventAuditCommand>
    {
        public CreatedAccountEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {

        }

        protected override void Handle(CreatedAccountEventAuditCommand request)
        {
            var entity = request.MapToEntity();
            
        }
    }
}
