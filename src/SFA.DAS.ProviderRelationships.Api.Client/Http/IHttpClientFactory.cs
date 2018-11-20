using System.Net.Http;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}