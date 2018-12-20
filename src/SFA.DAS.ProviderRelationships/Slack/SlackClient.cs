using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Slack
{
    public class SlackClient : ISlackClient
    {
        private readonly HttpClient _httpClient;
        private readonly SlackConfiguration _configuration;

        public SlackClient(HttpClient httpClient, SlackConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public Task PostCreatedEmployerAccount(string name)
        {
            var text = $"{name} has joined the digital apprenticeship service";
                
            return _httpClient.PostAsync(_configuration.DasMetricsApiUrl, new { text }, new JsonMediaTypeFormatter());
        }
    }
}