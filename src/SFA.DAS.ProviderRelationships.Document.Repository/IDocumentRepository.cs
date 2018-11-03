using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentRepository<TDocument> where TDocument : class
    {
        Task<TDocument> GetById(Guid id);
        IQueryable<TDocument> CreateQuery(FeedOptions options = null);
        Task Update(TDocument document);
        Task Remove(Guid id);
    }
}