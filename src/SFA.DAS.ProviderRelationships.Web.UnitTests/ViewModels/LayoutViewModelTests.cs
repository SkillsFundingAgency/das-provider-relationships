using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.ViewModels;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.ViewModels
{
    [TestFixture]
    class LayoutViewModelTests : FluentTest<LayoutViewModelTestsFixture>
    {
        [TestCase("https://test2-eas.apprenticeships.sfa.bis.gov.uk/", "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=clientId&returnurl=",
          "https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=clientId&returnurl=https%3a%2f%2ftest2-eas.apprenticeships.sfa.bis.gov.uk%2fservice%2fpassword%2fchange")]
        public void WhenGettingPasswordLink_ThenShouldBeCorrect(string employerPortalBaseUrl, string changePasswordLink, string expectedChangePasswordLink)
        {
            Run(f =>
                {
                    f.Config.EmployerPortalBaseUrl = employerPortalBaseUrl;
                    f.AuthenticationUrls.Setup(au => au.ChangePasswordLink).Returns(changePasswordLink);
                    // Links are generated in LayoutViewModel's c'tor, so we need to new it here, rather than the Fixture's c'tor
                    f.LayoutViewModel = new LayoutViewModel(f.Config, f.AuthenticationUrls.Object);
                },
                f => f.LayoutViewModel.ChangePasswordLink,
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
                    // Links are generated in LayoutViewModel's c'tor, so we need to new it here, rather than the Fixture's c'tor
                    f.LayoutViewModel = new LayoutViewModel(f.Config, f.AuthenticationUrls.Object);
                },
                f => f.LayoutViewModel.ChangeEmailLink,
                (f, r) => r.Should().Be(expectedChangeEmailLink));
        }
    }

    public class LayoutViewModelTestsFixture
    {
        public LayoutViewModel LayoutViewModel;
        public readonly ProviderRelationshipsConfiguration Config;
        public readonly Mock<IAuthenticationUrls> AuthenticationUrls;

        public LayoutViewModelTestsFixture()
        {
            Config = new ProviderRelationshipsConfiguration();
            AuthenticationUrls = new Mock<IAuthenticationUrls>();
        }
    }
}