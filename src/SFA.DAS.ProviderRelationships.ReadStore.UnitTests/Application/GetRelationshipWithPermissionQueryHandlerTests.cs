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
                    new RelationshipDto
                    {
                        EmployerAccountId = 1,
                        EmployerAccountPublicHashedId = "AAA111",
                        EmployerAccountName = "account name 1",
                        EmployerAccountLegalEntityId = 1,
                        EmployerAccountLegalEntityPublicHashedId = "ALE111",
                        EmployerAccountLegalEntityName = "legal entity name ALE111",
                        EmployerAccountProviderId = 1,
                        Ukprn = 11111111
                    },
                    new RelationshipDto
                    {
                        EmployerAccountId = 1,
                        EmployerAccountPublicHashedId = "AAA111",
                        EmployerAccountName = "account name 1",
                        EmployerAccountLegalEntityId = 2,
                        EmployerAccountLegalEntityPublicHashedId = "ALE222",
                        EmployerAccountLegalEntityName = "legal entity name ALE222",
                        EmployerAccountProviderId = 1,
                        Ukprn = 11111111
                    },
                    new RelationshipDto
                    {
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
        internal IApiRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult> Handler { get; set; }
        internal Mock<IPermissionsRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<Relationship> DocumentQuery { get; set; }
        internal List<Relationship> Permissions { get; set; }

        public GetRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            Query = new GetRelationshipWithPermissionQuery(11111111, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IPermissionsRepository>();
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
            Permissions.AddRange(new []
            {
                new PermissionBuilder()
                    .WithAccountId(1)
                    .WithAccountPublicHashedId("AAA111")
                    .WithAccountName("account name 1")
                    .WithAccountLegalEntityId(1)
                    .WithAccountLegalEntityPublicHashedId("ALE111")
                    .WithAccountLegalEntityName("legal entity name ALE111")
                    .WithAccountProviderId(1)
                    .WithUkprn(11111111)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithAccountId(1)
                    .WithAccountPublicHashedId("AAA111")
                    .WithAccountName("account name 1")
                    .WithAccountLegalEntityId(2)
                    .WithAccountLegalEntityPublicHashedId("ALE222")
                    .WithAccountLegalEntityName("legal entity name ALE222")
                    .WithAccountProviderId(1)
                    .WithUkprn(11111111)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithAccountId(2)
                    .WithAccountPublicHashedId("AAA222")
                    .WithAccountName("account name 2")
                    .WithAccountLegalEntityId(3)
                    .WithAccountLegalEntityPublicHashedId("ALE333")
                    .WithAccountLegalEntityName("legal entity name ALE333")
                    .WithAccountProviderId(2)
                    .WithUkprn(22222222)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithAccountId(3)
                    .WithAccountPublicHashedId("AAA333")
                    .WithAccountName("account name 3")
                    .WithAccountLegalEntityId(4)
                    .WithAccountLegalEntityPublicHashedId("ALE444")
                    .WithAccountLegalEntityName("legal entity name ALE444")
                    .WithAccountProviderId(3)
                    .WithUkprn(22222222)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithAccountId(4)
                    .WithAccountPublicHashedId("AAA444")
                    .WithAccountName("account name 4")
                    .WithAccountLegalEntityId(5)
                    .WithAccountLegalEntityPublicHashedId("ALE555")
                    .WithAccountLegalEntityName("legal entity name ALE555")
                    .WithAccountProviderId(4)
                    .WithUkprn(11111111)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
            });
            
            return this;
        }
    }
}