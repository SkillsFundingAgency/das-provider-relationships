namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IConfiguration
    {
        T Get<T>(string serviceName);
    }
}