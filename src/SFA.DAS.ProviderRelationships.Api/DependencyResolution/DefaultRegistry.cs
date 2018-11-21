using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api.Logging;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            For<ILoggingContext>().Use(c => GetLoggingContext(c));
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