using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Application;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class HasRelationshipWithPermissionQueryHandlerTests : FluentTest<HasRelationshipWithPermissionQueryHandlerTestsFixture>
    {
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public Task Handle_WhenHandlingHasRelationshipWithPermissionQuery_ThenShouldReturnHasRelationshipWithPermissionQueryResponse(long ukprn, bool result)
        {
            return RunAsync(f => f.Handle(ukprn), (f, r) => r.Should().Be(result));
        }
    }

    public class HasRelationshipWithPermissionQueryHandlerTestsFixture
    {
        internal HasRelationshipWithPermissionQuery HasRelationshipWithPermissionQuery { get; set; }
        internal IRequestHandler<HasRelationshipWithPermissionQuery, bool> Handler { get; set; }
        public IReadOnlyDocumentRepository<ProviderPermission> DocumentReadOnlyRepository { get; set; }
        public List<ProviderPermission> ProviderPermissions { get; set; }

        public HasRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            ProviderPermissions = new List<ProviderPermission>
            {
                new ProviderPermission { Ukprn = 1, Operations = new List<Operation> { Operation.CreateCohort } },
                new ProviderPermission { Ukprn = 1, Operations = new List<Operation>() },
                new ProviderPermission { Ukprn = 2, Operations = new List<Operation> { Operation.CreateCohort } },
                new ProviderPermission { Ukprn = 2, Operations = new List<Operation> { Operation.CreateCohort } },
                new ProviderPermission { Ukprn = 2, Operations = new List<Operation>() },
                new ProviderPermission { Ukprn = 3, Operations = new List<Operation>() }
            };
            
            DocumentReadOnlyRepository = new FakeReadOnlyDocumentRepository<ProviderPermission>(ProviderPermissions, null);
            Handler = new HasRelationshipWithPermissionQueryHandler(DocumentReadOnlyRepository);
        }

        public Task<bool> Handle(long ukprn)
        {
            return Handler.Handle(new HasRelationshipWithPermissionQuery(ukprn, Operation.CreateCohort), CancellationToken.None);
        }
    }
}