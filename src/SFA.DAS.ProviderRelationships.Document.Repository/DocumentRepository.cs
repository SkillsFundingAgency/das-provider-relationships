using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public abstract class DocumentRepository<TDocument> : IDocumentRepository<TDocument> where TDocument : class
    {
        private readonly IDocumentClient _documentClient;
        private readonly string _databaseName;
        private readonly string _collectionName;

        protected DocumentRepository(IDocumentClient documentClient, string databaseName, string collectionName)
        {
            _documentClient = documentClient;
            _databaseName = databaseName;
            _collectionName = collectionName;
        }

        public Task Add(TDocument document, RequestOptions requestOptions = null)
        {
            return _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), document, requestOptions);
        }

        public IQueryable<TDocument> CreateQuery(FeedOptions feedOptions = null)
        {
            return _documentClient.CreateDocumentQuery<TDocument>(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), feedOptions);
        }

        public async Task<TDocument> GetById(Guid id, RequestOptions requestOptions = null)
        {
            try
            {
                var response = await _documentClient.ReadDocumentAsync<TDocument>(UriFactory.CreateDocumentUri(_databaseName, _collectionName, id.ToString()), requestOptions).ConfigureAwait(false);
                
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

        public Task Remove(Guid id, RequestOptions requestOptions = null)
        {
            return _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, id.ToString()), requestOptions);
        }

        public Task Update(TDocument document, RequestOptions requestOptions = null)
        {
            return _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), document, requestOptions);
        }
    }
}