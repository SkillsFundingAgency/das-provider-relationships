using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    public class AuthenticationStartupTests : FluentTest<AuthenticationStartupTestsFixture>     // might have to update fluenttest to handle 2-phase construction of the testfixture for async construction
    {
        [Test]
        [Ignore("Unit testing AuthenticationStartup has a poor effort:benefit ratio. Test is here to document issues and options.")]
        public void WhenInitialising_ThenShouldSomething()
        {
            //Run(f =>);
        }
    }

    public class AuthenticationStartupTestsFixture : FluentTestFixture
    {
        private AuthenticationStartup _authenticationStartup;

        public AuthenticationStartupTestsFixture()
        {
            // Initialise uses extension methods off IAppBuilder, so to mock AuthenticationStartup to handle that, we could...
            // add a fake of Owin.CookieAuthenticationExtensions and shim UseCookieAuthentication, and fake SFA.DAS.OidcMiddleware.UseCodeFlowAuthentication and shim UseCodeFlowAuthentication
            // add a level of indirection to access IAppBuilder in AuthenticationStartup
            // create a new interface that inherits from IAppBuilder and adds the extension methods (would this work?)
            // not bother testing Initialise

            // also, PostAuthenticationAction uses an extension ClaimsIdentityExtensions.GetClaimValue

            // also, GetSigningCertificate news up a real X509Store (which implements no interface!)

            // also, other hoops to jump through

            // perhaps unit testing AuthenticationStartup has a poor effort:benefit ratio!

            var appBuilder = new Mock<IAppBuilder>();
            var identityServerConfiguration = new Mock<IIdentityServerConfiguration>();
            var logger = new Mock<ILog>();
            _authenticationStartup = new AuthenticationStartup(appBuilder.Object, identityServerConfiguration.Object, logger.Object);
        }
    }
}
