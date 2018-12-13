using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

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
            var entity = MapToEntity(request);
            _db.Value.UpdatedPermissionsEventAudits.Add(entity);
        }

        private static UpdatedPermissionsEventAudit MapToEntity(UpdatedPermissionsEventAuditCommand command)
        {
            return new UpdatedPermissionsEventAudit(DateTime.UtcNow, command.Updated,
                ConstructOperationsAudit(command.GrantedOperations), command.UserRef, command.Ukprn,
                command.AccountProviderLegalEntityId, command.AccountProviderId, command.AccountLegalEntityId,
                command.AccountId);
        }

        private static string ConstructOperationsAudit(IEnumerable<Operation> operations)
        {
            var first = true;
            var result = string.Empty;
            foreach (var operation in operations)
            {
                if (!first) { result += ","; }
                result += ((short)operation).ToString();
                first = false;
            }
            return result;
        }
    }
}