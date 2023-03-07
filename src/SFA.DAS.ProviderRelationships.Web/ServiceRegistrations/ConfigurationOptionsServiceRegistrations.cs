﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.PAS.Account.Api.ClientV2.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using ConfigurationKeys = SFA.DAS.ProviderRelationships.Configuration.ConfigurationKeys;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<ProviderRelationshipsConfiguration>(configuration.GetSection(ConfigurationKeys.ProviderRelationships));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsConfiguration>>().Value);

        services.AddSingleton(configuration.Get<ProviderRelationshipsConfiguration>());

        services.Configure<OuterApiConfiguration>(configuration.GetSection(nameof(OuterApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<OuterApiConfiguration>>().Value);

        services.Configure<IdentityServerConfiguration>(configuration.GetSection("Oidc"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<IdentityServerConfiguration>>().Value);
        //--- delving into the unknown: :scream:
        //notable ommissions: featuretoggle,
        // these were prev part of ProviderRelationshipsConfiguration: roatpapi, pasaccountapi, recruitapi, registrationapi 
        services.Configure<OidcConfiguration>(configuration.GetSection("Oidc"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<OidcConfiguration>>().Value);

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureActiveDirectory"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        services.Configure<EmployerUrlsConfiguration>(configuration.GetSection("EmployerUrls"));
        services.AddSingleton<IEmployerUrlsConfiguration>(cfg => cfg.GetService<IOptions<EmployerUrlsConfiguration>>().Value);

        services.Configure<ReadStoreConfiguration>(configuration.GetSection("ReadStore"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ReadStoreConfiguration>>().Value);

        services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection("ProviderRelationshipsApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);

        services.Configure<RoatpApiConfiguration>(configuration.GetSection("RoatpApiClientSettings"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpApiConfiguration>>().Value);

        services.Configure<PasAccountApiConfiguration>(configuration.GetSection("PasAccountApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<PasAccountApiConfiguration>>().Value);

        services.Configure<RecruitApiConfiguration>(configuration.GetSection("RecruitApiClientConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiConfiguration>>().Value);

        services.Configure<RegistrationApiConfiguration>(configuration.GetSection("RegistrationApiClientConfiguration"));
        services.AddSingleton<IRegistrationApiConfiguration>(cfg => cfg.GetService<IOptions<RegistrationApiConfiguration>>().Value);


        services.Configure<EncodingConfig>(config =>
        {
            var encodings = configuration.GetSection("Encodings").Get<List<Encoding.Encoding>>();
            config.Encodings = encodings;
        });
        services.AddSingleton(cfg => cfg.GetService<IOptions<EncodingConfig>>().Value);
    }
}