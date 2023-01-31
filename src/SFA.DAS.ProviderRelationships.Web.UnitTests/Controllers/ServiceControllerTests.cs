using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
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
        public void SignOut_WhenGettingSignOutAction_ThenShouldReturnSignOutResult()
        {
            TestAsync(
                f => f.SignOut(), 
                (f, r) => r.Should().NotBeNull().And.BeOfType<SignOutResult>().And.Match<SignOutResult>(result =>
                    result.Properties.Items.ContainsKey("id_token") &&
                    result.AuthenticationSchemes.Contains(CookieAuthenticationDefaults.AuthenticationScheme) &&
                    result.AuthenticationSchemes.Contains(OpenIdConnectDefaults.AuthenticationScheme)
                ));
        }
    }

    public class ServiceControllerTestsFixture
    {
        public ServiceController ServiceController { get; set; }
        public string LogoutEndpoint { get; set; }
        public Mock<IAuthenticationUrls> AuthenticationUrls { get; set; }

        public ServiceControllerTestsFixture()
        {
            LogoutEndpoint = "/logout";
            AuthenticationUrls = new Mock<IAuthenticationUrls>();

            AuthenticationUrls.Setup(u => u.LogoutEndpoint).Returns(LogoutEndpoint);
            
            ServiceController = new ServiceController();
        }

        public Task<IActionResult> SignOut()
        {
            return ServiceController.SignOutEmployer();
        }
    }
}