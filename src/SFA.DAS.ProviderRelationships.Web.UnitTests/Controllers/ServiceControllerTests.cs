using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.GovUK.Auth.Services;
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

        public ServiceControllerTestsFixture()
        {
            ServiceController = new ServiceController(null, new StubAuthenticationService(null, null, null));
        }

        public Task<IActionResult> SignOut()
        {
            return ServiceController.SignOutEmployer();
        }
    }
}