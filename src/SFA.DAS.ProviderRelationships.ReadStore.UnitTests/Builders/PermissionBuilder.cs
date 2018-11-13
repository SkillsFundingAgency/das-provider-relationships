using System.Collections.Generic;
using Moq;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders
{
    internal class PermissionBuilder
    {
        private readonly Mock<Permission> _permission = new Mock<Permission> { CallBase = true };

        public PermissionBuilder WithEmployerAccountId(long employerAccountId)
        {
            _permission.SetupProperty(p => p.EmployerAccountId, employerAccountId);
            
            return this;
        }

        public PermissionBuilder WithEmployerAccountPublicHashedId(string employerAccountPublicHashedId)
        {
            _permission.SetupProperty(p => p.EmployerAccountPublicHashedId, employerAccountPublicHashedId);
            
            return this;
        }

        public PermissionBuilder WithEmployerAccountName(string employerAccountName)
        {
            _permission.SetupProperty(p => p.EmployerAccountName, employerAccountName);
            
            return this;
        }

        public PermissionBuilder WithEmployerAccountLegalEntityId(long employerAccountLegalEntityId)
        {
            _permission.SetupProperty(p => p.EmployerAccountLegalEntityId, employerAccountLegalEntityId);
            
            return this;
        }

        public PermissionBuilder WithEmployerAccountLegalEntityPublicHashedId(string employerAccountLegalEntityPublicHashedId)
        {
            _permission.SetupProperty(p => p.EmployerAccountLegalEntityPublicHashedId, employerAccountLegalEntityPublicHashedId);
            
            return this;
        }

        public PermissionBuilder WithEmployerAccountLegalEntityName(string employerAccountLegalEntityName)
        {
            _permission.SetupProperty(p => p.EmployerAccountLegalEntityName, employerAccountLegalEntityName);
            
            return this;
        }

        public PermissionBuilder WithEmployerAccountProviderId(int employerAccountProviderId)
        {
            _permission.SetupProperty(p => p.EmployerAccountProviderId, employerAccountProviderId);
            
            return this;
        }

        public PermissionBuilder WithUkprn(long ukprn)
        {
            _permission.SetupProperty(p => p.Ukprn, ukprn);
            
            return this;
        }
        
        public PermissionBuilder WithOperation(Operation operation)
        {
            _permission.SetupProperty(p => p.Operations, new List<Operation> { operation });
            
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