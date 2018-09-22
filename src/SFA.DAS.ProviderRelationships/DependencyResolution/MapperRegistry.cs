using AutoMapper;
using SFA.DAS.ProviderRelationships.Mappings;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class MapperRegistry : Registry
    {
        public MapperRegistry()
        {
            For<IConfigurationProvider>().Use(c => new MapperConfiguration(cfg => cfg.AddProfiles(typeof(TrainingProviderMappings).Assembly))).Singleton();
            For<IMapper>().Use(c => c.GetInstance<IConfigurationProvider>().CreateMapper()).Singleton();
        }
    }
}