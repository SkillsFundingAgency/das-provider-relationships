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
    public class ProviderRelationshipServiceTests : FluentTest<ProviderRelationshipServiceTestsFixture>
    {
        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public Task WhenCheckingForAnyProviderRelationshipsWithPermission_ThenShouldReturnCorrectResult(long ukprn, bool result)
        {
            return RunAsync(f => f.Service.HasRelationshipWithPermission(ukprn, PermissionEnumDto.CreateCohort, CancellationToken.None), 
            (f, r) => r.Should().Be(result));
        }
    }

    public class ProviderRelationshipServiceTestsFixture
    {
        public ProviderRelationshipService Service;
        public IDocumentReadOnlyRepository<ProviderPermissions> ReadRepository { get; set; }
        public List<ProviderPermissions> ProviderRelationships { get; set; }

        public ProviderRelationshipServiceTestsFixture()
        {

            ProviderRelationships = new List<ProviderPermissions>
            {
                new ProviderPermissions { Ukprn = 1, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> {new GrantPermission { Permission = PermissionEnumDto.CreateCohort }}) },
                new ProviderPermissions { Ukprn = 1, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) },
                new ProviderPermissions { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> {new GrantPermission { Permission = PermissionEnumDto.CreateCohort }}) },
                new ProviderPermissions { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission> {new GrantPermission { Permission = PermissionEnumDto.CreateCohort }}) },
                new ProviderPermissions { Ukprn = 2, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) },
                new ProviderPermissions { Ukprn = 3, GrantPermissions = new List<GrantPermission>( new List<GrantPermission>()) }
            };

            ReadRepository = new FakeReadOnlyRepository<ProviderPermissions>(ProviderRelationships, null);

            Service = new ProviderRelationshipService(ReadRepository);
        }
    }
}