using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.Invitations
{
    [TestFixture]
    [Parallelizable]
    public class GetTests : FluentTest<GetTestsFixture>
    {
        [Test]
        public Task WhenValidCorrelationIdIsSupplied_ThenShouldReturnInvitationFromQuery()
        {
            return RunAsync(f => f.CallGet(), 
                (f, r) =>
                {
                    r.Should().NotBeNull();
                    r.Should().BeOfType<OkNegotiatedContentResult<InvitationDto>>();
                    ((OkNegotiatedContentResult<InvitationDto>) r).Content.Should().BeEquivalentTo(f.Result.Invitation);
                });
        }

        [Test]
        public Task WhenCorrelationIdIsInvalid_ThenShouldReturnBadRequest()
        {
            return RunAsync(f => f.SetCorrelationId("INVALID"), f => f.CallGet(),
                (f, r) => r.AssertSingleModelError("correlationId", "An invalid correlation id was supplied"));
        }
    }

    public class GetTestsFixture
    {
        public string CorrelationId { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public InvitationsController InvitationsController { get; set; }
        public GetInvitationByIdQueryResult Result { get; set; }

        public GetTestsFixture()
        {
            CorrelationId = Guid.NewGuid().ToString();

            Mediator = new Mock<IMediator>();

            Result = new GetInvitationByIdQueryResult(
                new InvitationDto());
            
            Mediator.Setup(m => m.Send(It.Is<GetInvitationByIdQuery>(q => q.CorrelationId == Guid.Parse(CorrelationId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result);
            
            InvitationsController = new InvitationsController(Mediator.Object);
        }

        public async Task<IHttpActionResult> CallGet()
        {
            return await InvitationsController.Get(CorrelationId, CancellationToken.None);
        }

        public GetTestsFixture SetCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }
    }
}