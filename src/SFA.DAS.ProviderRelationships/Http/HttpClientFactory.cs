using System.Net.Http;
using SFA.DAS.Http;

namespace SFA.DAS.ProviderRelationships.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateHttpClient()
        {
            return new HttpClientBuilder().Build();
        }
    }
}