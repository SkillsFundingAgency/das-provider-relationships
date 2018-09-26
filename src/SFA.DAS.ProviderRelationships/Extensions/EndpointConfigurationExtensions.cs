using System;
using NServiceBus;
using SFA.DAS.NServiceBus.AzureServiceBus;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration UseAzureServiceBusTransport(this EndpointConfiguration config, Func<string> connectionStringBuilder)
        {
            var isDevelopment = ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL);

            config.UseAzureServiceBusTransport(isDevelopment, connectionStringBuilder, r => {});

            return config;
        }
    }
}