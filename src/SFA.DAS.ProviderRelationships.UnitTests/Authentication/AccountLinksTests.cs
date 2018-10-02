using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    class AccountLinksTests : FluentTest<AccountLinksTestsFixture>
    {
        [TestCase("https://test2-eas.apprenticeships.sfa.bis.gov.uk/", "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=clientId&returnurl=",
          "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=clientId&returnurl=https%3a%2f%2ftest2-eas.apprenticeships.sfa.bis.gov.uk%2fservice%2fpassword%2fchange")]
        public void WhenGettingPasswordLink_ThenShouldBeCorrect(string employerPortalBaseUrl, string changePasswordLink, string expectedChangePasswordLink)
        {
            Run(f =>
            {
                f.Config.EmployerPortalBaseUrl = employerPortalBaseUrl;
                f.AuthenticationUrls.Setup(au => au.ChangePasswordLink).Returns(changePasswordLink);
                // links are generated in AccountLinks's c'tor, so we need to new it here, rather than the Fixture's c'tor
                f.AccountLinks = new AccountLinks(f.Config, f.AuthenticationUrls.Object);
            },
                f => f.AccountLinks.ChangePasswordLink,
                (f, r) => r.Should().Be(expectedChangePasswordLink));
        }

        [TestCase("https://test2-eas.apprenticeships.sfa.bis.gov.uk/", "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changeemail?clientId=clientId&returnurl=",
            "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changeemail?clientId=clientId&returnurl=https%3a%2f%2ftest2-eas.apprenticeships.sfa.bis.gov.uk%2fservice%2femail%2fchange")]
        public void WhenGettingEmailLink_ThenShouldBeCorrect(string employerPortalBaseUrl, string changeEmailLink, string expectedChangeEmailLink)
        {
            Run(f =>
            {
                f.Config.EmployerPortalBaseUrl = employerPortalBaseUrl;
                f.AuthenticationUrls.Setup(au => au.ChangeEmailLink).Returns(changeEmailLink);
                // links are generated in AccountLinks's c'tor, so we need to new it here, rather than the Fixture's c'tor
                f.AccountLinks = new AccountLinks(f.Config, f.AuthenticationUrls.Object);
            },
                f => f.AccountLinks.ChangeEmailLink,
                (f, r) => r.Should().Be(expectedChangeEmailLink));
        }
    }

    internal class AccountLinksTestsFixture
    {
        internal AccountLinks AccountLinks;
        internal readonly ProviderRelationshipsConfiguration Config;
        internal readonly Mock<IAuthenticationUrls> AuthenticationUrls;

        public AccountLinksTestsFixture()
        {
            Config = new ProviderRelationshipsConfiguration();
            AuthenticationUrls = new Mock<IAuthenticationUrls>();
        }
    }
}