using System.Web.Http;
using SFA.DAS.ProviderRelationships.Api.Authentication.AzureActiveDirectory.DependencyResolution;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using WebApi.StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(HttpConfiguration config)
        {
            config.UseStructureMap(c =>
            {
                c.AddRegistry<AzureActiveDirectoryAuthenticationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<HashingRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}