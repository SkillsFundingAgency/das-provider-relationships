using System.Net.Http;
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

        public HttpClient CreateHttpClient()
        {
            //var httpClient = new HttpClientBuilder()
            //    .WithDefaultHeaders()
            //    .WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(_recruitApiClientConfig))
            //    .Build();

            //httpClient.BaseAddress = new Uri(_recruitApiClientConfig.ApiBaseUrl);

            //// set timeout, so we don't end up delaying the rendering of the homepage for a misbehaving api
            ////httpClient.Timeout = TimeSpan.Parse(_recruitApiClientConfig.TimeoutTimeSpan);

            //return httpClient;
            return new AzureActiveDirectoryHttpClientFactory(_recruitApiClientConfig).CreateHttpClient();
        }
    }
}
