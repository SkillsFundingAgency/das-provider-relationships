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

        // same as above without using Run() (for comparison)

        //[SetUp]
        //public void SetUp()
        //{
        //    _fixture = new IdentityServerConfigurationFactoryTestsFixture();
        //}

        //private IdentityServerConfigurationFactoryTestsFixture _fixture;

        //[TestCase("https://test2-login.apprenticeships.sfa.bis.gov.uk/account/confirm", "https://test2-login.apprenticeships.sfa.bis.gov.uk/identity", "/account/confirm")]
        //[TestCase("https://example.com/account/confirm", "https://example.com", "/account/confirm")]
        //public void WhenGettingConfigurationContext_ThenShouldReturnConfigurationContextWithCorrectAccountActivationUrlX(string expectedAccountActivationUrl, string baseAddress, string accountActivationUrl)
        //{
        //    _fixture.Arrange(baseAddress, accountActivationUrl);

        //    var result = _fixture.Act();

        //    result.Should().NotBeNull();
        //    result.AccountActivationUrl.Should().Be(expectedAccountActivationUrl);
        //}
    }

    public class IdentityServerConfigurationFactoryTestsFixture : FluentTestFixture
    {
        public IdentityServerConfigurationFactory IdentityServerConfigurationFactory;
        public Mock<IIdentityServerConfiguration> MockIdentityServerConfiguration;

        public IdentityServerConfigurationFactoryTestsFixture()
        {
            MockIdentityServerConfiguration = new Mock<IIdentityServerConfiguration>();
            IdentityServerConfigurationFactory = new IdentityServerConfigurationFactory(MockIdentityServerConfiguration.Object);
        }

        public void Arrange(string baseAddress, string accountActivationUrl)
        {
            MockIdentityServerConfiguration.Setup(isc => isc.BaseAddress).Returns(baseAddress);
            MockIdentityServerConfiguration.Setup(isc => isc.AccountActivationUrl).Returns(accountActivationUrl);
        }

        public ConfigurationContext Act()
        {
            return IdentityServerConfigurationFactory.Get();
        }
    }
}
