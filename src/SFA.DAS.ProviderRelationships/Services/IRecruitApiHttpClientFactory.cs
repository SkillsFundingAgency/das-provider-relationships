using System.Net.Http;
using SFA.DAS.Http;

namespace SFA.DAS.ProviderRelationships.Services
{
    public interface IRecruitApiHttpClientFactory
    {
        IRestHttpClient CreateRestHttpClient();
    }
}