using System.Collections.Specialized;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Environment
{
    [TestFixture]
    public class EnvironmentConfigurationTests : FluentTest<EnvironmentConfigurationTestsFixture>
    {
//        [Test]
//        public WhenGettingEnvironmentsConfiguration_ThenX()
//        {
//            Run(f => f, f => f.Get(), (f, r) => r.Should().BeNull());
//        }
    }

    public class EnvironmentConfigurationTestsFixture
    {
        public EnvironmentConfiguration EnvironmentConfiguration { get; set; }
        public NameValueCollection AppSettings { get; set; }
        public Mock<IEnvironment> Environment { get; set; }

        public EnvironmentConfigurationTestsFixture()
        {
            AppSettings = new NameValueCollection();
            Environment = new Mock<IEnvironment>();
            EnvironmentConfiguration = new EnvironmentConfiguration(AppSettings, Environment.Object);
        }

        public void Set()
        {
            AppSettings["ConfigurationStorageConnectionString"] = "";
        }
        
//        public void Get()
//        {
//            EnvironmentConfiguration.Get<>();
//        }
    }
}