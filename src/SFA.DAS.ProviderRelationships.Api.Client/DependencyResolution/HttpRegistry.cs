using System.Net.Http;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.Http;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    internal class HttpRegistry : Registry
    {
        public HttpRegistry()
        {
            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
            For<IRestHttpClient>().Use<RestHttpClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();
        }
    }
}