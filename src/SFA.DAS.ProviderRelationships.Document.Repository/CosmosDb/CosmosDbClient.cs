using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosDbClient<TEntity> : IDocumentDbClient<TEntity> where TEntity : class
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

        public async Task<TEntity> GetById(string collectionName, Guid id)
        {
            try
            {
                var resourceResponse = await _dbClient.ReadDocumentAsync(DocumentUri(collectionName, id.ToString()));

                var result = JsonConvert.DeserializeObject<TEntity>(resourceResponse.Resource.ToString());
                return result;
            }
            catch (DocumentClientException exc)
            {
                if (exc.StatusCode == HttpStatusCode.NotFound)
                    return null;
                throw;
            }
        }

        public IQueryable<TEntity> CreateQuery(string collectionName)
        {
            var defaultFeedOptions = new FeedOptions {MaxItemCount = -1, EnableCrossPartitionQuery = false};
            return CreateQuery(collectionName, defaultFeedOptions);
        }

        public IQueryable<TEntity> CreateQuery(string collectionName, FeedOptions feedOptions)
        {
            return _dbClient.CreateDocumentQuery<TEntity>(DocumentCollectionUri(collectionName), feedOptions);
        }

        public IDocumentQuery<TEntity> ConvertToDocumentQuery(IQueryable<TEntity> query) => query.AsDocumentQuery();

        protected Uri DocumentCollectionUri(string collectionName) =>
            UriFactory.CreateDocumentCollectionUri(_configuration.DatabaseName, collectionName);

        protected Uri DocumentUri(string collectionName, string id) =>
            UriFactory.CreateDocumentUri(_configuration.DatabaseName, collectionName, id);
    }
}
