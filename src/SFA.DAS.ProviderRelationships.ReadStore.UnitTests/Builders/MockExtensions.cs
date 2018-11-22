using System.Collections.Generic;
using Moq;
using SFA.DAS.CosmosDb;
using SFA.DAS.CosmosDb.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders
{
    public static class MockExtensions
    {
        public static void SetupCosmosCreateQueryToReturn<T, TDocument>(this Mock<T> mock, IEnumerable<TDocument> list) where T : class, IDocumentRepository<TDocument> where TDocument : class
        {
            var documentQuery = new FakeDocumentQuery<TDocument>(list);
            mock.Setup(x => x.CreateQuery(null)).Returns(documentQuery);
        }
    }


}
