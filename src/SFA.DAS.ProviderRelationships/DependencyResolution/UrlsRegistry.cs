using StructureMap;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class UrlsRegistry : Registry
    {
        public UrlsRegistry()
        {
            For<IEmployerUrls>().Use<EmployerUrls>();
        }
    }
}