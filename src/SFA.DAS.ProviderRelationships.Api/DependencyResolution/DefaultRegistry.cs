using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api.Data;
using SFA.DAS.ProviderRelationships.Api.Logging;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            For<ILoggingContext>().Use(c => GetLoggingContext(c));
            For<IProviderRelationshipsDbContextFactory>().Use<ProviderRelationshipsApiDbContextFactory>();
            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
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