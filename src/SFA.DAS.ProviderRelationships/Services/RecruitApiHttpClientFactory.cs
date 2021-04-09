using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RecruitApiHttpClientFactory : IRecruitApiHttpClientFactory
    {
        private readonly RecruitApiConfiguration _recruitApiClientConfig;

        public RecruitApiHttpClientFactory(RecruitApiConfiguration recruitApiClientConfig)
        {
            _recruitApiClientConfig = recruitApiClientConfig;
        }

        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new AzureActiveDirectoryHttpClientFactory(_recruitApiClientConfig).CreateHttpClient());
        }
    }
}