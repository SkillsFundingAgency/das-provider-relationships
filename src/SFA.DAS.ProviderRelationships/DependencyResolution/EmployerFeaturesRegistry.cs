using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class EmployerFeaturesRegistry : Registry
    {
        public EmployerFeaturesRegistry()
        {
            For<EmployerFeaturesConfiguration>().Use(() => ConfigurationHelper.GetConfiguration<EmployerFeaturesConfiguration>("SFA.DAS.Authorization.EmployerFeatures")).Singleton();
        }
    }
}