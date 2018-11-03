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
        internal Mock<IPermissionsRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<Permission> DocumentQuery { get; set; }
        internal List<Permission> Permissions { get; set; }

        public HasRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            Query = new HasRelationshipWithPermissionQuery(11111111, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IPermissionsRepository>();
            Permissions = new List<Permission>();
            DocumentQuery = new DocumentQueryFake<Permission>(Permissions);

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
                new PermissionBuilder()
                    .WithEmployerAccountId(1)
                    .WithEmployerAccountPublicHashedId("AAA111")
                    .WithEmployerAccountName("account name 1")
                    .WithEmployerAccountLegalEntityId(1)
                    .WithEmployerAccountLegalEntityPublicHashedId("ALE111")
                    .WithEmployerAccountLegalEntityName("legal entity name ALE111")
                    .WithEmployerAccountProviderId(1)
                    .WithUkprn(11111111)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithEmployerAccountId(1)
                    .WithEmployerAccountPublicHashedId("AAA111")
                    .WithEmployerAccountName("account name 1")
                    .WithEmployerAccountLegalEntityId(2)
                    .WithEmployerAccountLegalEntityPublicHashedId("ALE222")
                    .WithEmployerAccountLegalEntityName("legal entity name ALE222")
                    .WithEmployerAccountProviderId(1)
                    .WithUkprn(11111111)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithEmployerAccountId(2)
                    .WithEmployerAccountPublicHashedId("AAA222")
                    .WithEmployerAccountName("account name 2")
                    .WithEmployerAccountLegalEntityId(3)
                    .WithEmployerAccountLegalEntityPublicHashedId("ALE333")
                    .WithEmployerAccountLegalEntityName("legal entity name ALE333")
                    .WithEmployerAccountProviderId(2)
                    .WithUkprn(22222222)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithEmployerAccountId(3)
                    .WithEmployerAccountPublicHashedId("AAA333")
                    .WithEmployerAccountName("account name 3")
                    .WithEmployerAccountLegalEntityId(4)
                    .WithEmployerAccountLegalEntityPublicHashedId("ALE444")
                    .WithEmployerAccountLegalEntityName("legal entity name ALE444")
                    .WithEmployerAccountProviderId(3)
                    .WithUkprn(22222222)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
                new PermissionBuilder()
                    .WithEmployerAccountId(4)
                    .WithEmployerAccountPublicHashedId("AAA444")
                    .WithEmployerAccountName("account name 4")
                    .WithEmployerAccountLegalEntityId(5)
                    .WithEmployerAccountLegalEntityPublicHashedId("ALE555")
                    .WithEmployerAccountLegalEntityName("legal entity name ALE555")
                    .WithEmployerAccountProviderId(4)
                    .WithUkprn(11111111)
                    .WithOperation(Operation.CreateCohort)
                    .Build(),
            });
            
            return this;
        }
    }
}