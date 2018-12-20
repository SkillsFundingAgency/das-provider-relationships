using System.Net.Http;
using SFA.DAS.ProviderRelationships.Http;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class HttpRegistry : Registry
    {
        public HttpRegistry()
        {
            For<HttpClient>().Use(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
        }
    }
}