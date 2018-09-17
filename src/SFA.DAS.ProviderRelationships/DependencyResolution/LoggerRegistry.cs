using SFA.DAS.NLog.Logger;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            For<ILog>().Use(c => new NLogLogger(c.ParentType, c.GetInstance<ILoggingContext>(), null)).AlwaysUnique();
        }
    }
}