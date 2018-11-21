namespace SFA.DAS.ProviderRelationships.ReadStore.Configuration
{
    public interface ITableStorageConfigurationService
    {
        T Get<T>();
    }
}