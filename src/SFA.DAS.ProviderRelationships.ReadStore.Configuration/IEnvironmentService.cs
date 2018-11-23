﻿namespace SFA.DAS.ProviderRelationships.ReadStore.Configuration
{
    public interface IEnvironmentService
    {
        string GetVariable(string variableName);
        bool IsCurrent(params DasEnv[] environment);
    }
}