using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IStartupTask
    {
        Task StartAsync();
        Task StopAsync();
    }
}