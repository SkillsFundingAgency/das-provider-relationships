using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Authentication.Extensions.Legacy;
using System.Threading;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RegistrationApiClient : ApiClientBase, IRegistrationApiClient
    {
        private readonly string _apiBaseUrl;
        private readonly string _identifierUri;
        private readonly HttpClient _client;

        public RegistrationApiClient(HttpClient client, IRegistrationApiConfiguration configuration) : base(client)
        {
            _apiBaseUrl = configuration.BaseUrl.EndsWith("/")
                ? configuration.BaseUrl
                : configuration.BaseUrl + "/";

            _identifierUri = configuration.IdentifierUri;
            _client = client;
        }

        public async Task Unsubscribe(string CorrelationId)
        {
            await AddAuthenticationHeader();

            var url = $"{_apiBaseUrl}api/unsubscribe/{CorrelationId}";
            await _client.GetAsync(url);
        }

        public async Task<string> GetInvitations(string CorrelationId, CancellationToken cancellationToken = default)
        {
            await AddAuthenticationHeader();

            var url = $"{_apiBaseUrl}api/invitations/{CorrelationId}";
            var response = await _client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task AddAuthenticationHeader()
        {
            if (ConfigurationManager.AppSettings["EnvironmentName"].ToUpper() != "LOCAL")
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_identifierUri);

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}