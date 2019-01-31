using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Configuration
{
    [TestFixture]
    [Parallelizable]
    public class GoogleAnalyticsConfigurationFactoryTests : FluentTest<GoogleAnalyticsConfigurationFactoryTestsFixture>
    {
        [Test]
        public void CreateConfiguration_WhenEnvironmentIsNotPreprodOrProd_ThenShouldReturnConfigurationWithNullContainerIdAndTrackingId()
        {
            Run(f => f.CreateConfiguration(), (f, r) => r.Should().NotBeNull()
                .And.Match<GoogleAnalyticsConfiguration>(c => c.ContainerId == null && c.TrackingId == null));
        }
        
        [Test]
        public void CreateConfiguration_WhenEnvironmentIsPreprod_ThenShouldReturnConfigurationWithContainerIdAndTrackingId()
        {
            Run(f => f.SetEnvironment(DasEnv.PREPROD), f => f.CreateConfiguration(), (f, r) => r.Should().NotBeNull()
                .And.Match<GoogleAnalyticsConfiguration>(c => c.ContainerId == "GTM-KWQBWGJ" && c.TrackingId == "UA-83918739-10"));
        }
        
        [Test]
        public void CreateConfiguration_WhenEnvironmentIsProd_ThenShouldReturnConfigurationWithContainerIdAndTrackingId()
        {
            Run(f => f.SetEnvironment(DasEnv.PROD), f => f.CreateConfiguration(), (f, r) => r.Should().NotBeNull()
                .And.Match<GoogleAnalyticsConfiguration>(c => c.ContainerId == "GTM-KWQBWGJ" && c.TrackingId == "UA‌-83918739-9"));
        }
    }

    public class GoogleAnalyticsConfigurationFactoryTestsFixture
    {
        public IGoogleAnalyticsConfigurationFactory GoogleAnalyticsConfigurationFactory { get; set; }
        public Mock<IEnvironmentService> EnvironmentService { get; set; }

        public GoogleAnalyticsConfigurationFactoryTestsFixture()
        {
            EnvironmentService = new Mock<IEnvironmentService>();
            GoogleAnalyticsConfigurationFactory = new GoogleAnalyticsConfigurationFactory(EnvironmentService.Object);
        }

        public GoogleAnalyticsConfiguration CreateConfiguration()
        {
            return GoogleAnalyticsConfigurationFactory.CreateConfiguration();
        }

        public GoogleAnalyticsConfigurationFactoryTestsFixture SetEnvironment(DasEnv env)
        {
            EnvironmentService.Setup(e => e.IsCurrent(env)).Returns(true);
            
            return this;
        }
    }
}