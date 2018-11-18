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

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class HasPermissionQueryHandlerTests : FluentTest<HasPermissionQueryHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenPermissionIsNotGranted_ThenShouldReturnFalse()
        {
            return RunAsync(f => f.Handle(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task Handle_WhenPermissionIsGranted_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.AddRelationships(), f => f.Handle(), (f, r) => r.Should().BeTrue());
        }
    }

    public class HasPermissionQueryHandlerTestsFixture
    {
        internal HasPermissionQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        internal IApiRequestHandler<HasPermissionQuery, bool> Handler { get; set; }
        internal Mock<IRelationshipsRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<Relationship> DocumentQuery { get; set; }
        internal List<Relationship> Permissions { get; set; }

        public HasPermissionQueryHandlerTestsFixture()
        {
            Query = new HasPermissionQuery(11111111, 1, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IRelationshipsRepository>();
            Permissions = new List<Relationship>();
            DocumentQuery = new FakeDocumentQuery<Relationship>(Permissions);

            PermissionsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = new HasPermissionQueryHandler(PermissionsRepository.Object);
        }

        public Task<bool> Handle()
        {
            return Handler.Handle(Query, CancellationToken);
        }

        public HasPermissionQueryHandlerTestsFixture AddRelationships()
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


                //new RelationshipBuilder()
                //    .WithAccountId(1)
                //    .WithAccountPublicHashedId("AAA111")
                //    .WithAccountName("account name 1")
                //    .WithAccountLegalEntityId(1)
                //    .WithAccountLegalEntityPublicHashedId("ALE111")
                //    .WithAccountLegalEntityName("legal entity name ALE111")
                //    .WithAccountProviderId(1)
                //    .WithUkprn(11111111)
                //    .WithOperation(Operation.CreateCohort)
                //    .Build(),
                //new RelationshipBuilder()
                //    .WithAccountId(1)
                //    .WithAccountPublicHashedId("AAA111")
                //    .WithAccountName("account name 1")
                //    .WithAccountLegalEntityId(2)
                //    .WithAccountLegalEntityPublicHashedId("ALE222")
                //    .WithAccountLegalEntityName("legal entity name ALE222")
                //    .WithAccountProviderId(1)
                //    .WithUkprn(11111111)
                //    .WithOperation(Operation.CreateCohort)
                //    .Build(),
                //new RelationshipBuilder()
                //    .WithAccountId(2)
                //    .WithAccountPublicHashedId("AAA222")
                //    .WithAccountName("account name 2")
                //    .WithAccountLegalEntityId(3)
                //    .WithAccountLegalEntityPublicHashedId("ALE333")
                //    .WithAccountLegalEntityName("legal entity name ALE333")
                //    .WithAccountProviderId(2)
                //    .WithUkprn(22222222)
                //    .WithOperation(Operation.CreateCohort)
                //    .Build(),
                //new RelationshipBuilder()
                //    .WithAccountId(3)
                //    .WithAccountPublicHashedId("AAA333")
                //    .WithAccountName("account name 3")
                //    .WithAccountLegalEntityId(4)
                //    .WithAccountLegalEntityPublicHashedId("ALE444")
                //    .WithAccountLegalEntityName("legal entity name ALE444")
                //    .WithAccountProviderId(3)
                //    .WithUkprn(22222222)
                //    .WithOperation(Operation.CreateCohort)
                //    .Build(),
                //new RelationshipBuilder()
                //    .WithAccountId(4)
                //    .WithAccountPublicHashedId("AAA444")
                //    .WithAccountName("account name 4")
                //    .WithAccountLegalEntityId(5)
                //    .WithAccountLegalEntityPublicHashedId("ALE555")
                //    .WithAccountLegalEntityName("legal entity name ALE555")
                //    .WithAccountProviderId(4)
                //    .WithUkprn(11111111)
                //    .WithOperation(Operation.CreateCohort)
                //    .Build(),
            });

            return this;
        }
    }
}