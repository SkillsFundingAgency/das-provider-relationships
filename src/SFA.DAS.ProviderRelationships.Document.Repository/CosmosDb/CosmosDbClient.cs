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
    public class CosmosDbClient<TResult> where TResult : class
    {
        private readonly IDocumentClient _dbClient;
        private readonly CosmosDbConfiguration _configuration;
        private readonly string _documentCollectionName;

        public CosmosDbClient(
            IDocumentClientFactory dbClientFactory,
            CosmosDbConfiguration configuration,
            string documentCollectionName)
        {
            _dbClient = dbClientFactory.Create(configuration);
            _configuration = configuration;
            _documentCollectionName = documentCollectionName;
        }

        public async Task<TResult> GetById(Guid id)
        {
            try
            {
                var resourceResponse = await _dbClient.ReadDocumentAsync(DocumentUri(id.ToString()));

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

        public Task<IEnumerable<TResult>> Search(Expression<Func<TResult, bool>> sqlApiQuery)
        {
            var defaultFeedOptions = new FeedOptions {MaxItemCount = -1, EnableCrossPartitionQuery = false};
            return Search(sqlApiQuery, defaultFeedOptions);
        }

        public async Task<IEnumerable<TResult>> Search(Expression<Func<TResult, bool>> sqlApiQuery, FeedOptions feedOptions)
        {
            try
            {
                var query =
                    _dbClient
                        .CreateDocumentQuery<TResult>(DocumentCollectionUri(), feedOptions)
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

            catch (Exception e)
            {
                throw new Exception("Fuck", e);
            }
        }

        protected Uri DocumentCollectionUri() =>
            UriFactory.CreateDocumentCollectionUri(_configuration.DatabaseName, _documentCollectionName);

        protected Uri DocumentUri(string id) =>
            UriFactory.CreateDocumentUri(_configuration.DatabaseName, _documentCollectionName, id);
    }
}
