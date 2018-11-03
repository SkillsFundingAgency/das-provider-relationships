using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : class
    {
        private Uri DocumentCollectionUri(string collectionName) => UriFactory.CreateDocumentCollectionUri(_databaseName, collectionName);
        private Uri DocumentUri(string collectionName, string id) => UriFactory.CreateDocumentUri(_databaseName, collectionName, id);

        private readonly IDocumentClient _documentClient;
        private readonly string _databaseName;
        private readonly string _collectionName;

        public DocumentRepository(IDocumentClient documentClient, string databaseName, string collectionName)
        {
            _documentClient = documentClient;
            _databaseName = databaseName;
            _collectionName = collectionName;
        }

        public async Task<TDocument> GetById(Guid id)
        {
            try
            {
                var documentUri = DocumentUri(_collectionName, id.ToString());
                var response = await _documentClient.ReadDocumentAsync<TDocument>(documentUri).ConfigureAwait(false);
                
                return response.Document;
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                throw;
            }
        }

        public IQueryable<TDocument> CreateQuery(FeedOptions feedOptions = null)
        {
            var documentCollectionUri = DocumentCollectionUri(_collectionName);
            var query = _documentClient.CreateDocumentQuery<TDocument>(documentCollectionUri, feedOptions);

            return query;
        }

        public Task Update(TDocument entity)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}