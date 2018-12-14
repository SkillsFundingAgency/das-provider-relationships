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
            _db.Value.UpdatedPermissionsEventAudits.Add(new UpdatedPermissionsEventAudit(DateTime.UtcNow,
                request.Updated, ConstructOperationsAudit(request.GrantedOperations), request.UserRef, request.Ukprn,
                request.AccountProviderLegalEntityId, request.AccountProviderId, request.AccountLegalEntityId,
                request.AccountId));
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