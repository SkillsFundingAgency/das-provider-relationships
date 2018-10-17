using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Startup
{
    public interface IStartupTask
    {
        Task StartAsync();
        Task StopAsync();
    }
}