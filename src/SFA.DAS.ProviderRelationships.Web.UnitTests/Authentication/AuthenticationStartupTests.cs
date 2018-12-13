using System.Security.Claims;
using Moq;
using NUnit.Framework;
using Owin;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class AuthenticationStartupTests : FluentTest<AuthenticationStartupTestsFixture> // might have to update fluenttest to handle 2-phase construction of the testfixture for async construction
    {
    }

    public class AuthenticationStartupTestsFixture
    {
        public const string IdType = "id", EmailAddressType = "email_address", DisplayNameType = "display_name";

        private readonly AuthenticationStartup _authenticationStartup;
        public ClaimsIdentity ClaimsIdentity;

        public AuthenticationStartupTestsFixture()
        {
            // Initialise uses extension methods off IAppBuilder, so to mock AuthenticationStartup to handle that, we could...
            // add a fake of Owin.CookieAuthenticationExtensions and shim UseCookieAuthentication, and fake SFA.DAS.OidcMiddleware.UseCodeFlowAuthentication and shim UseCodeFlowAuthentication
            // (fakes don't yet support .net standard out of the box. see, https://developercommunity.visualstudio.com/content/problem/162321/fakes-not-generated-using-net-471-after-updating-t.html)
            // add a level of indirection to access IAppBuilder in AuthenticationStartup
            // create a new interface that inherits from IAppBuilder and adds the extension methods (would this work?)
            // not bother testing Initialise

            // also, GetSigningCertificate news up a real X509Store (which implements no interface!)

            // also, other hoops to jump through

            // perhaps unit testing AuthenticationStartup has a poor effort:benefit ratio!

            var mockAppBuilder = new Mock<IAppBuilder>();
            var mockIdentityServerConfig = new Mock<IOidcConfiguration>();
            var mockAuthenticationUrls = new Mock<IAuthenticationUrls>();
            var mockPostAuthenticationHandler = new Mock<IPostAuthenticationHandler>();

            var mockConfigurationFactory = new Mock<ConfigurationFactory>();
            var logger = new Mock<ILog>();
            _authenticationStartup = new AuthenticationStartup(mockAppBuilder.Object, mockIdentityServerConfig.Object,
                mockAuthenticationUrls.Object, mockPostAuthenticationHandler.Object, mockConfigurationFactory.Object, logger.Object);
        }

        //public void AssertClaim(string claimType, string expectedValue)
        //{
        //    ClaimsIdentity.Claims.Should().ContainSingle(c => c.Type == claimType).Which.Value.Should().Be(expectedValue);
        //}
    }
}
