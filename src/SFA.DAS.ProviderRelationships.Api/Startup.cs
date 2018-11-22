using Owin;
using Microsoft.Owin;
using SFA.DAS.ProviderRelationships.Api;

[assembly: OwinStartup(typeof(Startup))]

namespace SFA.DAS.ProviderRelationships.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}