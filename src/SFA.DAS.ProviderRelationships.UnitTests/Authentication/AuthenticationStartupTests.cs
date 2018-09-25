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
            const string idType = "id", emailAddressType = "email_address", displayNameType = "display_name";
            const string idValue = "id_value", emailAddressValue = "email_address_value", displayNameValue = "display_name_value";

            Run(f =>
                {
                    f.ClaimsIdentity = new ClaimsIdentity(new[]
                    {
                        new Claim(idType, idValue),
                        new Claim(emailAddressType, emailAddressValue),
                        new Claim(displayNameType, displayNameValue)
                    });
                    f.MockClaimValue.Setup(cv => cv.Id).Returns(idType);
                    f.MockClaimValue.Setup(cv => cv.Email).Returns(emailAddressType);
                    f.MockClaimValue.Setup(cv => cv.DisplayName).Returns(displayNameType);
                },
                f => f.CallPostAuthenticationAction(),
            f =>
            {
                f.AssertClaim(ClaimTypes.NameIdentifier, idValue);
                f.AssertClaim(ClaimTypes.Name, displayNameValue);
                f.AssertClaim("sub", idValue);
                f.AssertClaim("email", emailAddressValue);
            });
        }
    }

    public class AuthenticationStartupTestsFixture : FluentTestFixture
    {
        private readonly AuthenticationStartup _authenticationStartup;
        public ClaimsIdentity ClaimsIdentity;
        public readonly Mock<IClaimValue> MockClaimValue;

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

            var appBuilder = new Mock<IAppBuilder>();
            var identityServerConfiguration = new Mock<IIdentityServerConfiguration>();
            var authenticationUrls = new Mock<IAuthenticationUrls>();
           MockClaimValue = new Mock<IClaimValue>();
            var logger = new Mock<ILog>();
            _authenticationStartup = new AuthenticationStartup(appBuilder.Object, identityServerConfiguration.Object,
                authenticationUrls.Object, MockClaimValue.Object, logger.Object);
        }

        public void CallPostAuthenticationAction()
        {
            _authenticationStartup.PostAuthenticationAction(ClaimsIdentity, MockClaimValue.Object);
        }

        public void AssertClaim(string claimType, string expectedValue)
        {
            ClaimsIdentity.Claims.Should().ContainSingle(c => c.Type == claimType).Which.Value.Should().Be(expectedValue);
        }
    }
}
