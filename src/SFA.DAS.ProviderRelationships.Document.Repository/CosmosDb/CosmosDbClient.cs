using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosDbClient<TResult> : IDocumentDbClient<TResult> where TResult : class
    {
        private readonly IDocumentClient _dbClient;
        private readonly IDocumentConfiguration _configuration;

        public CosmosDbClient(
            IDocumentClientFactory dbClientFactory,
            IDocumentConfiguration configuration)
        {
            _dbClient = dbClientFactory.Create(configuration);
            _configuration = configuration;
        }

        public async Task<TResult> GetById(string collectionName, Guid id)
        {
            try
            {
                var resourceResponse = await _dbClient.ReadDocumentAsync(DocumentUri(collectionName, id.ToString()));

                var result = JsonConvert.DeserializeObject<TResult>(resourceResponse.Resource.ToString());
                return result;
            }
            catch (DocumentClientException exc)
            {
                if (exc.StatusCode == HttpStatusCode.NotFound)
                    return null;
                throw;
            }
        }

        public Task<IEnumerable<TResult>> Search(string collectionName, Expression<Func<TResult, bool>> sqlApiQuery)
        {
            var defaultFeedOptions = new FeedOptions {MaxItemCount = -1, EnableCrossPartitionQuery = false};
            return Search(collectionName, sqlApiQuery, defaultFeedOptions);
        }

        public async Task<IEnumerable<TResult>> Search(string collectionName, Expression<Func<TResult, bool>> sqlApiQuery, FeedOptions feedOptions)
        {
            try
            {
                var query =
                    _dbClient
                        .CreateDocumentQuery<TResult>(DocumentCollectionUri(collectionName), feedOptions)
                        .Where(sqlApiQuery);

                return await query.AsDocumentQuery().ExecuteNextAsync<TResult>();
            }
            catch (DocumentClientException documentClientException)
            {
                if(documentClientException.StatusCode == HttpStatusCode.NotFound)
                    throw new DocumentException("Collection Not Found", HttpStatusCode.NotFound, documentClientException) ;

                if (documentClientException.StatusCode == HttpStatusCode.BadRequest && documentClientException.Message.StartsWith("Cross partition query is required but disabled"))
                    throw new DocumentException("This Collection is partitioned, the query must include the partition key or you must set the option to query across all partitions.", HttpStatusCode.BadRequest, documentClientException);
                
                throw;
            }
            catch (JsonSerializationException jsonException)
            {
                throw new DocumentException(jsonException.Message, HttpStatusCode.InternalServerError, jsonException);
            }
        }

        protected Uri DocumentCollectionUri(string collectionName) =>
            UriFactory.CreateDocumentCollectionUri(_configuration.DatabaseName, collectionName);

        protected Uri DocumentUri(string collectionName, string id) =>
            UriFactory.CreateDocumentUri(_configuration.DatabaseName, collectionName, id);
    }
}
