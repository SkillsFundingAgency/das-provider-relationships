using System;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Audit.DataAccess.MapperExtensions
{
    public static class DeletedPermissionsEventAuditMapperExtensions
    {
        public static DeletedPermissionsEventAudit MapToEntity(this DeletedPermissionsEventAuditCommand command)
        {
            return new DeletedPermissionsEventAudit {
                Ukprn = command.Ukprn,
                AccountProviderLegalEntityId = command.AccountProviderLegalEntityId,
                Deleted = command.Deleted,
                TimeLogged = DateTime.UtcNow
            };
        }
    }
}