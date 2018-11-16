using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class UrlsRegistry : Registry
    {
        public UrlsRegistry()
        {
            For<IEmployerUrls>().Use<EmployerUrls>();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>()).Singleton();
        }
    }
}