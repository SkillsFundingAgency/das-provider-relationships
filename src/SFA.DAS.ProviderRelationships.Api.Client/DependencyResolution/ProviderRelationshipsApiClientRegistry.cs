using System.Net.Http;
using Microsoft.Azure.Documents;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator;
using StructureMap;
using DocumentClientFactory = SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data.DocumentClientFactory;
using IDocumentClientFactory = SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data.IDocumentClientFactory;
using ProviderRelationshipsReadStoreConfiguration = SFA.DAS.ProviderRelationships.Api.Client.Configuration.ProviderRelationshipsReadStoreConfiguration;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            //IncludeRegistry<AutoConfigurationRegistry>();
            
            
//            IncludeRegistry<ReadStoreConfigurationRegistry>();
            IncludeRegistry<AutoConfigurationRegistry>();
            //todo: main config will contain one of these
            For<ProviderRelationshipsReadStoreConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsReadStoreConfiguration>());


//            IncludeRegistry<ReadStoreDataRegistry>();
            For<IDocumentClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IAccountProviderLegalEntitiesRepository>().Use<ReadOnlyAccountProviderLegalEntitiesRepository>().Ctor<IDocumentClient>().IsNamedInstance(GetType().FullName);


//todo: do mediator registrations by convention
//            IncludeRegistry<ReadStoreMediatorRegistry>();
            For<ReadStoreServiceFactory>().Use<ReadStoreServiceFactory>(c => c.GetInstance);
            For<IReadStoreMediator>().Use<ReadStoreMediator>();
            //todo: will be required somewhere else (or convention)
            //For<IReadStoreRequestHandler<DeletePermissionsCommand, Unit>>().Use<DeletePermissionsCommandHandler>();
            For<IReadStoreRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IReadStoreRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
            //todo: will be required somewhere else
            //For<IReadStoreRequestHandler<UpdatePermissionsCommand, Unit>>().Use<UpdatePermissionsCommandHandler>();



            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
            For<IRestHttpClient>().Use<RestHttpClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();
            For<ProviderRelationshipsApiClientConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsApiClientConfiguration>());
        }
    }
}