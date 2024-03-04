using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.Models.Requests;
using SFA.DAS.ProviderRelationships.Api.Models.Responses;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.AccountProviders;

[TestFixture]
public class AccountProvidersControllerTests
{
    [Test, MoqAutoData]
    public async Task GetAccountProviders_InvalidAccountId_ShouldReturn_BadRequest(
        [NoAutoProperties] GetAccountProvidersQueryResult accountProvidersResponse,
        [Frozen] Mock<IMediator> mediatorMock,
        [NoAutoProperties] AccountProvidersController controller)
    {
        // Arrange
        long accountId = 0;
        // mediatorMock
        //     .Setup(m => m.Send(It.Is<GetAccountProvidersQuery>(c => c.AccountId == accountId)))
        //     .ReturnsAsync(accountProvidersResponse);

        // Act
        var result = await controller.Get(accountId, new CancellationToken());

        // Assert
        result.AssertModelError(nameof(accountId), "Account ID needs to be set");
    }


    [Test, MoqAutoData]
    public async Task GetAccountProviders_ValidAccountId_ShouldReturn_Results(
        long accountId,
        [NoAutoProperties] GetAccountProvidersQueryResult accountProvidersResponse,
        [Frozen] Mock<IMediator> mediatorMock,
        [NoAutoProperties] AccountProvidersController controller)
    {
        // Arrange
        var expected = new GetAccountProvidersResponse {
            AccountId = accountId,
            AccountProviders = accountProvidersResponse.AccountProviders
        };

        mediatorMock
            .Setup(m => m.Send(
                It.Is<GetAccountProvidersQuery>(c => c.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountProvidersResponse);

        // Act
        var result = await controller.Get(accountId, new CancellationToken());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var value = ((OkObjectResult)result).Value;
        value.Should().BeOfType<GetAccountProvidersResponse>();
        value.Should().BeEquivalentTo(expected);
    }

    [Test, MoqAutoData]
    public async Task Invitation_ValidData_ShouldReturn_OkResult(
        long accountId,
        AddAccountProviderFromInvitationPostRequest request,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProvidersController controller,
        long ukprn,
        long accountProviderId
        )
    {
        // Arrange
        var findResult = new FindProviderToAddQueryResult(ukprn, null);

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        mediatorMock.Setup(m => m.Send(It.IsAny<FindProviderToAddQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(findResult);

        mediatorMock.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(accountProviderId);

        var expectedResponse = new AddAccountProviderFromInvitationResponse { AccountProviderId = accountProviderId };

        // Act
        var result = await controller.Invitation(accountId, request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task Invitation_ProviderAlreadyAdded_ShouldReturn_OkResult(
        long accountId,
        AddAccountProviderFromInvitationPostRequest request,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProvidersController controller,
        long ukprn,
        long accountProviderId
        )
    {
        // Arrange
        var findResult = new FindProviderToAddQueryResult(ukprn, accountProviderId);

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        mediatorMock.Setup(m => m.Send(It.IsAny<FindProviderToAddQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(findResult);

        mediatorMock.Setup(m => m.Send(It.IsAny<AddAccountProviderCommand>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(accountProviderId);

        // Act
        var result = await controller.Invitation(accountId, request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Test, MoqAutoData]
    public async Task Invitation_ProviderNotFound_ShouldReturn_NotFoundResult(
       long accountId,
       AddAccountProviderFromInvitationPostRequest request,
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] AccountProvidersController controller,
       long accountProviderId
       )
    {
        // Arrange
        var findResult = new FindProviderToAddQueryResult(null, accountProviderId);

        mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), It.IsAny<CancellationToken>()))
             .Returns(Task.CompletedTask);

        mediatorMock.Setup(m => m.Send(It.IsAny<FindProviderToAddQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(findResult);

        // Act
        var result = await controller.Invitation(accountId, request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }


    [Test, MoqAutoData]
    public async Task Invitation_ProviderNotFound_ShouldReturn_BadResult(
       AddAccountProviderFromInvitationPostRequest request,
       [Greedy] AccountProvidersController controller
       )
    {       
        // Act
        var result = await controller.Invitation(0, request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}