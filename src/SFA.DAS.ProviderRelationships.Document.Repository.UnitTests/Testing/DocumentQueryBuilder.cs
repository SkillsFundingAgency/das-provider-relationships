using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Moq;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Testing
{
    public class DocumentQueryBuilder<TDocument> where TDocument : class
    {
        private readonly Mock<IDocumentQuery<TDocument>> _documentQuery = new Mock<IDocumentQuery<TDocument>>();

        public DocumentQueryBuilder()
        {
            _documentQuery.SetupSequence(q => q.HasMoreResults).Returns(true).Returns(false);
        }

        public DocumentQueryBuilder<TDocument> WithDocuments(IEnumerable<TDocument> documents)
        {
            var documentQuery = _documentQuery.As<IOrderedQueryable<TDocument>>();
            var query = documents.AsQueryable();
            var queryProvider = new Mock<IQueryProvider>();
            
            documentQuery.Setup(q => q.ElementType).Returns(() => query.ElementType);
            documentQuery.Setup(q => q.Expression).Returns(() => query.Expression);
            documentQuery.Setup(q => q.GetEnumerator()).Returns(() => query.GetEnumerator());
            documentQuery.Setup(q => q.Provider).Returns(() => queryProvider.Object);
            
            queryProvider.Setup(p => p.CreateQuery<TDocument>(It.IsAny<Expression>()))
                .Callback<Expression>(e => query = query.Provider.CreateQuery<TDocument>(e))
                .Returns<Expression>(e => documentQuery.Object);
            
            _documentQuery.Setup(q => q.ExecuteNextAsync<TDocument>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new FeedResponse<TDocument>(query.ToList()));
            
            return this;
        }

        public IOrderedQueryable<TDocument> Build()
        {
            return (IOrderedQueryable<TDocument>)_documentQuery.Object;
        }
    }
}