using System.Net.Http;

namespace SFA.DAS.ProviderRelationships.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}