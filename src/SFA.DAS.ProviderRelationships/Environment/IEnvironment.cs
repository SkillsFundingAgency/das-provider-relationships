namespace SFA.DAS.ProviderRelationships.Environment
{
    public interface IEnvironment
    {
        DasEnv Current { get; }
        bool IsCurrent(params DasEnv[] environment);
    }
}