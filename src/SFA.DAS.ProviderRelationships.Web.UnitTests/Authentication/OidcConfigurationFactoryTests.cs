using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authentication
{
    [TestFixture]
    [Parallelizable]
    public class OidcConfigurationFactoryTests : FluentTest<IdentityServerConfigurationFactoryTestsFixture>
    {
        [TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/account/confirm", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity", "/account/confirm")]
        [TestCase("https://example.com/account/confirm", "https://example.com", "/account/confirm")]
        public void WhenGettingConfigurationContext_ThenShouldReturnConfigurationContextWithCorrectAccountActivationUrl(string expectedAccountActivationUrl, string baseAddress, string accountActivationUrl)
        {
            Run(f => f.Arrange(baseAddress, accountActivationUrl), f => f.Act(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.AccountActivationUrl.Should().Be(expectedAccountActivationUrl);
            });
        }
    }

    public class IdentityServerConfigurationFactoryTestsFixture
    {
        private readonly Mock<IOidcConfiguration> _oidcConfiguration;
        private readonly OidcConfigurationFactory _oidcConfigurationFactory;

        public IdentityServerConfigurationFactoryTestsFixture()
        {
            _oidcConfiguration = new Mock<IOidcConfiguration>();
            _oidcConfigurationFactory = new OidcConfigurationFactory(_oidcConfiguration.Object);
        }

        public void Arrange(string baseAddress, string accountActivationUrl)
        {
            _oidcConfiguration.Setup(isc => isc.BaseAddress).Returns(baseAddress);
            _oidcConfiguration.Setup(isc => isc.AccountActivationUrl).Returns(accountActivationUrl);
        }

        public ConfigurationContext Act()
        {
            return _oidcConfigurationFactory.Get();
        }
    }
}