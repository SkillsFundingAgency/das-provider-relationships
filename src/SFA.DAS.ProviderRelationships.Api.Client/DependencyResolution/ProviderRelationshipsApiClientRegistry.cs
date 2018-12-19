using System.Net.Http;
using Microsoft.Azure.Documents;
using MediatR;
using StructureMap;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();

            For<IDocumentClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IAccountProviderLegalEntitiesRepository>().Use<ReadOnlyAccountProviderLegalEntitiesRepository>().Ctor<IDocumentClient>().IsNamedInstance(GetType().FullName);

//todo: do mediator registrations by convention
//            IncludeRegistry<ReadStoreMediatorRegistry>();
            //For<ReadStoreServiceFactory>().Use<ReadStoreServiceFactory>(c => c.GetInstance);
            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
//            For<IReadStoreMediator>().Use<ReadStoreMediator>();
//            For<IReadStoreRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
//            For<IReadStoreRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();

            Scan(s =>
            {
                s.AssemblyContainingType<HasPermissionQueryHandler>();
                s.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                //s.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            });

            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
            For<IRestHttpClient>().Use<RestHttpClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();

            //todo: how to wrangle these so don't read the config from table storage twice? level of indirection/cache
            For<ReadStoreConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsApiClientConfiguration>().ReadStore);
            For<AzureAdClientConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsApiClientConfiguration>().AzureAdClient);
        }
    }
}