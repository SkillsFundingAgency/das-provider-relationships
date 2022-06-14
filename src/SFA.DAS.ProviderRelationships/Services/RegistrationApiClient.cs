using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Authentication.Extensions.Legacy;
using System.Threading;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RegistrationApiClient : ApiClientBase, IRegistrationApiClient
    {
        private readonly string _apiBaseUrl;
        private readonly string _identifierUri;
        private readonly HttpClient _client;
        private readonly HttpRequestMessage _httpRequestMessage;

        public RegistrationApiClient(HttpClient client, IRegistrationApiConfiguration configuration, HttpRequestMessage httpRequestMessage) : base(client)
        {
            _apiBaseUrl = configuration.BaseUrl.EndsWith("/")
                ? configuration.BaseUrl
                : configuration.BaseUrl + "/";

            _identifierUri = configuration.IdentifierUri;
            _client = client;
            _httpRequestMessage = httpRequestMessage;
        }

        public async Task Unsubscribe(string CorrelationId)
        {
            await AddAuthenticationHeader(_httpRequestMessage);

            var url = $"{_apiBaseUrl}api/unsubscribe/{CorrelationId}";
            await _client.GetAsync(url);
        }

        public async Task<InvitationDto> GetInvitations(string CorrelationId, CancellationToken cancellationToken = default)
        {
            await AddAuthenticationHeader(_httpRequestMessage);

            var url = $"{_apiBaseUrl}api/invitations/{CorrelationId}";
            var response = await _client.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<InvitationDto>(message);
        }

        protected async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            if (ConfigurationManager.AppSettings["EnvironmentName"].ToUpper() != "LOCAL")
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(_identifierUri);
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
    }
}