using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public static class StartupTasks
    {
        private static IEnumerable<IStartupTask> _startups;

        public static Task StartAsync(IEnumerable<IStartupTask> startups)
        {
            return Task.WhenAll((_startups = startups).Select(t => t.StartAsync()));
        }
        
        public static Task StopAsync()
        {
            return Task.WhenAll(_startups.Reverse().Select(t => t.StopAsync()));
        }
    }
}