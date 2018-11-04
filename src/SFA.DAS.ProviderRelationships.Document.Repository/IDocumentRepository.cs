using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentRepository<TDocument> where TDocument : class
    {
        Task Add(TDocument document, RequestOptions requestOptions = null);
        IQueryable<TDocument> CreateQuery(FeedOptions feedOptions = null);
        Task<TDocument> GetById(Guid id, RequestOptions requestOptions = null);
        Task Remove(Guid id, RequestOptions requestOptions = null);
        Task Update(TDocument document, RequestOptions requestOptions = null);
    }
}