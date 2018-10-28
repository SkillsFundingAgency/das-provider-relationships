using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
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
        //public Mock<ICosmosQueryWrapper<ProviderPermissions>> CosmosQueryWrapper;
        public ICosmosQueryWrapper<ProviderPermissions> CosmosQueryWrapper;
        public IDocumentReadOnlyRepository<ProviderPermissions> ReadRepository  { get; set; }
        public List<ProviderPermissions> ProviderRelationships { get; set; }

        public GetHasRelationshipWithPermissionHandlerTestsFixture()
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
            //CosmosQueryWrapper = new FakeCosmosQueryWrapper<ProviderPermissions>(ProviderRelationships);
            //CosmosQueryWrapper = new Mock<ICosmosQueryWrapper<ProviderPermissions>>();
            //CosmosQueryWrapper.Setup()
            Handler = new GetHasRelationshipWithPermissionHandler(ReadRepository);
        }
    }
}