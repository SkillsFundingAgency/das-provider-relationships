using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IStartup
    {
        Task StartAsync();
        Task StopAsync();
    }
}