using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions;
using SFA.DAS.ProviderRelationships.Data;

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
            var entity = request.MapToEntity();
            _db.Value.CreatedAccountEventAudits.Add(entity);
            //try
            //{
            //    _db.Value.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
            
        }
    }
}