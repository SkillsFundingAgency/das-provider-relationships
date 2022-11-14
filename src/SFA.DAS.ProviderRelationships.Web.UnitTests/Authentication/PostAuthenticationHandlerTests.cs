using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.UnitOfWork.DependencyResolution.StructureMap;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class PostAuthenticationHandlerTests : FluentTest<PostAuthenticationHandlerTestsFixture>
    {
        [Test]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_AndNotUsingGovUkSignIn_ThenShouldSendCreateOrUpdateUserCommand()
        {
            await RunAsync(f => f.Handle(), 
                f => f.Mediator.Verify(m => m.Send(It.Is<CreateOrUpdateUserCommand>(c => 
                c.Ref == f.Ref && c.Email == f.Email && c.FirstName == f.FirstName && c.LastName == f.LastName), CancellationToken.None), 
                Times.Once));
        }
        
        [Test]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_AndNotUsingGovUkSignIn_ThenShouldNotCallOuterApi()
        {
            await RunAsync(f => f.Handle(), 
                f => f.MockOuterApiClient.Verify(m => m.Get<GetUserAccountsResponse>(It.IsAny<GetEmployerAccountRequest>()), 
                    Times.Never));
        }
        
        [Test, MoqAutoData]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_AndIsUsingGovUkSignIn_ThenShouldCallOuterApi(
            string userId,
            string email,
            GetUserAccountsResponse apiResponse,
            [Frozen] Mock<IMediator> mockMediator,
            [Frozen] ProviderRelationshipsConfiguration config, 
            [Frozen] Mock<IOuterApiClient> mockOuterApiClient,
            PostAuthenticationHandler handler)
        {
            //arrange
            var identity = new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            });
            config.UseGovUkSignIn = true;
            var expectedRequest = new GetEmployerAccountRequest(userId, email);
            
            //act
            await handler.Handle(identity);
            
            //assert
            mockMediator.Verify(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), CancellationToken.None),
                Times.Never);
            mockOuterApiClient.Verify(m => m.Get<GetUserAccountsResponse>(It.Is<GetEmployerAccountRequest>(request =>
                    request.GetUrl == expectedRequest.GetUrl)),
                Times.Once);
        }
        
        [Test]
        public async Task Handle_WhenHandlingPostAuthenticationIdentity_AndIsUsingGovUkSignIn_ThenShouldNotSendCreateOrUpdateUserCommand()
        {
            await RunAsync(f => f.Config.UseGovUkSignIn = true,
                f => f.HandleGovUk(), 
                f => f.Mediator.Verify(m => m.Send(It.IsAny<CreateOrUpdateUserCommand>(), CancellationToken.None), 
                    Times.Never));
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
        public ClaimsIdentity GovUkClaimsIdentity { get; set; }
        public IPostAuthenticationHandler Handler { get; set; }
        public Mock<IUnitOfWorkScope> UnitOfWorkScope { get; set; }
        public Mock<IContainer> Container { get; set; }
        public Mock<IOuterApiClient> MockOuterApiClient { get; set; }
        public ProviderRelationshipsConfiguration Config { get; set; }
        
        public PostAuthenticationHandlerTestsFixture()
        {
            Ref = Guid.NewGuid();
            Email = "foo@bar.com";
            FirstName = "Foo";
            LastName = "Bar";
            Mediator = new Mock<IMediator>();
            
            ClaimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(DasClaimTypes.Id, Ref.ToString()),
                new Claim(DasClaimTypes.Email, Email),
                new Claim(DasClaimTypes.GivenName, FirstName),
                new Claim(DasClaimTypes.FamilyName, LastName),
            });
            
            GovUkClaimsIdentity = new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, Ref.ToString()),
                new Claim(ClaimTypes.Email, Email),
            });

            UnitOfWorkScope = new Mock<IUnitOfWorkScope>();
            Container = new Mock<IContainer>();
            MockOuterApiClient = new Mock<IOuterApiClient>();
            Config = new ProviderRelationshipsConfiguration();
            
            UnitOfWorkScope.Setup(s => s.RunAsync(It.IsAny<Func<IContainer, Task>>())).Returns(Task.CompletedTask).Callback<Func<IContainer, Task>>(o => o(Container.Object));
            Container.Setup(c => c.GetInstance<IMediator>()).Returns(Mediator.Object);

            Handler = new PostAuthenticationHandler(UnitOfWorkScope.Object, MockOuterApiClient.Object, Config);
        }

        public async Task Handle()
        {
            await Handler.Handle(ClaimsIdentity);
        }
        
        public async Task HandleGovUk()
        {
            await Handler.Handle(GovUkClaimsIdentity);
        }
    }
}