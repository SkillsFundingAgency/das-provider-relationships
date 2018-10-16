using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Logging;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IStartupTask>().Add<StartupEndpoint>();
            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            For<ILoggingContext>().Use(c => GetLoggingContext(c));
            
            Scan(s =>
            {
                s.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS"));
                s.With(new ControllerConvention());
            });
        }

        private ILoggingContext GetLoggingContext(IContext context)
        {
            LoggingContext loggingContext = null;

            try
            {
                loggingContext = new LoggingContext(context.GetInstance<HttpContextBase>());
            }
            catch (HttpException)
            {
            }

            return loggingContext;
        }
    }
}