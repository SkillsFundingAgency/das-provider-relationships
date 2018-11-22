using System.Collections.Generic;
using Moq;
using SFA.DAS.CosmosDb;
using SFA.DAS.CosmosDb.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders
{
    public static class MockExtensions
    {
        public static void SetupCreateQueryToReturn<TRepository, TDocument>(this Mock<TRepository> documentRepository, IEnumerable<TDocument> documents) where TRepository : class, IDocumentRepository<TDocument> where TDocument : class
        {
            var documentQuery = new FakeDocumentQuery<TDocument>(documents);
            
            documentRepository.Setup(r => r.CreateQuery(null)).Returns(documentQuery);
        }
    }
}