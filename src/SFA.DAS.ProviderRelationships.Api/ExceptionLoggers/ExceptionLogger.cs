using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Extensions;
using WebApi.StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.ExceptionLoggers
{
    public class ExceptionLogger : System.Web.Http.ExceptionHandling.ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var logger = context.Request.GetService<ILog>();
            
            logger.Error(context.Exception, context.Exception.GetAggregateMessage());
        }
    }
}