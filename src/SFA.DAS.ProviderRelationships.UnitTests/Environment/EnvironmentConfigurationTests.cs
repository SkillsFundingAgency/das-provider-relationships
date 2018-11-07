using System.Collections.Specialized;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Environment
{
    [TestFixture]
    public class EnvironmentConfigurationTests : FluentTest<EnvironmentConfigurationTestsFixture>
    {
        
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

//        public void Get()
//        {
//            EnvironmentConfiguration.Get<>();
//        }
    }
}