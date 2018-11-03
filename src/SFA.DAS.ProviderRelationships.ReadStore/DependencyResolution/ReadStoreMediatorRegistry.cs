using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution
{
    internal class ReadStoreMediatorRegistry : Registry
    {
        public ReadStoreMediatorRegistry()
        {
            For<IApiMediator>().Use<ApiMediator>();
            For<IApiRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult>>().Use<GetRelationshipWithPermissionQueryHandler>();
            For<IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<ApiServiceFactory>().Use<ApiServiceFactory>(c => c.GetInstance);
        }
    }
}