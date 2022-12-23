using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.CosmosDb.Testing;
using SFA.DAS.ProviderRelationships.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.ReadStore.Application.Queries
{
    [TestFixture]
    [Parallelizable]
    public class HasRelationshipWithPermissionQueryHandlerTests : FluentTest<HasRelationshipWithPermissionQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenRelationshipsDoNotExist_ThenShouldReturnFalse()
        {
            return TestAsync(f => f.Handle(), (f, r) => r.Should().BeFalse());
        }
        
        [Test]
        public Task Handle_WhenRelationshipsExist_ThenShouldReturnTrue()
        {
            return TestAsync(f => f.AddAccountProviderLegalEntities(), f => f.Handle(), (f, r) => r.Should().BeTrue());
        }
    }

    public class HasRelationshipWithPermissionQueryHandlerTestsFixture
    {
        internal HasRelationshipWithPermissionQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        internal IRequestHandler<HasRelationshipWithPermissionQuery, bool> Handler { get; set; }
        internal Mock<IAccountProviderLegalEntitiesRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<AccountProviderLegalEntity> DocumentQuery { get; set; }
        internal List<AccountProviderLegalEntity> Permissions { get; set; }

        public HasRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            Query = new HasRelationshipWithPermissionQuery(11111111, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IAccountProviderLegalEntitiesRepository>();
            Permissions = new List<AccountProviderLegalEntity>();
            DocumentQuery = new FakeDocumentQuery<AccountProviderLegalEntity>(Permissions);

            PermissionsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = new HasRelationshipWithPermissionQueryHandler(PermissionsRepository.Object);
        }

        public Task<bool> Handle()
        {
            return Handler.Handle(Query, CancellationToken);
        }
        
        public HasRelationshipWithPermissionQueryHandlerTestsFixture AddAccountProviderLegalEntities()
        {
            var accountProviderLegalEntity = new Mock<AccountProviderLegalEntity>();
            accountProviderLegalEntity.SetupGet(aple => aple.AccountLegalEntityId).Returns(1);
            accountProviderLegalEntity.SetupGet(aple => aple.Ukprn).Returns(11111111);
            accountProviderLegalEntity.SetupGet(aple => aple.Operations).Returns(new[] {Operation.CreateCohort});

            Permissions.AddRange(new []
            {
                accountProviderLegalEntity.Object
            });
            
            return this;
        }
    }
}