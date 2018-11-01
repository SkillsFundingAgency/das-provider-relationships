using System.Collections.Generic;
using Moq;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests
{
    public class PermissionBuilder
    {
        private readonly Mock<Permission> _permission = new Mock<Permission> { CallBase = true };

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
    }
}