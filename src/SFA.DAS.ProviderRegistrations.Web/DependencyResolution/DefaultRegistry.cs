using Microsoft.Extensions.Configuration;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.ProviderRegistrations.Web.Authentication;
using SFA.DAS.ProviderRegistrations.Web.Authorization;
using SFA.DAS.ProviderUrlHelper;
using StructureMap;
using StructureMap.Building.Interception;

namespace SFA.DAS.ProviderRegistrations.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        private const string ServiceName = "SFA.DAS.ProviderRegistrations";

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith(ServiceName));
                    scan.RegisterConcreteTypesAgainstTheFirstInterface();
                });

            For<IAuthorizationHandler>().Add<ServiceAuthorizationHandler>();
            For<IAuthenticationService>().Use<AuthenticationService>().Singleton();
            For<IAuthorizationContextProvider>().Use<AuthorizationContextProvider>();
            For<ILinkGenerator>().Use<LinkGenerator>().Singleton();
            // Toggle<IProviderRelationshipsApiClient, StubProviderRelationshipsApiClient>("UseStubProviderRelationships");
        }
        
        private void Toggle<TPluginType, TConcreteType>(string configurationKey) where TConcreteType : TPluginType
        {
            For<TPluginType>().InterceptWith(new FuncInterceptor<TPluginType>((c, o) => c.GetInstance<IConfiguration>().GetValue<bool>(configurationKey) ? c.GetInstance<TConcreteType>() : o));
        }
    }
}
