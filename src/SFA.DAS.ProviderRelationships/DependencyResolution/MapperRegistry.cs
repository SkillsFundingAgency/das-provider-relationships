using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class MapperRegistry : Registry
    {
        public MapperRegistry()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("SFA.DAS"));
            var profiles = new List<Profile>();
            foreach (var assembly in assemblies)
            {
                foreach (var profile in assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x)))
                {
                    profiles.Add(Activator.CreateInstance(profile) as Profile);
                }
            }
            For<IConfigurationProvider>().Use(c => new MapperConfiguration(cfg => cfg.AddProfiles(profiles))).Singleton();
            For<IMapper>().Use(c => c.GetInstance<IConfigurationProvider>().CreateMapper()).Singleton();
        }
    }
}