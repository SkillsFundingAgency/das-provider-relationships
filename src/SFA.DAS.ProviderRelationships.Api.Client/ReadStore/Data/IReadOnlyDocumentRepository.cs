using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    public interface IReadOnlyDocumentRepository<TDocument> where TDocument : class
    {
        IQueryable<TDocument> CreateQuery(FeedOptions feedOptions = null);

        Task<TDocument> GetById(
            Guid id,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default (CancellationToken));
    }
}
