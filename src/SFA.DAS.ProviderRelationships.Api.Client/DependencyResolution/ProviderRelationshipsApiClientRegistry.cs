using System.Net.Http;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            IncludeRegistry<ReadStoreConfigurationRegistry>();
            IncludeRegistry<ReadStoreDataRegistry>();
            IncludeRegistry<ReadStoreMediatorRegistry>();
            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
            For<IRestClient>().Use<RestClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();
            For<ProviderRelationshipsApiClientConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsApiClientConfiguration>());
        }
    }
}