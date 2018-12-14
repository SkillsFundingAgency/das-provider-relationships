using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CosmosDb.Testing;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.Client.UnitTests.ReadStore.Application.Queries
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
            return RunAsync(f => f.AddAccountProviderLegalEntities(), f => f.Handle(), (f, r) => r.Should().BeTrue());
        }
    }

    public class HasPermissionQueryHandlerTestsFixture
    {
        internal HasPermissionQuery Query { get; set; }
        public CancellationToken CancellationToken { get; set; }
        internal IReadStoreRequestHandler<HasPermissionQuery, bool> Handler { get; set; }
        internal Mock<IAccountProviderLegalEntitiesRepository> PermissionsRepository { get; set; }
        internal IOrderedQueryable<AccountProviderLegalEntityDto> DocumentQuery { get; set; }
        internal List<AccountProviderLegalEntityDto> Permissions { get; set; }

        public HasPermissionQueryHandlerTestsFixture()
        {
            Query = new HasPermissionQuery(11111111, 1, Operation.CreateCohort);
            CancellationToken = CancellationToken.None;
            PermissionsRepository = new Mock<IAccountProviderLegalEntitiesRepository>();
            Permissions = new List<AccountProviderLegalEntityDto>();
            DocumentQuery = new FakeDocumentQuery<AccountProviderLegalEntityDto>(Permissions);

            PermissionsRepository.Setup(r => r.CreateQuery(null)).Returns(DocumentQuery);

            Handler = new HasPermissionQueryHandler(PermissionsRepository.Object);
        }

        public Task<bool> Handle()
        {
            return Handler.Handle(Query, CancellationToken);
        }
        
        public HasPermissionQueryHandlerTestsFixture AddAccountProviderLegalEntities()
        {
            Permissions.AddRange(new []
            {
                DocumentActivator.CreateInstance<AccountProviderLegalEntityDto>()
                    .Set(r => r.AccountLegalEntityId, 1)
                    .Set(r => r.Ukprn, 11111111)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<AccountProviderLegalEntityDto>()
                    .Set(r => r.AccountLegalEntityId, 2)
                    .Set(r => r.Ukprn, 11111111)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<AccountProviderLegalEntityDto>()
                    .Set(r => r.AccountLegalEntityId, 3)
                    .Set(r => r.Ukprn, 22222222)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<AccountProviderLegalEntityDto>()
                    .Set(r => r.AccountLegalEntityId, 4)
                    .Set(r => r.Ukprn, 22222222)
                    .Add(r => r.Operations, Operation.CreateCohort),
                DocumentActivator.CreateInstance<AccountProviderLegalEntityDto>()
                    .Set(r => r.AccountLegalEntityId, 5)
                    .Set(r => r.Ukprn, 11111111)
                    .Add(r => r.Operations, Operation.CreateCohort)
            });
            
            return this;
        }
    }
}