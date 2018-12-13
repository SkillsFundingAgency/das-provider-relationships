using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions
{
    public static class UpdatedPermissionsEventAuditMapperExtensions
    {
        public static UpdatedPermissionsEventAudit MapToEntity(this UpdatedPermissionsEventAuditCommand command)
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