using MediatR;
using SFA.DAS.ProviderRelationships.Application.Commands;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class AuditRegistry : Registry
    {
        public AuditRegistry()
        {
            For<IRequestHandler<CreatedAccountEventAuditCommand, Unit>>().Use<CreatedAccountEventAuditCommandHandler>();
            For<IRequestHandler<UpdatedPermissionsEventAuditCommand, Unit>>().Use<UpdatedPermissionsEventAuditCommandHandler>();
            For<IRequestHandler<DeletedPermissionsEventAuditCommand, Unit>>().Use<DeletedPermissionsEventAuditCommandHandler>();
        }
    }
}
