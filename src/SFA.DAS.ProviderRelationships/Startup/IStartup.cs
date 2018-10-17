using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Startup
{
    public interface IStartup
    {
        Task StartAsync();
        Task StopAsync();
    }
}