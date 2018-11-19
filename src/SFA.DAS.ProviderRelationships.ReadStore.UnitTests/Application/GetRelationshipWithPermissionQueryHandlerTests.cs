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
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class GetRelationshipWithPermissionQueryHandlerTests : FluentTest<GetRelationshipWithPermissionQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenRelationshipsDoNotExist_ThenShouldReturnNoRelationships()
        {
            return RunAsync(f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Relationships.Should().BeEmpty();
            });
        }

        [Test]
        public Task Handle_WhenRelationshipsExist_ThenShouldReturnRelationships()
        {
            return RunAsync(f => f.AddRelationships(), f => f.Handle(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Relationships.Should().NotBeEmpty().And.HaveCount(3).And.AllBeOfType<RelationshipDto>().And.BeEquivalentTo(
                    new RelationshipDto {
                        EmployerAccountId = 1,
                        EmployerAccountPublicHashedId = "AAA111",
                        EmployerAccountName = "account name 1",
                        EmployerAccountLegalEntityId = 1,
                        EmployerAccountLegalEntityPublicHashedId = "ALE111",
                        EmployerAccountLegalEntityName = "legal entity name ALE111",
                        EmployerAccountProviderId = 1,
                        Ukprn = 11111111
                    },
                    new RelationshipDto {
                        EmployerAccountId = 1,
                        EmployerAccountPublicHashedId = "AAA111",
                        EmployerAccountName = "account name 1",
                        EmployerAccountLegalEntityId = 2,
                        EmployerAccountLegalEntityPublicHashedId = "ALE222",
                        EmployerAccountLegalEntityName = "legal entity name ALE222",
                        EmployerAccountProviderId = 1,
                        Ukprn = 11111111
                    },
                    new RelationshipDto {
                        EmployerAccountId = 4,
                        EmployerAccountPublicHashedId = "AAA444",
                        EmployerAccountName = "account name 4",
                        EmployerAccountLegalEntityId = 5,
                        EmployerAccountLegalEntityPublicHashedId = "ALE555",
                        EmployerAccountLegalEntityName = "legal entity name ALE555",
                        EmployerAccountProviderId = 4,
                        Ukprn = 11111111
                    });
            });
        }
    }

    public class GetRelationshipWithPermissionQueryHandlerTestsFixture
    {
        internal GetRelationshipWithPermissionQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        internal IReadStoreRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult> Handler { get; set; }
        internal Mock<IRelationshipsRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<Relationship> DocumentQuery { get; set; }
        internal List<Relationship> Permissions { get; set; }

        public GetRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            Query = new GetRelationshipWithPermissionQuery(11111111, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IRelationshipsRepository>();
            Permissions = new List<Relationship>();
            DocumentQuery = new FakeDocumentQuery<Relationship>(Permissions);

            PermissionsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = new GetRelationshipWithPermissionQueryHandler(PermissionsRepository.Object);
        }

        internal Task<GetRelationshipWithPermissionQueryResult> Handle()
        {
            return Handler.Handle(Query, CancellationToken);
        }

        public GetRelationshipWithPermissionQueryHandlerTestsFixture AddRelationships()
        {
            Permissions.AddRange(new[]
            {
                new RelationshipBuilder()
                    .WithUkprn(11111111)
                    .WithAccount(new Account(1, "AAA111","account name 1"))
                    .WithAccountProvider(new AccountProvider(1, new HashSet<Operation> { Operation.CreateCohort}))
                    .WithAccountLegalEntity(new AccountLegalEntity(1,"ALE111", "legal entity name ALE111"))
                    .Build(),
                new RelationshipBuilder()
                    .WithUkprn(11111111)
                    .WithAccount(new Account(1, "AAA111","account name 1"))
                    .WithAccountProvider(new AccountProvider(1, new HashSet<Operation> { Operation.CreateCohort}))
                    .WithAccountLegalEntity(new AccountLegalEntity(2,"ALE222", "legal entity name ALE222"))
                    .Build(),
                new RelationshipBuilder()
                    .WithUkprn(22222222)
                    .WithAccount(new Account(2, "AAA222","account name 2"))
                    .WithAccountProvider(new AccountProvider(2, new HashSet<Operation> { Operation.CreateCohort}))
                    .WithAccountLegalEntity(new AccountLegalEntity(3,"ALE333", "legal entity name ALE333"))
                    .Build(),
                new RelationshipBuilder()
                    .WithUkprn(22222222)
                    .WithAccount(new Account(3, "AAA333","account name 3"))
                    .WithAccountProvider(new AccountProvider(3, new HashSet<Operation> { Operation.CreateCohort}))
                    .WithAccountLegalEntity(new AccountLegalEntity(4,"ALE444", "legal entity name ALE444"))
                    .WithExplicitOperator(Operation.CreateCohort)
                    .Build(),
                new RelationshipBuilder()
                    .WithUkprn(11111111)
                    .WithAccount(new Account(4, "AAA444","account name 4"))
                    .WithAccountProvider(new AccountProvider(4, new HashSet<Operation> { Operation.CreateCohort}))
                    .WithAccountLegalEntity(new AccountLegalEntity(5,"ALE555", "legal entity name ALE555"))
                    .WithExplicitOperator(Operation.CreateCohort)
                    .Build(),
            });

            return this;
        }
    }
}