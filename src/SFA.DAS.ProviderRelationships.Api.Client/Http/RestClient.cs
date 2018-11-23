using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    // todo: don't use ApiClientBase (or improve it), it...
    // doesn't insert the response body into the exception when it gets returned on error (nor does it log the response body, so blinds you to the root cause (if it's supplied))!
    // only supports fetching the response body as a string, and doesn't allow getting it as a type (ReadAsAsync<>)
    // fetches the response body on error for no reason
    // uses inheritance rather than composition
    // is not test friendly
    // doesn't actually give you much
    
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

    public class RestClientException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ReasonPhrase { get; private set; }
        public Uri RequestUri { get; private set; }
        public string ErrorResponse { get; private set; }
        
//        public RestClientException(HttpStatusCode statusCode, string reasonPhrase, Uri requestUri, string errorResponse)
//        {
//            StatusCode = statusCode;
//            ReasonPhrase = reasonPhrase;
//            RequestUri = requestUri;
//            ErrorResponse = errorResponse;
//        }

        public RestClientException(HttpResponseMessage httpResponseMessage, string errorResponse)
        : base(GenerateMessage(httpResponseMessage, errorResponse))
        {
            StatusCode = httpResponseMessage.StatusCode;
            ReasonPhrase = httpResponseMessage.ReasonPhrase;
            RequestUri = httpResponseMessage.RequestMessage.RequestUri;
            ErrorResponse = errorResponse;
        }
        
        // assumes response content hasn't already been read
        public static async Task<RestClientException> Create(HttpResponseMessage httpResponseMessage)
        {
            return new RestClientException(httpResponseMessage, await httpResponseMessage.Content.ReadAsStringAsync());
        }

        private static string GenerateMessage(HttpResponseMessage httpResponseMessage, string errorResponse)
        {
            return
$@"Request '{httpResponseMessage.RequestMessage.RequestUri}' returned {httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase}
Response:
{errorResponse}";
        }
    }

    public interface IRestClient
    {
        Task<string> Get(string uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default);
        Task<T> Get<T>(string uri, object queryData = null, CancellationToken cancellationToken = default);
    }
    
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        
        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
//            _httpClient.DefaultRequestHeaders.Accept.Clear();
//            _httpClient.DefaultRequestHeaders.Accept.Add(
//                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task<T> Get<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return Get<T>(uri.ToString(), queryData, cancellationToken);
        }
        
        public async Task<T> Get<T>(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken);

            //1 form array?
            return await response.Content.ReadAsAsync<T>(new [] { new JsonMediaTypeFormatter() }, cancellationToken);
        }

        public async Task<string> Get(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpResponseMessage> GetResponse(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            if (queryData != null)
                uri = AddQueryString(uri, queryData);
            
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw await RestClientException.Create(response);

            return response;
        }
        
        private string AddQueryString(string uri, object queryData)
        {
            //todo: querystringhelper supports icollection props, we don't need that yet though
            var queryDataDictionary = queryData.GetType().GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(queryData)?.ToString() ?? string.Empty);
            return QueryHelpers.AddQueryString(uri, queryDataDictionary);
        }
        //Post can use PostAsJsonAsync
    }
}