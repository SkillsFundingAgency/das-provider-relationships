﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Web.Authorization;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage(
                    ProviderRegistrationsConfigurationKeys.Encoding,
                    ProviderRegistrationsConfigurationKeys.ProviderRegistrations));
        }
    }

    public static class IServiceCollectionExtensions
    {
        //private const string Key = "SFA.DAS.ProviderCommitments";

        //public static IServiceCollection AddDasConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddOptions();

        //    services.Configure<ApprenticeshipInfoServiceConfiguration>(configuration.GetSection($"{Key}:ApprenticeshipInfoServiceConfiguration"));
        //    services.Configure<AuthenticationSettings>(configuration.GetSection($"{Key}:AuthenticationSettings"));
        //    services.Configure<PublicAccountIdHashingConfiguration>(configuration.GetSection($"{Key}:PublicAccountIdHashingConfiguration"));
        //    services.Configure<PublicAccountLegalEntityIdHashingConfiguration>(configuration.GetSection($"{Key}:PublicAccountLegalEntityIdHashingConfiguration"));
        //    services.Configure<CommitmentsClientApiConfiguration>(configuration.GetSection($"{Key}:CommitmentsClientApi"));
        //    return services;
        //}

        public static IServiceCollection AddDasAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ProviderMatch", policy => policy.Requirements.Add(new ProviderRequirement()));
            });

            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IAuthorizationHandler, ProviderHandler>();

            return services;
        }
    }
}