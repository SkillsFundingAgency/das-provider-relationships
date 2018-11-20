using System.Net.Http;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<ReadStoreDataRegistry>();
            IncludeRegistry<ReadStoreMediatorRegistry>();
            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<HttpClientFactory>();
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
        }
    }
}