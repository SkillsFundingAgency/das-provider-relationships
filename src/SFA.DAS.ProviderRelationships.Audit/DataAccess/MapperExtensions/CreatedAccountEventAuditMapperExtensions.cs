using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions
{
    public static class CreatedAccountEventAuditMapperExtensions
    {
        public static CreatedAccountEventAudit MapToEntity(this CreatedAccountEventAuditCommand command)
        {
            return new CreatedAccountEventAudit
            {
                Name = command.Name,
                AccountId = command.AccountId,
                PublicHashedId = command.PublicHashedId,
                UserName = command.UserName,
                UserRef = command.UserRef,
                TimeLogged = DateTime.UtcNow
            };
        }
    }

    public static class UpdatedPermissionsEventAuditMapperExtensions
    {
        public static UpdatedPermissionsEventAudit MapToEntity(this UpdatedPermissionsEventAuditCommand command)
        {
            return new UpdatedPermissionsEventAudit {
                AccountId = command.AccountId,
                UserRef = command.UserRef,
                TimeLogged = DateTime.UtcNow,
                AccountLegalEntityId = command.AccountLegalEntityId,
                AccountProviderId = command.AccountProviderId,
                AccountProviderLegalEntityId = command.AccountProviderLegalEntityId,
                GrantedOperations = ConstructOperationsAudit(command.GrantedOperations),
                Ukprn = command.Ukprn,
                Updated = command.Updated
            };
        }

        private static string ConstructOperationsAudit(IEnumerable<Operation> operations)
        {
            var first = true;
            var result = string.Empty;
            foreach (var operation in operations)
            {
                if (!first) { result += ","; }
                result += operation.ToString();
                first = false;
            }
            return result;
        }
    }
}
