using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    //todo: unit tests for this
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        
        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken);
            return await response.Content.ReadAsAsync<T>(cancellationToken);
        }
        
        public Task<T> Get<T>(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return Get<T>(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public async Task<string> Get(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }
        
        public Task<string> Get(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return Get(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        private async Task<HttpResponseMessage> GetResponse(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            if (queryData != null)
                uri = new Uri(AddQueryString(uri.ToString(), queryData), UriKind.RelativeOrAbsolute);
            
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
    }
}