namespace SFA.DAS.ProviderRelationships.Configuration
{
    public interface IEnvironmentConfiguration
    {
        T Get<T>(string serviceName);
    }
}