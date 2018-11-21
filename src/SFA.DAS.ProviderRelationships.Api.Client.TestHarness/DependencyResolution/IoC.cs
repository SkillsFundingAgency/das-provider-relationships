using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ProviderRelationshipsApiClientRegistry>();
            });
        }
    }
}