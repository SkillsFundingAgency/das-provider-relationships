using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Startup
{
    public class Startup : IStartup
    {
        private readonly IEnumerable<IStartupTask> _startupTasks;

        public Startup(IEnumerable<IStartupTask> startupTasks)
        {
            _startupTasks = startupTasks;
        }

        public Task StartAsync()
        {
            return Task.WhenAll(_startupTasks.Select(t => t.StartAsync()));
        }

        public Task StopAsync()
        {
            return Task.WhenAll(_startupTasks.Reverse().Select(t => t.StopAsync()));
        }
    }
}