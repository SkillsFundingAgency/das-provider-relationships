//todo: delete?
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ReadStoreMediatorRegistry : Registry
    {
        public ReadStoreMediatorRegistry()
        {
            For<ReadStoreServiceFactory>().Use<ReadStoreServiceFactory>(c => c.GetInstance);
            For<IReadStoreMediator>().Use<ReadStoreMediator>();
            For<IReadStoreRequestHandler<DeletePermissionsCommand, Unit>>().Use<DeletePermissionsCommandHandler>();
            For<IReadStoreRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IReadStoreRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
            For<IReadStoreRequestHandler<UpdatePermissionsCommand, Unit>>().Use<UpdatePermissionsCommandHandler>();
        }
    }
}