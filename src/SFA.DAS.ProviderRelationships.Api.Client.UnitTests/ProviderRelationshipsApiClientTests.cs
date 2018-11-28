using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
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
        public Task GetAccountProviderLegalEntitiesWithPermission_WhenAccountProviderLegalEntitiesDoNotExist_ThenShouldReturnNoAccountProviderLegalEntities()
        {
            return RunAsync(f => f.GetAccountProviderLegalEntitiesWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Relationships.Should().BeEmpty();
            });
        }
        
        [Test]
        public Task GetAccountProviderLegalEntitiesWithPermission_WhenAccountProviderLegalEntitiesExist_ThenShouldReturnAccountProviderLegalEntities()
        {
            return RunAsync(f => f.AddRelationshipsToRelationshipsResponse(), f => f.GetAccountProviderLegalEntitiesWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Should().BeEquivalentTo(f.AccountProviderLegalEntitiesResponse);
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
            return RunAsync(f => f.SetHealthCheckFailure(), f => f.HealthCheck(), (f, r) => r.Should().Throw<RestClientException>());
        }
    }

    public class ProviderRelationshipsApiClientTestsFixture
    {
        public PermissionRequest PermissionRequest { get; set; }
        public AccountProviderLegalEntitiesRequest AccountProviderLegalEntitiesRequest { get; set; }
        public RelationshipsRequest RelationshipsRequest { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IProviderRelationshipsApiClient ProviderRelationshipsApiClient { get; set; }
        public HttpClient HttpClient { get; set; }

        public IRestClient RestClient { get; set; }
        public FakeHttpMessageHandler HttpMessageHandler { get; set; }
        internal Mock<IReadStoreMediator> Mediator { get; set; }
        public AccountProviderLegalEntitiesResponse AccountProviderLegalEntitiesResponse { get; set; }
        public List<AccountProviderLegalEntityDto> Relationships { get; set; }

        public ProviderRelationshipsApiClientTestsFixture()
        {
            AccountProviderLegalEntitiesResponse = new AccountProviderLegalEntitiesResponse {Relationships = new List<AccountProviderLegalEntityDto>()};
            Relationships = new List<AccountProviderLegalEntityDto>();
            CancellationToken = CancellationToken.None;
            HttpMessageHandler = new FakeHttpMessageHandler();
            HttpClient = new HttpClient(HttpMessageHandler) { BaseAddress = new Uri("https://foo.bar") };
            RestClient = new RestClient(HttpClient);
            Mediator = new Mock<IReadStoreMediator>();
            
            SetupHttpClientGetToReturnRelationshipsResponse();
            
            ProviderRelationshipsApiClient = new ProviderRelationshipsApiClient(RestClient, Mediator.Object);
        }

        public Task<AccountProviderLegalEntitiesResponse> GetAccountProviderLegalEntitiesWithPermission()
        {
            AccountProviderLegalEntitiesRequest = new AccountProviderLegalEntitiesRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };
            
            return ProviderRelationshipsApiClient.GetAccountProviderLegalEntitiesWithPermission(AccountProviderLegalEntitiesRequest, CancellationToken);
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
            AccountProviderLegalEntitiesRequest = new AccountProviderLegalEntitiesRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };

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
                new AccountProviderLegalEntityDto(),
                new AccountProviderLegalEntityDto()
            });
            
            return this;
        }
        
        public ProviderRelationshipsApiClientTestsFixture AddRelationshipsToRelationshipsResponse()
        {
            AccountProviderLegalEntitiesResponse.Relationships = new []
            {
                new AccountProviderLegalEntityDto(),
                new AccountProviderLegalEntityDto()
            };

            SetupHttpClientGetToReturnRelationshipsResponse();
            
            return this;
        }
        
        public ProviderRelationshipsApiClientTestsFixture SetHealthCheckFailure()
        {
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Kaboom"),
                ReasonPhrase = "Internal server error",
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://example.com/healthcheck")
            };
            
            return this;
        }

        private void SetupHttpClientGetToReturnRelationshipsResponse()
        {
            //todo: for test inject hard-coded string instead?
            var stringBody = JsonConvert.SerializeObject(AccountProviderLegalEntitiesResponse);
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage { Content = new StringContent(stringBody, Encoding.Default, "application/json") };
        }
    }
}