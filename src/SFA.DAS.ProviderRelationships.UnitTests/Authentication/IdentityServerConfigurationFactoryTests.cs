using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Authentication
{
    [TestFixture]
    public class IdentityServerConfigurationFactoryTests : FluentTest<IdentityServerConfigurationFactoryTestsFixture>
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
        private readonly IdentityServerConfigurationFactory _identityServerConfigurationFactory;
        private readonly Mock<IIdentityServerConfiguration> _mockIdentityServerConfiguration;

        public IdentityServerConfigurationFactoryTestsFixture()
        {
            _mockIdentityServerConfiguration = new Mock<IIdentityServerConfiguration>();
            _identityServerConfigurationFactory = new IdentityServerConfigurationFactory(_mockIdentityServerConfiguration.Object);
        }

        public void Arrange(string baseAddress, string accountActivationUrl)
        {
            _mockIdentityServerConfiguration.Setup(isc => isc.BaseAddress).Returns(baseAddress);
            _mockIdentityServerConfiguration.Setup(isc => isc.AccountActivationUrl).Returns(accountActivationUrl);
        }

        public ConfigurationContext Act()
        {
            return _identityServerConfigurationFactory.Get();
        }
    }
}
