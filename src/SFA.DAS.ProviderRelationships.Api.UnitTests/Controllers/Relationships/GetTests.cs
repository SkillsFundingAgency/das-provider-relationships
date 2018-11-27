using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
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
        public Task WhenValidUkprnAndOperationIsSuppliedAndRelationshipExistsForProviderAndTheyHavePermissionForOperation_ThenShouldReturnCorrectRelationship()
        {
            return RunAsync(f => f.CallGet(), (f, r) => r.Should().NotBeNull());
        }

        //todo: distinguish between not founds (put error message in response body indicating what was not found)? return empty?
        //todo: in general, include error response in body, something like {ErrorCode: x, ErrorMessage: ""}
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndRelationshipExistsForProviderAndTheyDoNotHavePermissionForOperation_ThenShouldReturnNotFound()
        {
        }
        
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndRelationshipDoesNotExistsForProvider_ThenShouldReturnNotFound()
        {
        }
        
        [Test]
        public void WhenValidUkprnAndOperationIsSuppliedAndProviderDoesntExist_ThenShouldReturnNotFound()
        {
        }
        
        [Test]
        public void WhenUkprIsMissing_ThenShouldReturnNotImplemented()
        {
        }
        
        [Test]
        public void WhenOperationIsMissing_ThenShouldReturnNotImplemented()
        {
        }

        [Test]
        public void WhenUnknownOperationIsSupplied_ThenShouldReturnBadRequest()
        {
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

            Result = new GetRelationshipsWithPermissionQueryResult(new RelationshipDto[0]);
            
            Mediator.Setup(m => m.Send(It.Is<GetRelationshipsWithPermissionQuery>(q => q.Ukprn == GetRelationshipsParameters.Ukprn.Value && q.Operation == Operation.CreateCohort), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result);
            
            RelationshipsController = new RelationshipsController(Mediator.Object);
        }

        public async Task<IHttpActionResult> CallGet()
        {
            return await RelationshipsController.Get(GetRelationshipsParameters); //, CancellationToken.None);
        }
    }
}