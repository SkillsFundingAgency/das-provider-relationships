using Moq;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public class PermissionBuilder
    {
        private readonly Mock<Permission> _permission = new Mock<Permission> { CallBase = true };

        public PermissionBuilder WithId(long id)
        {
            _permission.SetupProperty(p => p.Id, id);
            
            return this;
        }

        public PermissionBuilder WithAccountProviderLegalEntityId(long accountProviderLegalEntityId)
        {
            _permission.SetupProperty(p => p.AccountProviderLegalEntityId, accountProviderLegalEntityId);
            
            return this;
        }

        public PermissionBuilder WithOperation(Operation operation)
        {
            _permission.SetupProperty(p => p.Operation, operation);
            
            return this;
        }

        public Permission Build()
        {
            return _permission.Object;
        }

        public static implicit operator Permission(PermissionBuilder builder)
        {
            return builder.Build();
        }
    }
}