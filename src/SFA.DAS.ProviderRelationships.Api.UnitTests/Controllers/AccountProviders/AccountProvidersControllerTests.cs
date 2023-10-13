using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.UnitTests.Extensions;
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
        var expected = new GetAccountProvidersResponse 
        {
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
}