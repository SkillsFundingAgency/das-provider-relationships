using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class AuthenticationUrlsTests : FluentTest<AuthenticationUrlsTestsFixture>
    {
        #region Endpoints

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/authorize",
            "/connect/authorize", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingAuthorizeEndpoint_ThenShouldReturnCorrectAuthorizeEndpoint(string expectedAuthorizeEndpoint, string authorizeEndPoint, string baseAddress)
        {
            Run(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f._mockIdentityServerConfig.Setup(c => c.AuthorizeEndPoint).Returns(authorizeEndPoint);
                },
                f => f.AuthenticationUrls.AuthorizeEndpoint,
                (f, r) => r.Should().Be(expectedAuthorizeEndpoint));
        }

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/token",
            "/connect/token", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingTokenEndpoint_ThenShouldReturnCorrectTokenEndpoint(string expectedEndpoint, string tokenEndpoint, string baseAddress)
        {
            Run(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f._mockIdentityServerConfig.Setup(c => c.TokenEndpoint).Returns(tokenEndpoint);
                },
                f => f.AuthenticationUrls.TokenEndpoint,
                (f, r) => r.Should().Be(expectedEndpoint));
        }

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/userinfo",
            "/connect/userinfo", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingUserInfoEndpoint_ThenShouldReturnCorrectUserInfoEndpoint(string expectedEndpoint, string userInfoEndpoint, string baseAddress)
        {
            Run(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f._mockIdentityServerConfig.Setup(c => c.UserInfoEndpoint).Returns(userInfoEndpoint);
                },
                f => f.AuthenticationUrls.UserInfoEndpoint,
                (f, r) => r.Should().Be(expectedEndpoint));
        }

        #endregion Endpoints

        #region ChangeUrls

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changepassword?clientId=devprorel&returnurl=",
            "/account/changepassword?clientId={0}&returnurl=", "devprorel", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingChangePasswordUrl_ThenShouldReturnCorrectChangePasswordUrl(string expectedUrl, string changePasswordUrl, string clientId, string baseAddress)
        {
            Run(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f._mockIdentityServerConfig.Setup(c => c.ClientId).Returns(clientId);
                    f._mockIdentityServerConfig.Setup(c => c.ChangePasswordUrl).Returns(changePasswordUrl);
                },
                f => f.AuthenticationUrls.ChangePasswordUrl,
                (f, r) => r.Should().Be(expectedUrl));
        }

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/account/changeemail?clientId=devprorel&returnurl=",
            "/account/changeemail?clientId={0}&returnurl=", "devprorel", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingChangeEmailUrl_ThenShouldReturnChangeEmailUrl(string expectedUrl, string changeEmailUrl, string clientId, string baseAddress)
        {
            Run(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f._mockIdentityServerConfig.Setup(c => c.ClientId).Returns(clientId);
                    f._mockIdentityServerConfig.Setup(c => c.ChangeEmailUrl).Returns(changeEmailUrl);
                },
                f => f.AuthenticationUrls.ChangeEmailUrl,
                (f, r) => r.Should().Be(expectedUrl));
        }

        #endregion ChangeUrls
    }

    public class AuthenticationUrlsTestsFixture
    {
        public readonly AuthenticationUrls AuthenticationUrls;
        public readonly Mock<IIdentityServerConfiguration> _mockIdentityServerConfig;

        public AuthenticationUrlsTestsFixture()
        {
            _mockIdentityServerConfig = new Mock<IIdentityServerConfiguration>();
            AuthenticationUrls = new AuthenticationUrls(_mockIdentityServerConfig.Object);
        }

        public void SetBaseAddress(string baseAddress)
        {
            _mockIdentityServerConfig.Setup(c => c.BaseAddress).Returns(baseAddress);
        }
    }
}
