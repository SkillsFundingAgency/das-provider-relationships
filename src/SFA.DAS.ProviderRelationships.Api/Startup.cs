using Microsoft.OpenApi.Models;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.NServiceBus.Features.ClientOutbox.Data;
using SFA.DAS.NServiceBus.SqlServer.Features.ClientOutbox.Data;
using SFA.DAS.ProviderRelationships.Api.Authentication;
using SFA.DAS.ProviderRelationships.Api.Authorization;
using SFA.DAS.ProviderRelationships.Api.Extensions;
using SFA.DAS.ProviderRelationships.Api.Filters;
using SFA.DAS.ProviderRelationships.Api.Handlers;
using SFA.DAS.ProviderRelationships.Api.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;

namespace SFA.DAS.ProviderRelationships.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration.BuildDasConfiguration();
    }

    public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
    {
        serviceProvider.StartNServiceBus(_configuration, _configuration.IsDevOrLocal());
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfigurationSections(_configuration);

        var providerRelationshipsConfiguration = _configuration.Get<ProviderRelationshipsConfiguration>();
        var isDevelopment = _configuration.IsDevOrLocal();

        services.AddLogging();

        services.AddApiAuthentication(_configuration, isDevelopment)
                .AddApiAuthorization(isDevelopment);

        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo {
                Version = "v1",
                Title = "Provider Relationships API"
            });
        });

        services.AddApplicationServices();
        services.AddAutoMapper(typeof(Startup), typeof(AccountLegalEntityMappings));
        services.AddDatabaseRegistration(providerRelationshipsConfiguration.DatabaseConnectionString);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(RevokePermissionsCommand)));
        services.AddReadStoreServices();
        services.AddTransient<IClientOutboxStorageV2, ClientOutboxPersisterV2>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; })
             .AddMvc(opt =>
             {
                 if (!_configuration.IsDevOrLocal())
                 {
                     opt.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
                 }
             });

        services.AddApplicationInsightsTelemetry();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection()
            .UseApiGlobalExceptionHandler(loggerFactory.CreateLogger(nameof(Startup)))
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            })
            .UseSwagger()
            .UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Provider Relationships API");
                opt.RoutePrefix = string.Empty;
            });
    }
}