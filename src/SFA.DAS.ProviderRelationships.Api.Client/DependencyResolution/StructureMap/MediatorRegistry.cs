using MediatR;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap
{
    internal class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            For<IMediator>().Use<Mediator>();
            For<IRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
            For<IRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
        }
    }
}