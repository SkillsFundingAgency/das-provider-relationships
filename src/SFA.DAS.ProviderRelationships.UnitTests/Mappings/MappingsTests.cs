﻿using AutoMapper;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Mappings;

namespace SFA.DAS.ProviderRelationships.UnitTests.Mappings
{
    [TestFixture]
    [Parallelizable]
    public class MappingsTests
    {
        [Test]
        public void AssertConfigurationIsValid_WhenAssertingConfigurationIsValid_ThenShouldNotThrowException()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(HealthCheckMappings)));

            config.AssertConfigurationIsValid();
        }
    }
}