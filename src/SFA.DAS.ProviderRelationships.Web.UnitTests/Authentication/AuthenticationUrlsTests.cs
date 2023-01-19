using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class AuthenticationUrlsTests : FluentTest<AuthenticationUrlsTestsFixture>
    {
        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/authorize",
            "/connect/authorize", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingAuthorizeEndpoint_ThenShouldReturnCorrectAuthorizeEndpoint(string expectedEndpoint, string authorizeEndpoint, string baseAddress)
        {
            Test(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f.IdentityServerConfiguration.Setup(c => c.AuthorizeEndpoint).Returns(authorizeEndpoint);
                },
                f => f.AuthenticationUrls.AuthorizeEndpoint,
                (f, r) => r.Should().Be(expectedEndpoint));
        }

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/token",
            "/connect/token", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingTokenEndpoint_ThenShouldReturnCorrectTokenEndpoint(string expectedEndpoint, string tokenEndpoint, string baseAddress)
        {
            Test(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f.IdentityServerConfiguration.Setup(c => c.TokenEndpoint).Returns(tokenEndpoint);
                },
                f => f.AuthenticationUrls.TokenEndpoint,
                (f, r) => r.Should().Be(expectedEndpoint));
        }

        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/userinfo",
            "/connect/userinfo", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingUserInfoEndpoint_ThenShouldReturnCorrectUserInfoEndpoint(string expectedEndpoint, string userInfoEndpoint, string baseAddress)
        {
            Test(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f.IdentityServerConfiguration.Setup(c => c.UserInfoEndpoint).Returns(userInfoEndpoint);
                },
                f => f.AuthenticationUrls.UserInfoEndpoint,
                (f, r) => r.Should().Be(expectedEndpoint));
        }
        
        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/identity/connect/endsession?id_token_hint=abc123",
            "/connect/endsession?id_token_hint={0}", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity")]
        public void WhenGettingLogoutEndpoint_ThenShouldReturnCorrectLogoutEndpoint(string expectedEndpoint, string logoutEndpoint, string baseAddress)
        {
            Test(f =>
                {
                    f.SetBaseAddress(baseAddress);
                    f.SetCurrentUserClaim("id_token", "abc123");
                    f.IdentityServerConfiguration.Setup(c => c.LogoutEndpoint).Returns(logoutEndpoint);
                },
                f => f.AuthenticationUrls.LogoutEndpoint,
                (f, r) => r.Should().Be(expectedEndpoint));
        }
    }

    public class AuthenticationUrlsTestsFixture
    {
        public AuthenticationUrls AuthenticationUrls { get; set; }
        public Mock<IOidcConfiguration> IdentityServerConfiguration { get; set; }
        public Mock<IAuthenticationService> AuthenticationService { get; set; }

        public AuthenticationUrlsTestsFixture()
        {
            IdentityServerConfiguration = new Mock<IOidcConfiguration>();
            AuthenticationService = new Mock<IAuthenticationService>();
            AuthenticationUrls = new AuthenticationUrls(IdentityServerConfiguration.Object, AuthenticationService.Object);
        }

        public void SetBaseAddress(string baseAddress)
        {
            IdentityServerConfiguration.Setup(c => c.BaseAddress).Returns(baseAddress);
        }

        public void SetCurrentUserClaim(string key, string value)
        {
            AuthenticationService.Setup(s => s.GetCurrentUserClaimValue(key)).Returns(value);
        }
    }
}
