using System;
using SFA.DAS.ProviderRelationships.Auditing.Commands;
using SFA.DAS.ProviderRelationships.Auditing.DataAccess.Entities;

namespace SFA.DAS.ProviderRelationships.Auditing.DataAccess.MapperExtensions
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
}
