using MediatR;
using SFA.DAS.ProviderRelationships.Application.Commands;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(c => c.GetInstance);
            
            Scan(s =>
            {
                s.AssemblyContainingType<RunHealthCheckCommandHandler>();
                //s.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                s.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });
        }
    }
}