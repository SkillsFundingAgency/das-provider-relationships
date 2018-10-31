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
        public IDocumentReadOnlyRepository<ProviderPermissions> DocumentReadOnlyRepository { get; set; }
        public List<ProviderPermissions> ProviderPermissions { get; set; }

        public HasRelationshipWithPermissionQueryHandlerTestsFixture()
        {
            ProviderPermissions = new List<ProviderPermissions>
            {
                new ProviderPermissions { Ukprn = 1, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> { new GrantPermission { Permission = Operation.CreateCohort } }) },
                new ProviderPermissions { Ukprn = 1, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) },
                new ProviderPermissions { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> { new GrantPermission { Permission = Operation.CreateCohort } }) },
                new ProviderPermissions { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> { new GrantPermission { Permission = Operation.CreateCohort } }) },
                new ProviderPermissions { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) },
                new ProviderPermissions { Ukprn = 3, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) }
            };
            
            DocumentReadOnlyRepository = new FakeReadOnlyRepository<ProviderPermissions>(ProviderPermissions, null);
            Handler = new HasRelationshipWithPermissionQueryHandler(DocumentReadOnlyRepository);
        }

        public Task<bool> Handle(long ukprn)
        {
            return Handler.Handle(new HasRelationshipWithPermissionQuery(ukprn, Operation.CreateCohort), CancellationToken.None);
        }
    }
}