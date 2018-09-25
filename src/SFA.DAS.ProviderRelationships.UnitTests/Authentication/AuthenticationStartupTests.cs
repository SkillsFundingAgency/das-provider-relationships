using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    public class AuthenticationStartupTests : FluentTest<AuthenticationStartupTestsFixture> // might have to update fluenttest to handle 2-phase construction of the testfixture for async construction
    {
        [Test]
        public void WhenPostAuthenticationActionIsCalled_ThenShouldAddClaims()
        {
            //using (ShimsContext.Create())
            //{
            //}

            Run(f =>
            {
                f._authenticationStartup.PostAuthenticationAction(f._claimsIdentity, f._mockClaimValue.Object);
            },
            f =>
            {
                f.AssertClaim(ClaimTypes.NameIdentifier, "id_value");
                f.AssertClaim(ClaimTypes.Name, "display_name_value");
                f.AssertClaim("sub", "id_value");
                f.AssertClaim("email", "email_address_value");
            });
        }
    }

    public class AuthenticationStartupTestsFixture : FluentTestFixture
    {
        public AuthenticationStartup _authenticationStartup;
        public ClaimsIdentity _claimsIdentity;
        public Mock<IClaimValue> _mockClaimValue;

        public AuthenticationStartupTestsFixture()
        {
            // Initialise uses extension methods off IAppBuilder, so to mock AuthenticationStartup to handle that, we could...
            // add a fake of Owin.CookieAuthenticationExtensions and shim UseCookieAuthentication, and fake SFA.DAS.OidcMiddleware.UseCodeFlowAuthentication and shim UseCodeFlowAuthentication
            // (fakes don't yet support .net standard out of the box. see, https://developercommunity.visualstudio.com/content/problem/162321/fakes-not-generated-using-net-471-after-updating-t.html)
            // add a level of indirection to access IAppBuilder in AuthenticationStartup
            // create a new interface that inherits from IAppBuilder and adds the extension methods (would this work?)
            // not bother testing Initialise

            // also, PostAuthenticationAction uses an extension ClaimsIdentityExtensions.GetClaimValue

            // also, GetSigningCertificate news up a real X509Store (which implements no interface!)

            // also, other hoops to jump through

            // perhaps unit testing AuthenticationStartup has a poor effort:benefit ratio!

            var appBuilder = new Mock<IAppBuilder>();
            var identityServerConfiguration = new Mock<IIdentityServerConfiguration>();
            var authenticationUrls = new Mock<IAuthenticationUrls>();
           _mockClaimValue = new Mock<IClaimValue>();
            var logger = new Mock<ILog>();
            _authenticationStartup = new AuthenticationStartup(appBuilder.Object, identityServerConfiguration.Object,
                authenticationUrls.Object, _mockClaimValue.Object, logger.Object);

            _claimsIdentity = new ClaimsIdentity(new [] {new Claim("id", "id_value"), new Claim("email_address", "email_address_value"), new Claim("display_name", "display_name_value") });
            _mockClaimValue.Setup(cv => cv.Id).Returns("id");
            _mockClaimValue.Setup(cv => cv.Email).Returns("email_address");
            _mockClaimValue.Setup(cv => cv.DisplayName).Returns("display_name");
        }

        public void AssertClaim(string claimType, string expectedValue)
        {
            _claimsIdentity.Claims.Should().ContainSingle(c => c.Type == claimType).Which.Value.Should().Be(expectedValue);
        }
    }
}
