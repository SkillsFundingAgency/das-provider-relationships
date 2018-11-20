using AutoMapper;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Web.Mappings;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Mappings
{
    [TestFixture]
    [Parallelizable]
    public class MappingsTests
    {
        [Test]
        public void AssertConfigurationIsValid_WhenAssertingConfigurationIsValid_ThenShouldNotThrowException()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(AccountProviderMappings)));

            config.AssertConfigurationIsValid();
        }
    }
}