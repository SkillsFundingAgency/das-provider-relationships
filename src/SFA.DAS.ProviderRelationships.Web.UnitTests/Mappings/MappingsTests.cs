using AutoMapper;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Web.Mappings;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Mappings
{
    [TestFixture]
    public class MappingsTests
    {
        [Test]
        public void AssertConfigurationIsValid_WhenAssertingConfigurationIsValid_ThenShouldNotThrowException()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMappings)));

            config.AssertConfigurationIsValid();
        }
    }
}