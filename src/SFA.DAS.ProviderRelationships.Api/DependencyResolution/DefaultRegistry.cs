using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api.Logging;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    //public class DefaultRegistry : Registry
    //{
    //    public DefaultRegistry()
    //    {
    //        For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContextHelper.Current));
    //        For<ILoggingContext>().Use(c => GetLoggingContext(c));
    //        For<IProviderRelationshipsDbContextFactory>().Use<DbContextWithNServiceBusTransactionFactory>();
    //    }

    //    private ILoggingContext GetLoggingContext(IContext context)
    //    {
    //        LoggingContext loggingContext = null;

    //        try
    //        {
    //            loggingContext = new LoggingContext(context.GetInstance<HttpContextBase>());
    //        }
    //        catch (HttpException)
    //        {
    //        }

    //        return loggingContext;
    //    }
    //}
}