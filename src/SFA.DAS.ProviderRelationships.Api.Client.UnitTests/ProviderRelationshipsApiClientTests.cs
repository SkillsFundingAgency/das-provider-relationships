using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Api.Client.UnitTests.Fakes;
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
        public Task GetAccountProviderLegalEntitiesWithPermission_WhenAccountProviderLegalEntitiesDoNotExist_ThenShouldReturnNoAccountProviderLegalEntities()
        {
            return RunAsync(f => f.GetAccountProviderLegalEntitiesWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.AccountProviderLegalEntities.Should().BeEmpty();
            });
        }
        
        [Test]
        public Task GetAccountProviderLegalEntitiesWithPermission_WhenAccountProviderLegalEntitiesExist_ThenShouldReturnAccountProviderLegalEntities()
        {
            return RunAsync(f => f.AddAccountProviderLegalEntitiesToAccountProviderLegalEntitiesResponse(), f => f.GetAccountProviderLegalEntitiesWithPermission(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Should().BeEquivalentTo(f.GetAccountProviderLegalEntitiesWithPermissionResponse);
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
            return RunAsync(f => f.SetHealthCheckFailure(), f => f.HealthCheck(), (f, r) => r.Should().Throw<RestHttpClientException>());
        }
    }

    public class ProviderRelationshipsApiClientTestsFixture
    {
        public HasPermissionRequest HasPermissionRequest { get; set; }
        public GetAccountProviderLegalEntitiesWithPermissionRequest GetAccountProviderLegalEntitiesWithPermissionRequest { get; set; }
        public HasRelationshipWithPermissionRequest HasRelationshipWithPermissionRequest { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IProviderRelationshipsApiClient ProviderRelationshipsApiClient { get; set; }
        public HttpClient HttpClient { get; set; }
        public IRestHttpClient RestHttpClient { get; set; }
        public FakeHttpMessageHandler HttpMessageHandler { get; set; }
        internal Mock<IMediator> Mediator { get; set; }
        public GetAccountProviderLegalEntitiesWithPermissionResponse GetAccountProviderLegalEntitiesWithPermissionResponse { get; set; }
        public List<AccountProviderLegalEntityDto> Relationships { get; set; }

        public ProviderRelationshipsApiClientTestsFixture()
        {
            GetAccountProviderLegalEntitiesWithPermissionResponse = new GetAccountProviderLegalEntitiesWithPermissionResponse {AccountProviderLegalEntities = new List<AccountProviderLegalEntityDto>()};
            Relationships = new List<AccountProviderLegalEntityDto>();
            CancellationToken = CancellationToken.None;
            HttpMessageHandler = new FakeHttpMessageHandler();
            HttpClient = new HttpClient(HttpMessageHandler) { BaseAddress = new Uri("https://foo.bar") };
            RestHttpClient = new RestHttpClient(HttpClient);
            Mediator = new Mock<IMediator>();
            
            SetupHttpClientGetToReturnAccountProviderLegalEntitiesResponse();
            
            ProviderRelationshipsApiClient = new ProviderRelationshipsApiClient(RestHttpClient, Mediator.Object);
        }

        public Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission()
        {
            GetAccountProviderLegalEntitiesWithPermissionRequest = new GetAccountProviderLegalEntitiesWithPermissionRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };
            
            return ProviderRelationshipsApiClient.GetAccountProviderLegalEntitiesWithPermission(GetAccountProviderLegalEntitiesWithPermissionRequest, CancellationToken);
        }

        public Task<bool> HasPermission()
        {
            HasPermissionRequest = new HasPermissionRequest
            {
                Ukprn = 11111111,
                AccountLegalEntityId = 1,
                Operation = Operation.CreateCohort
            };
            
            Mediator.Setup(m => m.Send(It.Is<HasPermissionQuery>(q => q.Ukprn == HasPermissionRequest.Ukprn && q.EmployerAccountLegalEntityId == HasPermissionRequest.AccountLegalEntityId && q.Operation == HasPermissionRequest.Operation), CancellationToken))
                .ReturnsAsync(() => Relationships.Any());
            
            return ProviderRelationshipsApiClient.HasPermission(HasPermissionRequest, CancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission()
        {
            GetAccountProviderLegalEntitiesWithPermissionRequest = new GetAccountProviderLegalEntitiesWithPermissionRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };

            HasRelationshipWithPermissionRequest = new HasRelationshipWithPermissionRequest
            {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };
            
            Mediator.Setup(m => m.Send(It.Is<HasRelationshipWithPermissionQuery>(q => q.Ukprn == HasRelationshipWithPermissionRequest.Ukprn && q.Operation == HasRelationshipWithPermissionRequest.Operation), CancellationToken))
                .ReturnsAsync(() => Relationships.Any());
            
            return ProviderRelationshipsApiClient.HasRelationshipWithPermission(HasRelationshipWithPermissionRequest, CancellationToken);
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
        
        public ProviderRelationshipsApiClientTestsFixture AddAccountProviderLegalEntitiesToAccountProviderLegalEntitiesResponse()
        {
            GetAccountProviderLegalEntitiesWithPermissionResponse.AccountProviderLegalEntities = new []
            {
                new AccountProviderLegalEntityDto(),
                new AccountProviderLegalEntityDto()
            };

            SetupHttpClientGetToReturnAccountProviderLegalEntitiesResponse();
            
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

        private void SetupHttpClientGetToReturnAccountProviderLegalEntitiesResponse()
        {
            var stringBody = JsonConvert.SerializeObject(GetAccountProviderLegalEntitiesWithPermissionResponse);
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage { Content = new StringContent(stringBody, Encoding.Default, "application/json") };
        }
    }
}