using System.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<ILoggerFactory>().Use(() => new LoggerFactory().AddApplicationInsights(ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"], null).AddNLog()).Singleton();
        }
    }
}