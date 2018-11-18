using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Fakes;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application
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
        internal IApiRequestHandler<HasRelationshipWithPermissionQuery, bool> Handler { get; set; }
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
            Permissions.AddRange(new[]
            {
                new RelationshipBuilder()
                    .WithAccountProvider(new AccountProvider(11111111, 1, "AAA111","account name 1", 1))
                    .WithAccountProviderLegalEntity(new AccountProviderLegalEntity(1,1,"ALE111", "legal entity name ALE111"))
                    .WithPermissionsOperator(Operation.CreateCohort)
                    .Build(),
                new RelationshipBuilder()
                    .WithAccountProvider(new AccountProvider(11111111, 1, "AAA111","account name 1", 1))
                    .WithAccountProviderLegalEntity(new AccountProviderLegalEntity(2,2,"ALE222", "legal entity name ALE222"))
                    .WithPermissionsOperator(Operation.CreateCohort)
                    .Build(),
                new RelationshipBuilder()
                    .WithAccountProvider(new AccountProvider(22222222, 2, "AAA222","account name 2", 2))
                    .WithAccountProviderLegalEntity(new AccountProviderLegalEntity(3,3,"ALE333", "legal entity name ALE333"))
                    .WithPermissionsOperator(Operation.CreateCohort)
                    .Build(),
                new RelationshipBuilder()
                    .WithAccountProvider(new AccountProvider(22222222, 3, "AAA333","account name 3", 3))
                    .WithAccountProviderLegalEntity(new AccountProviderLegalEntity(4,4,"ALE444", "legal entity name ALE444"))
                    .WithPermissionsOperator(Operation.CreateCohort)
                    .Build(),
                new RelationshipBuilder()
                    .WithAccountProvider(new AccountProvider(11111111, 4, "AAA444","account name 4", 4))
                    .WithAccountProviderLegalEntity(new AccountProviderLegalEntity(5,5,"ALE555", "legal entity name ALE555"))
                    .WithPermissionsOperator(Operation.CreateCohort)
                    .Build(),
            });

            return this;
        }
    }
}