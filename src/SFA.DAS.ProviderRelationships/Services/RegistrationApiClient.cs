using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.Authentication.Extensions.Legacy;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RegistrationApiClient : ApiClientBase, IRegistrationApiClient
    {
        private readonly string _apiBaseUrl;
        private readonly string _identifierUri;
        private readonly HttpClient _client;
        private readonly ProviderRelationshipsConfiguration _prConfig;

        public RegistrationApiClient(HttpClient client, IRegistrationApiConfiguration configuration, ProviderRelationshipsConfiguration prConfig) : base(client)
        {
            _apiBaseUrl = configuration.ApiBaseUrl.EndsWith("/")
                ? configuration.ApiBaseUrl
                : configuration.ApiBaseUrl + "/";

            _identifierUri = configuration.IdentifierUri;
            _client = client;
            _prConfig = prConfig;
        }

        public async Task Unsubscribe(string CorrelationId)
        {
            var url = $"{_apiBaseUrl}api/unsubscribe/{CorrelationId}";
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            await AddAuthenticationHeader(httpRequestMessage);
            await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
        }

        public async Task<string> GetInvitations(string CorrelationId, CancellationToken cancellationToken = default)
        {
            var url = $"{_apiBaseUrl}api/invitations/{CorrelationId}";
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            await AddAuthenticationHeader(httpRequestMessage);
            var response = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        }

        protected async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (_prConfig.EnvironmentName.ToUpper() != "LOCAL")
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_identifierUri);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}