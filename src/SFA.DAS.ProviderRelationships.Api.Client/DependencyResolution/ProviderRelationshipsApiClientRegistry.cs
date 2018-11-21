using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            For<ApiServiceFactory>().Use<ApiServiceFactory>(c => c.GetInstance);
            For<IApiMediator>().Use<ApiMediator>();
            For<IApiRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult>>().Use<GetRelationshipWithPermissionQueryHandler>();
            For<IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IApiRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
            For<IDocumentClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IRelationshipsRepository>().Use<RelationshipsRepository>().Ctor<IDocumentClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();
            For<ITableStorageConfigurationService>().Use<TableStorageConfigurationService>();
            For<IEnvironmentService>().Use<EnvironmentService>();
        }
    }
}