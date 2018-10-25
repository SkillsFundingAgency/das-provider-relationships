using MediatR;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(c => c.GetInstance);

            Scan(s =>
            {
                s.AssemblyContainingType<GetHasRelationshipWithPermissionHandler>();
                s.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });
        }
    }

}
