using MediatR;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Audit.DependencyResolution
{
    public class AuditRegistry : Registry
    {
        public AuditRegistry()
        {
            For<IRequestHandler<CreatedAccountEventAuditCommand, Unit>>().Use<CreatedAccountEventAuditCommandHandler>();
        }
    }
}
