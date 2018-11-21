using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ApprenticeshipInfoServiceApiRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EnvironmentRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}