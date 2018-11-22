using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client.UnitTests.Fakes;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.Client.UnitTests
{
    //todo: i'd be tempted to split this into 2 separate tests: 1 for methods that go to the read store, and 1 that goes to the web.api
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
            return RunAsync(f => f.AddRelationshipsToRelationshipsResponse(), f => f.GetRelationshipsWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Should().BeEquivalentTo(f.RelationshipsResponse);
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
        
        [Test]
        public Task HealthCheck_WhenHealthCheckFails_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetHealthCheckFailure(), f => f.HealthCheck(), (f, r) => r.Should().Throw<HttpRequestException>());
        }
    }

    public class ProviderRelationshipsApiClientTestsFixture
    {
        public PermissionRequest PermissionRequest { get; set; }
        public RelationshipsRequest RelationshipsRequest { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IProviderRelationshipsApiClient ProviderRelationshipsApiClient { get; set; }
        public HttpClient Client { get; set; }
        public FakeHttpMessageHandler HttpMessageHandler { get; set; }
        internal Mock<IReadStoreMediator> Mediator { get; set; }
        public RelationshipsResponse RelationshipsResponse { get; set; }
        public List<RelationshipDto> Relationships { get; set; }

        public ProviderRelationshipsApiClientTestsFixture()
        {
            RelationshipsResponse = new RelationshipsResponse {Relationships = new List<RelationshipDto>()};
            Relationships = new List<RelationshipDto>();
            CancellationToken = CancellationToken.None;
            HttpMessageHandler = new FakeHttpMessageHandler();
            Client = new HttpClient(HttpMessageHandler) { BaseAddress = new Uri("https://foo.bar") };
            Mediator = new Mock<IReadStoreMediator>();
            
            //todo: for test inject hard-coded string instead?
            var stringBody = JsonConvert.SerializeObject(RelationshipsResponse);
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage() { Content = new StringContent(stringBody) };
            
            ProviderRelationshipsApiClient = new ProviderRelationshipsApiClient(Client, Mediator.Object);
        }

        public Task<RelationshipsResponse> GetRelationshipsWithPermission()
        {
            RelationshipsRequest = new RelationshipsRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };
            
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

        public Task HealthCheck()
        {
            return ProviderRelationshipsApiClient.HealthCheck();
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
        
        public ProviderRelationshipsApiClientTestsFixture AddRelationshipsToRelationshipsResponse()
        {
            RelationshipsResponse.Relationships = new []
            {
                new RelationshipDto(),
                new RelationshipDto()
            };
            
            //todo: for test inject hard-coded string instead?
            var stringBody = JsonConvert.SerializeObject(RelationshipsResponse);
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage() { Content = new StringContent(stringBody) };
            
            return this;
        }

        public ProviderRelationshipsApiClientTestsFixture SetHealthCheckFailure()
        {
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("") };
            
            return this;
        }
    }
}