using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.Client.UnitTests
{
    [TestFixture]
    public class GetHasRelationshipWithPermissionHandlerTests : FluentTest<GetHasRelationshipWithPermissionHandlerTestsFixture>
    {
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public Task Handle_WhenCheckingForAnyProviderRelationshipsWithPermission_ThenShouldReturnCorrectResult(long ukprn, bool result)
        {
            var request = new GetHasRelationshipWithPermissionQuery {
                Ukprn = ukprn,
                Permission = PermissionEnumDto.CreateCohort
            };
            return RunAsync(f => f.Handler.Handle(request, CancellationToken.None), (f, r) => r.Should().Be(result));
        }
    }

    public class GetHasRelationshipWithPermissionHandlerTestsFixture
    {
        public GetHasRelationshipWithPermissionHandler Handler;

        public IDocumentReadOnlyRepository<ProviderRelationship> ReadRepository  { get; set; }
        public List<ProviderRelationship> ProviderRelationships { get; set; }

        public GetHasRelationshipWithPermissionHandlerTestsFixture()
        {

            ProviderRelationships = new List<ProviderRelationship>
            {
                new ProviderRelationship { Ukprn = 1, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> {new GrantPermission { Permission = PermissionEnumDto.CreateCohort }}) },
                new ProviderRelationship { Ukprn = 1, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) },
                new ProviderRelationship { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> {new GrantPermission { Permission = PermissionEnumDto.CreateCohort }}) },
                new ProviderRelationship { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> {new GrantPermission { Permission = PermissionEnumDto.CreateCohort }}) },
                new ProviderRelationship { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) },
                new ProviderRelationship { Ukprn = 3, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) }
            };

            ReadRepository = new FakeReadOnlyRepository<ProviderRelationship>(ProviderRelationships, null);

            Handler = new GetHasRelationshipWithPermissionHandler(ReadRepository);
        }
    }
}