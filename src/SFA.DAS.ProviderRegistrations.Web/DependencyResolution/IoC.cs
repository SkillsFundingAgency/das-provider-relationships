using SFA.DAS.Authorization.DependencyResolution;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRegistrations.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<AuthorizationRegistry>();
            registry.IncludeRegistry<AutoConfigurationRegistry>();
            //registry.IncludeRegistry<CommitmentsApiClientRegistry>();
            //registry.IncludeRegistry<CommitmentPermissionsAuthorizationRegistry>();
            registry.IncludeRegistry<ConfigurationRegistry>();
            //registry.IncludeRegistry<CommitmentsSharedRegistry>();
            registry.IncludeRegistry<MediatorRegistry>();
            registry.IncludeRegistry<EncodingRegistry>();
            //registry.IncludeRegistry<ProviderFeaturesAuthorizationRegistry>();
            //registry.IncludeRegistry<ProviderPermissionsAuthorizationRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}