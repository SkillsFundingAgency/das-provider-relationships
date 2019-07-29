using System.Net.Http;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRecruitApiHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}