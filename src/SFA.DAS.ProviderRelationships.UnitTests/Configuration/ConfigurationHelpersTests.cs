using System.Configuration;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Configuration
{
    [TestFixture]
    public class ConfigurationHelpersTests : FluentTest<ConfigurationHelpersTestsFixture>
    {
        [Test]
        public void CurrentEnvironment_WhenGettingCurrentEnvironment_TheShouldReturnCurrentEnvironment()
        {
            Run(f => f.SetCurrentEnvironment(DasEnv.LOCAL), f => ConfigurationHelper.CurrentEnvironment, (f, r) => r.Should().Be(DasEnv.LOCAL));
        }

        [Test]
        public void CurrentEnvironment_WhenGettingCurrentEnvironmentMoreThanOnce_TheShouldReturnCurrentEnvironmentFromCache()
        {
            Run(f => f.SetCurrentEnvironmentAfterGetCurrentEnvironment(DasEnv.LOCAL, DasEnv.AT), f => ConfigurationHelper.CurrentEnvironment, (f, r) => r.Should().Be(DasEnv.LOCAL));
        }

        [Test]
        public void IsCurrentEnvironment_WhenCurrentEnvironmentDoesMatch_TheShouldReturnTrue()
        {
            Run(f => f.SetCurrentEnvironment(DasEnv.LOCAL), f => ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL), (f, r) => r.Should().BeTrue());
        }

        [Test]
        public void IsCurrentEnvironment_WhenCurrentEnvironmentDoesNotMatch_TheShouldReturnFalse()
        {
            Run(f => f.SetCurrentEnvironment(DasEnv.LOCAL), f => ConfigurationHelper.IsCurrentEnvironment(DasEnv.AT), (f, r) => r.Should().BeFalse());
        }
    }

    public class ConfigurationHelpersTestsFixture : FluentTestFixture
    {
        public ConfigurationHelpersTestsFixture SetCurrentEnvironment(DasEnv environment)
        {
            ConfigurationManager.AppSettings["EnvironmentName"] = environment.ToString();

            return this;
        }

        public ConfigurationHelpersTestsFixture SetCurrentEnvironmentAfterGetCurrentEnvironment(DasEnv oldEnvironment, DasEnv newEnvironment)
        {
            ConfigurationManager.AppSettings["EnvironmentName"] = oldEnvironment.ToString();

            var env = ConfigurationHelper.CurrentEnvironment;

            ConfigurationManager.AppSettings["EnvironmentName"] = newEnvironment.ToString();

            return this;
        }
    }
}