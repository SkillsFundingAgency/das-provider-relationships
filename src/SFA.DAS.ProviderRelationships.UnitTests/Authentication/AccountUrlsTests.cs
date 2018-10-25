using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    class AccountUrlsTests : FluentTest<AccountUrlsTestsFixture>
    {
        [TestCase("https://test2-eas.apprenticeships.sfa.bis.gov.uk/", "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=clientId&returnurl=",
          "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=clientId&returnurl=https%3a%2f%2ftest2-eas.apprenticeships.sfa.bis.gov.uk%2fservice%2fpassword%2fchange")]
        public void WhenGettingPasswordUrl_ThenShouldBeCorrect(string employerPortalBaseUrl, string changePasswordLink, string expectedChangePasswordUrl)
        {
            Run(f =>
            {
                f.Config.EmployerPortalBaseUrl = employerPortalBaseUrl;
                f.AuthenticationUrls.Setup(au => au.ChangePasswordUrl).Returns(changePasswordLink);
                // links are generated in AccountUrls's c'tor, so we need to new it here, rather than the Fixture's c'tor
                f.AccountUrls = new AccountUrls(f.Config, f.AuthenticationUrls.Object);
            },
                f => f.AccountUrls.ChangePasswordUrl,
                (f, r) => r.Should().Be(expectedChangePasswordUrl));
        }

        [TestCase("https://test2-eas.apprenticeships.sfa.bis.gov.uk/", "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changeemail?clientId=clientId&returnurl=",
            "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changeemail?clientId=clientId&returnurl=https%3a%2f%2ftest2-eas.apprenticeships.sfa.bis.gov.uk%2fservice%2femail%2fchange")]
        public void WhenGettingEmailUrl_ThenShouldBeCorrect(string employerPortalBaseUrl, string changeEmailLink, string expectedChangeEmailUrl)
        {
            Run(f =>
            {
                f.Config.EmployerPortalBaseUrl = employerPortalBaseUrl;
                f.AuthenticationUrls.Setup(au => au.ChangeEmailUrl).Returns(changeEmailLink);
                // links are generated in AccountUrls's c'tor, so we need to new it here, rather than the Fixture's c'tor
                f.AccountUrls = new AccountUrls(f.Config, f.AuthenticationUrls.Object);
            },
                f => f.AccountUrls.ChangeEmailUrl,
                (f, r) => r.Should().Be(expectedChangeEmailUrl));
        }
    }

    internal class AccountUrlsTestsFixture
    {
        internal AccountUrls AccountUrls;
        internal readonly ProviderRelationshipsConfiguration Config;
        internal readonly Mock<IAuthenticationUrls> AuthenticationUrls;

        public AccountUrlsTestsFixture()
        {
            Config = new ProviderRelationshipsConfiguration();
            AuthenticationUrls = new Mock<IAuthenticationUrls>();
        }
    }
}