using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;1
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class PostAuthenticationHandlerTests : FluentTest<PostAuthenticationHandlerTestsFixture>
    {
        [Test]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_ThenShouldSendCreateOrUpdateUserCommand()
        {
            await TestAsync(f => f.Handle(), f => f.Mediator.Verify(m => m.Send(It.Is<CreateOrUpdateUserCommand>(c => 
                c.Ref == f.Ref && c.Email == f.Email && c.FirstName == f.FirstName && c.LastName == f.LastName), CancellationToken.None), Times.Once));
        }
        
        [Test]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_AndNotUsingGovUkSignIn_ThenShouldNotCallOuterApi()
        {
            await TestAsync(f => f.Handle(), 
                f => f.MockOuterApiClient.Verify(m => m.Get<GetUserAccountsResponse>(It.IsAny<GetEmployerAccountRequest>()), 
                    Times.Never));
        }

        [Test, MoqAutoData]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_AndIsUsingGovUkSignIn_ThenShouldCallOuterApi(
            string govUkUserId,
            string email,
            Guid userId,
            GetUserAccountsResponse apiResponse,
            [Frozen] Mock<IOptions<ProviderRelationshipsConfiguration>> mockConfigOptions, 
            [Frozen] Mock<IOuterApiClient> mockOuterApiClient,
            [Frozen] Mock<IServiceProvider> mockContainer,
            [Frozen] Mock<IUnitOfWorkScope> mockUnitOfWork,
            [Frozen] Mock<IMediator> mockMediator,
            PostAuthenticationHandler handler)
        {
            //arrange
            var identity = new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, govUkUserId),
                new Claim(ClaimTypes.Email, email)
            });
            var expectedRequest = new GetEmployerAccountRequest(govUkUserId, email);
            apiResponse.EmployerUserId = userId.ToString();
            var accountsAsJson = JsonConvert.SerializeObject(apiResponse.UserAccounts.ToDictionary(k => k.AccountId));
            mockOuterApiClient
                .Setup(client => client.Get<GetUserAccountsResponse>(It.Is<GetEmployerAccountRequest>(request =>
                    request.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);
            mockUnitOfWork
                .Setup(scope => scope.RunAsync(It.IsAny<Func<IServiceProvider, Task>>()))
                .Returns(Task.CompletedTask)
                .Callback<Func<IServiceProvider, Task>>(o => o(mockContainer.Object));
            mockConfigOptions
                .Setup(options => options.Value)
                .Returns(new ProviderRelationshipsConfiguration() { UseGovUkSignIn = true });

            //act
            await handler.Handle(identity);

            //assert
            mockOuterApiClient.Verify(m => m.Get<GetUserAccountsResponse>(It.Is<GetEmployerAccountRequest>(request =>
                    request.GetUrl == expectedRequest.GetUrl)),
                Times.Once);
            identity.Claims.Should().Contain(claim =>
                claim.Type == EmployerClaimTypes.AssociatedAccounts &&
                claim.ValueType == JsonClaimValueTypes.Json &&
                claim.Value == accountsAsJson);
            identity.Claims.Should().Contain(claim =>
                claim.Type == EmployerClaimTypes.UserId &&
                claim.Value == apiResponse.EmployerUserId);
            identity.Claims.Should().Contain(claim =>
                claim.Type == EmployerClaimTypes.EmailAddress &&
                claim.Value == email);
            identity.Claims.Should().Contain(claim =>
                claim.Type == EmployerClaimTypes.GivenName &&
                claim.Value == apiResponse.FirstName);
            identity.Claims.Should().Contain(claim =>
                claim.Type == EmployerClaimTypes.FamilyName &&
                claim.Value == apiResponse.LastName);
            mockMediator.Verify(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), CancellationToken.None),
                Times.Once);
        }
    }

    public class PostAuthenticationHandlerTestsFixture
    {
        public Guid Ref { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public IPostAuthenticationHandler Handler { get; set; }
        public Mock<IUnitOfWorkScope> UnitOfWorkScope { get; set; }
        public Mock<IServiceProvider> Container { get; set; }
        public Mock<IOuterApiClient> MockOuterApiClient { get; set; }
        public Mock<IOptions<ProviderRelationshipsConfiguration>> MockConfigOptions { get; set; }
        
        public PostAuthenticationHandlerTestsFixture()
        {
            Ref = Guid.NewGuid();
            Email = "foo@bar.com";
            FirstName = "Foo";
            LastName = "Bar";
            Mediator = new Mock<IMediator>();
            
            ClaimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(EmployerClaimTypes.UserId, Ref.ToString()),
                new Claim(EmployerClaimTypes.EmailAddress, Email),
                new Claim(EmployerClaimTypes.GivenName, FirstName),
                new Claim(EmployerClaimTypes.FamilyName, LastName),
            });

            UnitOfWorkScope = new Mock<IUnitOfWorkScope>();
            Container = new Mock<IServiceProvider>();
            MockOuterApiClient = new Mock<IOuterApiClient>();
            MockConfigOptions = new Mock<IOptions<ProviderRelationshipsConfiguration>>();
            MockConfigOptions
                .Setup(options => options.Value)
                .Returns(new ProviderRelationshipsConfiguration() { UseGovUkSignIn = false });
            
            UnitOfWorkScope.Setup(s => s.RunAsync(It.IsAny<Func<IServiceProvider, Task>>())).Returns(Task.CompletedTask).Callback<Func<IServiceProvider, Task>>(o => o(Container.Object));

            Handler = new PostAuthenticationHandler(MockOuterApiClient.Object, Mediator.Object, MockConfigOptions.Object, Mock.Of<IConfiguration>());
        }

        public async Task Handle()
        {
            await Handler.Handle(ClaimsIdentity);
        }
    }
}