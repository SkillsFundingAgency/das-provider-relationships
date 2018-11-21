using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.ReadStore.Configuration
{
    public interface ITableStorageConfigurationService
    {
        Task<T> Get<T>();
    }
}