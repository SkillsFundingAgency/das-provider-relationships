using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CosmosDb.Testing;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class HasRelationshipWithPermissionQueryHandlerTests : FluentTest<HasRelationshipWithPermissionQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenRelationshipsDoNotExist_ThenShouldReturnFalse()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeFalse());
        }
        
        [Test]
        public Task Handle_WhenRelationshipsExist_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.AddRelationships(), f => f.Handle(), (f, r) => r.Should().BeTrue());
        }
    }

    public class HasRelationshipWithPermissionQueryHandlerTestsFixture
    {
        internal HasRelationshipWithPermissionQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        internal IReadStoreRequestHandler<HasRelationshipWithPermissionQuery, bool> Handler { get; set; }
        internal Mock<IRelationshipsRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<Relationship> DocumentQuery { get; set; }
        internal List<Relationship> Permissions { get; set; }

        public HasRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            Query = new HasRelationshipWithPermissionQuery(11111111, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IRelationshipsRepository>();
            Permissions = new List<Relationship>();
            DocumentQuery = new FakeDocumentQuery<Relationship>(Permissions);

            PermissionsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = new HasRelationshipWithPermissionQueryHandler(PermissionsRepository.Object);
        }

        public Task<bool> Handle()
        {
            return Handler.Handle(Query, CancellationToken);
        }
        
        public HasRelationshipWithPermissionQueryHandlerTestsFixture AddRelationships()
        {
            Permissions.AddRange(new []
            {
                DocumentActivator.CreateInstance<Relationship>()
                    .Set(r => r.AccountId, 1)
                    .Set(r => r.AccountLegalEntityId, 1)
                    .Set(r => r.AccountProviderId, 1)
                    .Set(r => r.Ukprn, 11111111)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<Relationship>()
                    .Set(r => r.AccountId, 1)
                    .Set(r => r.AccountLegalEntityId, 2)
                    .Set(r => r.AccountProviderId, 1)
                    .Set(r => r.Ukprn, 11111111)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<Relationship>()
                    .Set(r => r.AccountId, 2)
                    .Set(r => r.AccountLegalEntityId, 3)
                    .Set(r => r.AccountProviderId, 2)
                    .Set(r => r.Ukprn, 22222222)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<Relationship>()
                    .Set(r => r.AccountId, 3)
                    .Set(r => r.AccountLegalEntityId, 4)
                    .Set(r => r.AccountProviderId, 3)
                    .Set(r => r.Ukprn, 22222222)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<Relationship>()
                    .Set(r => r.AccountId, 4)
                    .Set(r => r.AccountLegalEntityId, 5)
                    .Set(r => r.AccountProviderId, 4)
                    .Set(r => r.Ukprn, 11111111)
                    .Add(r => r.Operations, Operation.CreateCohort)
            });
            
            return this;
        }
    }
}