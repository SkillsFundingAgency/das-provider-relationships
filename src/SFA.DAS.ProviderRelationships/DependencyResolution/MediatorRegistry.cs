using MediatR;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(c => c.GetInstance);
        }
    }
}