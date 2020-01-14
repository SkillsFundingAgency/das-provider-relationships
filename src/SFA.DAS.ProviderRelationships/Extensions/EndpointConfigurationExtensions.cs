using System;
using NServiceBus;

namespace SFA.DAS.ProviderRelationships.Extensions
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration UseAzureServiceBusTransport(
            this EndpointConfiguration config, Func<string> connectionStringBuilder, bool isDevelopment)
        {
            config.UseAzureServiceBusTransport(connectionStringBuilder, isDevelopment);

            return config;
        }
    }
}