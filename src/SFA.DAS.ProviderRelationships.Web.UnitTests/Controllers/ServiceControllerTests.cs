using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Web.Authentication;
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
            Run(f => f.SignOut(), 
                (f, r) => f.AuthenticationService.Verify(s => s.SignOutUser(), Times.Once));
        }
        
        [Test]
        public void SignOut_WhenGettingSignOutAction_ThenShouldReturnRedirectActionResult()
        {
            Run(f => f.SignOut(), 
                (f, r) => r.Should().NotBeNull().And.BeOfType<RedirectResult>().Which.Url.Should().Be(f.LogoutEndpoint));
        }
        
        [Test]
        public void SignOutCleanup_WhenGettingSignOutCleanupAction_ThenShouldSignOutUser()
        {
            Run(f => f.SignOutCleanup(), 
                f => f.AuthenticationService.Verify(s => s.SignOutUser(), Times.Once));
        }
    }

    public class ServiceControllerTestsFixture
    {
        private const string OwinEnvironmentKey = "owin.Environment";
        const string RequestHeaders = "owin.RequestHeaders";
        const string ResponseHeaders = "owin.ResponseHeaders";
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
            ServiceController.ControllerContext = new ControllerContext();
            ServiceController.ControllerContext.HttpContext = new HttpContextWrapper( 
                new HttpContext(
                    new HttpRequest(string.Empty, "https://localhost:123", string.Empty),
                    new HttpResponse(new StringWriter())));
            var owinEnvironment = new Dictionary<string, object> {
                {RequestHeaders, new Dictionary<string, string[]>()},
                {ResponseHeaders, new Dictionary<string, string[]>()}
            };
            ServiceController.ControllerContext.HttpContext.Items.Add(OwinEnvironmentKey, owinEnvironment);
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