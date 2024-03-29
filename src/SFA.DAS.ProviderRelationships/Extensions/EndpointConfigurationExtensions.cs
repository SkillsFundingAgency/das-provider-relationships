﻿
using System;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;

namespace SFA.DAS.ProviderRelationships.Extensions;

public static class EndpointConfigurationExtensions
{
    public static EndpointConfiguration ConfigureServiceBusTransport(this EndpointConfiguration config, Func<string> connectionStringBuilder, bool isDevelopment)
    {
        if (isDevelopment)
        {
            var transport = config.UseTransport<LearningTransport>();
            transport.Transactions(TransportTransactionMode.ReceiveOnly);
        }
        else
        {
            config.UseAzureServiceBusTransport(connectionStringBuilder());
        }

        config.UseMessageConventions();

        return config;
    }
}