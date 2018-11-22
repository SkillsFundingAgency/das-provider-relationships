using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
//  public abstract class ApiClientBase
//  {
//    private readonly QueryStringHelper _queryStringHelper;
//    private readonly HttpClient _client;
//
//    protected ApiClientBase(HttpClient client)
//    {
//      this._client = client;
//      this._queryStringHelper = new QueryStringHelper();
//    }
//
//    protected virtual async Task<string> GetAsync(string url)
//    {
//      HttpResponseMessage response = await this._client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
//      string str = await response.Content.ReadAsStringAsync();
//      response.EnsureSuccessStatusCode();
//      return str;
//    }
//
//    protected virtual async Task<string> GetAsync(string url, object data)
//    {
//      HttpResponseMessage response = await this._client.SendAsync(new HttpRequestMessage(HttpMethod.Get, string.Format("{0}{1}", (object) url, (object) this._queryStringHelper.GetQueryString(data))));
//      string str = await response.Content.ReadAsStringAsync();
//      response.EnsureSuccessStatusCode();
//      return str;
//    }
//  }
    
    // todo: don't use ApiClientBase (or improve it), it...
    // doesn't insert the response body into the exception when it gets returned on error, nor does it log the response body!
    // only supports fetching the response body as a string, and doesn't allow getting it as a type (ReadAsAsync<>)
    // fetches the response body on error for no reason
    // is not test friendly
    // doesn't actually give you much
    public class ProviderRelationshipsApiClient : ApiClientBase, IProviderRelationshipsApiClient
    {
        private readonly IReadStoreMediator _mediator;

        public ProviderRelationshipsApiClient(HttpClient client, IReadStoreMediator mediator)
            : base(client)
        {
            _mediator = mediator;
        }

        public async Task<RelationshipsResponse> GetRelationshipsWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default)
        {
            var bodyAsString = await GetAsync("relationships", request);
            return JsonConvert.DeserializeObject<RelationshipsResponse>(bodyAsString);
        }

        public Task<bool> HasPermission(PermissionRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasPermissionQuery(request.Ukprn, request.EmployerAccountLegalEntityId, request.Operation), cancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission(RelationshipsRequest request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(new HasRelationshipWithPermissionQuery(request.Ukprn, request.Operation), cancellationToken);
        }

        public Task HealthCheck()
        {
            return GetAsync("healthcheck");
        }
    }
}