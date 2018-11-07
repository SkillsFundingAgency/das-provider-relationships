namespace SFA.DAS.ProviderRelationships.Environment
{
    //this could live in Environment or Configuration
    public interface IEnvironmentConfiguration
    {
        T Get<T>(string serviceName);
    }
}