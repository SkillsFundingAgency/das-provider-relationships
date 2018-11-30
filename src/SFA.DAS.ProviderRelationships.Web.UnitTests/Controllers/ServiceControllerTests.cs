using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication.Oidc;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Controllers
{
    [TestFixture]
    [Parallelizable]
    public class ServiceControllerTests : FluentTest<ServiceControllerTestsFixture>
    {
        [Test]
        public void SignOut_WhenGettingSignOutAction_ThenShouldSignOutUser()
        {
            Run(f => f.SignOut(), (f, r) => f.AuthenticationService.Verify(s => s.SignOutUser(), Times.Once));
        }
        
        [Test]
        public void SignOut_WhenGettingSignOutAction_ThenShouldReturnRedirectActionResult()
        {
            Run(f => f.SignOut(), (f, r) => r.Should().NotBeNull().And.BeOfType<RedirectResult>().Which.Url.Should().Be(f.LogoutEndpoint));
        }
        
        [Test]
        public void SignOutCleanup_WhenGettingSignOutCleanupAction_ThenShouldSignOutUser()
        {
            Run(f => f.SignOutCleanup(), f => f.AuthenticationService.Verify(s => s.SignOutUser(), Times.Once));
        }
    }

    public class ServiceControllerTestsFixture
    {
        public Mock<IAuthenticationService> AuthenticationService { get; set; }
        public ServiceController ServiceController { get; set; }
        public string LogoutEndpoint { get; set; }
        public Mock<IAuthenticationUrls> AuthenticationUrls { get; set; }

        public ServiceControllerTestsFixture()
        {
            AuthenticationService = new Mock<IAuthenticationService>();
            LogoutEndpoint = "/logout";
            AuthenticationUrls = new Mock<IAuthenticationUrls>();

            AuthenticationUrls.Setup(u => u.LogoutEndpoint).Returns(LogoutEndpoint);
            
            ServiceController = new ServiceController(AuthenticationService.Object, AuthenticationUrls.Object);
        }

        public ActionResult SignOut()
        {
            return ServiceController.SignOut();
        }

        public void SignOutCleanup()
        {
            ServiceController.SignOutCleanup();
        }
    }
}