using MediatR;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    internal class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);

            For<IRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
            For<IRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
        }
    }
}