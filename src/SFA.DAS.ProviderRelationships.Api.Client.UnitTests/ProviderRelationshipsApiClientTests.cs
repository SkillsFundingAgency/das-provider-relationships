using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Http;
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
            return RunAsync(f => f.AddRelationships(false), f => f.HasPermission(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task HasPermission_WhenPermissionIsGranted_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.AddRelationships(true), f => f.HasPermission(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task HasRelationshipWithPermission_WhenRelationshipsDoNotExist_ThenShouldReturnFalse()
        {
            return RunAsync(f => f.AddRelationships(false), f => f.HasRelationshipWithPermission(), (f, r) => r.Should().BeFalse());
        }

        [Test]
        public Task HasRelationshipWithPermission_WhenRelationshipsExist_ThenShouldReturnTrue()
        {
            return RunAsync(f => f.AddRelationships(true), f => f.HasRelationshipWithPermission(), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public Task Ping_WhenHttpPingFails_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetHttpPingFailure(), f => f.Ping(), (f, r) => r.Should().Throw<RestHttpClientException>());
        }

        [Test]
        public Task Ping_WhenReadStorePingFails_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetHttpPermissionPingFailure(), f => f.Ping(), (f, r) => r.Should().Throw<RestHttpClientException>());
        }

        [Test]
        public Task RevokePermissions_ShouldExecuteHttpRequest()
             => RunAsync(
                 act: async f =>
                 {
                     f.RevokePermissionsRequest = new RevokePermissionsRequest(
                         ukprn: 299792458,
                         accountLegalEntityPublicHashedId: "DEADBEEF",
                         operationsToRevoke: new[] { Operation.Recruitment });

                     await f.ProviderRelationshipsApiClient.RevokePermissions(f.RevokePermissionsRequest);
                 },
                 assert: f =>
                 {
                     var req = f.HttpMessageHandler.HttpRequestMessage;
                     req.RequestUri.ToString().Should().Be("https://foo.bar/permissions/revoke");

                     var objectContent = (ObjectContent)req.Content;
                     objectContent.Value.Should().BeSameAs(f.RevokePermissionsRequest);
                 });
    }

    public class ProviderRelationshipsApiClientTestsFixture
    {
        public HasPermissionRequest HasPermissionRequest { get; set; }
        public GetAccountProviderLegalEntitiesWithPermissionRequest GetAccountProviderLegalEntitiesWithPermissionRequest { get; set; }
        public HasRelationshipWithPermissionRequest HasRelationshipWithPermissionRequest { get; set; }
        public RevokePermissionsRequest RevokePermissionsRequest { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IProviderRelationshipsApiClient ProviderRelationshipsApiClient { get; set; }
        public HttpClient HttpClient { get; set; }
        public IRestHttpClient RestHttpClient { get; set; }
        public FakeHttpMessageHandler HttpMessageHandler { get; set; }
        public GetAccountProviderLegalEntitiesWithPermissionResponse GetAccountProviderLegalEntitiesWithPermissionResponse { get; set; }
        public List<AccountProviderLegalEntityDto> Relationships { get; set; }

        public ProviderRelationshipsApiClientTestsFixture()
        {
            GetAccountProviderLegalEntitiesWithPermissionResponse = new GetAccountProviderLegalEntitiesWithPermissionResponse { AccountProviderLegalEntities = new List<AccountProviderLegalEntityDto>() };
            Relationships = new List<AccountProviderLegalEntityDto>();
            CancellationToken = CancellationToken.None;
            HttpMessageHandler = new FakeHttpMessageHandler();
            HttpClient = new HttpClient(HttpMessageHandler) { BaseAddress = new Uri("https://foo.bar") };
            RestHttpClient = new RestHttpClient(HttpClient);

            SetupHttpClientGetToReturnAccountProviderLegalEntitiesResponse();

            ProviderRelationshipsApiClient = new ProviderRelationshipsApiClient(RestHttpClient);
        }

        public Task<GetAccountProviderLegalEntitiesWithPermissionResponse> GetAccountProviderLegalEntitiesWithPermission()
        {
            GetAccountProviderLegalEntitiesWithPermissionRequest = new GetAccountProviderLegalEntitiesWithPermissionRequest {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort,
                AccountLegalEntityPublicHashedId = "ABC123",
                AccountHashedId = "XYZ789"
            };

            return ProviderRelationshipsApiClient.GetAccountProviderLegalEntitiesWithPermission(GetAccountProviderLegalEntitiesWithPermissionRequest, CancellationToken);
        }

        public Task<bool> HasPermission()
        {
            HasPermissionRequest = new HasPermissionRequest {
                Ukprn = 11111111,
                AccountLegalEntityId = 1,
                Operation = Operation.CreateCohort
            };

            return ProviderRelationshipsApiClient.HasPermission(HasPermissionRequest, CancellationToken);
        }

        public Task<bool> HasRelationshipWithPermission()
        {
            HasRelationshipWithPermissionRequest = new HasRelationshipWithPermissionRequest {
                Ukprn = 11111111,
                Operation = Operation.CreateCohort
            };

            return ProviderRelationshipsApiClient.HasRelationshipWithPermission(HasRelationshipWithPermissionRequest, CancellationToken);
        }

        public Task Ping()
        {
            return ProviderRelationshipsApiClient.Ping(CancellationToken);
        }

        public ProviderRelationshipsApiClientTestsFixture AddRelationships(bool hasRelationships)
        {
            SetupHttpClientGetToReturnBoolResponse(hasRelationships);

            return this;
        }

        public ProviderRelationshipsApiClientTestsFixture AddAccountProviderLegalEntitiesToAccountProviderLegalEntitiesResponse()
        {
            GetAccountProviderLegalEntitiesWithPermissionResponse.AccountProviderLegalEntities = new[]
            {
                new AccountProviderLegalEntityDto(),
                new AccountProviderLegalEntityDto()
            };

            SetupHttpClientGetToReturnAccountProviderLegalEntitiesResponse();

            return this;
        }

        public ProviderRelationshipsApiClientTestsFixture SetHttpPermissionPingFailure()
        {
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                Content = new StringContent("Kaboom"),
                ReasonPhrase = "Internal server error",
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://foo.bar/ping")
            };

            return this;
        }

        public ProviderRelationshipsApiClientTestsFixture SetHttpPingFailure()
        {
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                Content = new StringContent("Kaboom"),
                ReasonPhrase = "Internal server error",
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://foo.bar/ping")
            };

            return this;
        }

        private void SetupHttpClientGetToReturnAccountProviderLegalEntitiesResponse()
        {
            var stringBody = JsonConvert.SerializeObject(GetAccountProviderLegalEntitiesWithPermissionResponse);
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage { Content = new StringContent(stringBody, System.Text.Encoding.Default, "application/json") };
        }

        private void SetupHttpClientGetToReturnBoolResponse(bool response)
        {
            var stringBody = JsonConvert.SerializeObject(response ? 1 : 0);
            HttpMessageHandler.HttpResponseMessage = new HttpResponseMessage { Content = new StringContent(stringBody, System.Text.Encoding.Default, "application/json") };
        }
    }
}