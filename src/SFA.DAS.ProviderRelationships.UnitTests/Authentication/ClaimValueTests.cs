using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    public class ClaimValueTests : FluentTest<ClaimValueTestsFixture>
    {
        [TestCase("http://das/employer/identity/claims/id", "http://das/employer/identity/claims/", "id")]
        [TestCase("A1", "A", "1")]
        public void WhenGettingIdClaimValue_ThenShouldReturnIdClaimValue(string expectedIdClaimValue, string claimsBaseUrl, string id)
        {
            Run(f =>
                {
                    f.SetClaimsBaseUrl(claimsBaseUrl);
                    f._mockClaimIdentifierConfiguration.Setup(c => c.Id).Returns(id);
                },
                f => f._claimValue.Id,
                (f, r) => r.Should().Be(expectedIdClaimValue));
        }

        [TestCase("http://das/employer/identity/claims/display_name", "http://das/employer/identity/claims/", "display_name")]
        [TestCase("A1", "A", "1")]
        public void WhenGettingDisplayNameClaimValue_ThenShouldReturnDisplayNameClaimValue(string expectedDisplayNameClaimValue, string claimsBaseUrl, string displayName)
        {
            Run(f =>
                {
                    f.SetClaimsBaseUrl(claimsBaseUrl);
                    f._mockClaimIdentifierConfiguration.Setup(c => c.DisplayName).Returns(displayName);
                },
                f => f._claimValue.DisplayName,
                (f, r) => r.Should().Be(expectedDisplayNameClaimValue));
        }

        [TestCase("http://das/employer/identity/claims/id", "http://das/employer/identity/claims/", "id")]
        [TestCase("A1", "A", "1")]
        public void WhenGettingEmailClaimValue_ThenShouldReturnEmailClaimValue(string expectedEmailClaimValue, string claimsBaseUrl, string email)
        {
            Run(f =>
                {
                    f.SetClaimsBaseUrl(claimsBaseUrl);
                    f._mockClaimIdentifierConfiguration.Setup(c => c.Email).Returns(email);
                },
                f => f._claimValue.Email,
                (f, r) => r.Should().Be(expectedEmailClaimValue));
        }
    }

    public class ClaimValueTestsFixture : FluentTestFixture
    {
        public readonly ClaimValue _claimValue;
        public readonly Mock<IClaimIdentifierConfiguration> _mockClaimIdentifierConfiguration;

        public ClaimValueTestsFixture()
        {
            _mockClaimIdentifierConfiguration = new Mock<IClaimIdentifierConfiguration>();
            _claimValue = new ClaimValue(_mockClaimIdentifierConfiguration.Object);
        }

        public void SetClaimsBaseUrl(string claimsBaseUrl)
        {
            _mockClaimIdentifierConfiguration.Setup(c => c.ClaimsBaseUrl).Returns(claimsBaseUrl);
        }
    }
}
