using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.ActionParameters.Relationships;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.Relationships
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public Task WhenValidUkprnAndOperationIsSupplied_ThenShouldReturnRelationshipsFromQuery()
        {
            return RunAsync(f => f.CallGet(), 
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<RelationshipsResponse>>();
                    ((OkNegotiatedContentResult<RelationshipsResponse>)r).Content.Should().BeEquivalentTo(f.Result);
                });
        }

        [Test]
        public Task WhenUkprIsMissing_ThenShouldReturnNotImplemented()
        {
            return RunAsync(f => f.SetUkprn(null), f => f.CallGet(),
                (f, r) => r.Should().Throw<HttpResponseException>().Where(e => e.Response.StatusCode == HttpStatusCode.NotImplemented));
        }
        
        [Test]
        public Task WhenOperationIsMissing_ThenShouldReturnNotImplemented()
        {
            return RunAsync(f => f.SetOperation(null), f => f.CallGet(),
                (f, r) => r.Should().Throw<HttpResponseException>().Where(e => e.Response.StatusCode == HttpStatusCode.NotImplemented));
        }

        [Test]
        public Task WhenUnknownOperationIsSupplied_ThenShouldReturnBadRequest()
        {
            return RunAsync(f => f.SetOperation("Vasectomy"), f => f.CallGet(),
                                (f, r) => r.Should().NotBeNull().And.BeOfType<BadRequestResult>());
        }
    }

    public class GetTestsFixture
    {
        public GetRelationshipsParameters GetRelationshipsParameters { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public RelationshipsController RelationshipsController { get; set; }
        public GetRelationshipsWithPermissionQueryResult Result { get; set; }

        public GetTestsFixture()
        {
            GetRelationshipsParameters = new GetRelationshipsParameters
            {
                Ukprn = 12345678L,
                Operation = "CreateCohort"
            };

            Mediator = new Mock<IMediator>();

            Result = new GetRelationshipsWithPermissionQueryResult(new [] {
                new RelationshipDto {AccountId = 41L, AccountLegalEntityId = 4131L, AccountLegalEntityName = "AccountLegalEntityName", AccountLegalEntityPublicHashedId = "ALEPHI", AccountName = "AccountName", AccountProviderId = 491L, AccountPublicHashedId = "ACCPHI" }
            });
            
            Mediator.Setup(m => m.Send(It.Is<GetRelationshipsWithPermissionQuery>(q => q.Ukprn == GetRelationshipsParameters.Ukprn.Value && q.Operation == Operation.CreateCohort), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result);
            
            RelationshipsController = new RelationshipsController(Mediator.Object);
        }

        public async Task<IHttpActionResult> CallGet()
        {
            return await RelationshipsController.Get(GetRelationshipsParameters); //, CancellationToken.None);
        }

        public GetTestsFixture SetUkprn(long? ukprn)
        {
            GetRelationshipsParameters.Ukprn = ukprn;
            return this;
        }
        
        public GetTestsFixture SetOperation(string operation)
        {
            GetRelationshipsParameters.Operation = operation;
            return this;
        }
    }
}