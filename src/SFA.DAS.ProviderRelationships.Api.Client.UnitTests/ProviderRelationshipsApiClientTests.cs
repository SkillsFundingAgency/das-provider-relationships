using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.Client.UnitTests
{
    [TestFixture]
    [Parallelizable]
    public class ProviderRelationshipsApiClientTests : FluentTest<ProviderRelationshipsApiClientTestsFixture>
    {
        [Test]
        public Task GetRelationshipsWithPermission_WhenRelationshipsDoNotExist_ThenShouldReturnNoRelationships()
        {
            return RunAsync(f => f.GetRelationshipsWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Relationships.Should().BeEmpty();
            });
        }
        
        [Test]
        public Task GetRelationshipsWithPermission_WhenRelationshipsExist_ThenShouldReturnRelationships()
        {
            return RunAsync(f => f.AddRelationships(), f => f.GetRelationshipsWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Relationships.Should().NotBeEmpty().And.BeSameAs(f.Relationships);
            });
        }
        
        [Test]
        public Task HasPermission_WhenPermissionIsNotGranted_ThenShouldReturnFalse()
        {
            return RunAsync(f => f.HasPermission(), (f, r) => r.Should().BeFalse());
        }
        
        [Test]
        public Task HasPermission_WhenPermissionIsGranted_ThenShouldReturnFalse()
        {
            return RunAsync(f => f.AddRelationships(), f => f.HasPermission(), (f, r) => r.Should().BeTrue());
        }
        
        [Test]
        public Task HasRelationshipWithPermission_WhenRelationshipsDoNotExist_ThenShouldReturnFalse()
        {
            return RunAsync(f => f.HasRelationshipWithPermission(), (f, r) => r.Should().BeFalse());
        }
        
        [Test]
        public Task HasRelationshipWithPermission_WhenRelationshipsExist_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.AddRelationships(), f => f.HasRelationshipWithPermission(), (f, r) => r.Should().BeTrue());
        }
    }

    public class ProviderRelationshipsApiClientTestsFixture
    {
        public PermissionRequest PermissionRequest { get; set; }
        public RelationshipsRequest RelationshipsRequest { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IProviderRelationshipsApiClient ProviderRelationshipsApiClient { get; set; }
        internal Mock<IReadStoreMediator> Mediator { get; set; }
        public List<RelationshipDto> Relationships { get; set; }

        public ProviderRelationshipsApiClientTestsFixture()
        {
            Relationships = new List<RelationshipDto>();
            CancellationToken = CancellationToken.None;
            Mediator = new Mock<IReadStoreMediator>();
            ProviderRelationshipsApiClient = new ProviderRelationshipsApiClient(null, Mediator.Object);
        }

        public Task<RelationshipsResponse> GetRelationshipsWithPermission()
        {
            RelationshipsRequest = new RelationshipsRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };
            
            Mediator.Setup(m => m.Send(It.Is<GetRelationshipWithPermissionQuery>(q => q.Ukprn == RelationshipsRequest.Ukprn && q.Operation == RelationshipsRequest.Operation), CancellationToken))
                .ReturnsAsync(new GetRelationshipWithPermissionQueryResult { Relationships = Relationships });
            
            return ProviderRelationshipsApiClient.GetRelationshipsWithPermission(RelationshipsRequest, CancellationToken);
        }

        public Task<bool> HasPermission()
        {
            PermissionRequest = new PermissionRequest
            {
                Ukprn = 11111111,
                EmployerAccountLegalEntityId = 1,
                Operation = Operation.CreateCohort
            };
            
            Mediator.Setup(m => m.Send(It.Is<HasPermissionQuery>(q => q.Ukprn == PermissionRequest.Ukprn && q.EmployerAccountLegalEntityId == PermissionRequest.EmployerAccountLegalEntityId && q.Operation == PermissionRequest.Operation), CancellationToken))
                .ReturnsAsync(() => Relationships.Any());
            
            return ProviderRelationshipsApiClient.HasPermission(PermissionRequest, CancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission()
        {
            RelationshipsRequest = new RelationshipsRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };
            
            Mediator.Setup(m => m.Send(It.Is<HasRelationshipWithPermissionQuery>(q => q.Ukprn == RelationshipsRequest.Ukprn && q.Operation == RelationshipsRequest.Operation), CancellationToken))
                .ReturnsAsync(() => Relationships.Any());
            
            return ProviderRelationshipsApiClient.HasRelationshipWithPermission(RelationshipsRequest, CancellationToken);
        }

        public ProviderRelationshipsApiClientTestsFixture AddRelationships()
        {
            Relationships.AddRange(new []
            {
                new RelationshipDto(),
                new RelationshipDto()
            });
            
            return this;
        }
    }
}