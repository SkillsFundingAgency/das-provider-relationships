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

        public Permission WithAccountLegalEntityId(long accountLegalEntityId)
        {
            _permission.SetupProperty(p => p.AccountLegalEntityId, accountLegalEntityId);
            
            return this;
        }

        public PermissionBuilder WithAccountProviderId(long accountProviderId)
        {
            _permission.SetupProperty(p => p.AccountProviderId, accountProviderId);
            
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