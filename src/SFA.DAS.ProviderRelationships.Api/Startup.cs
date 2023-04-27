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
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;

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
        var providerRelationshipsConfiguration = _configuration.Get<ProviderRelationshipsConfiguration>();
        var isDevelopment = _configuration.IsDevOrLocal();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(RevokePermissionsCommand)));

        services.AddDatabaseRegistration(providerRelationshipsConfiguration.DatabaseConnectionString);
        services.AddApplicationServices();
        services.AddReadStoreServices();
        services.AddEntityFrameworkUnitOfWork<ProviderRelationshipsDbContext>();
        services.AddNServiceBusClientUnitOfWork();
        services.AddTransient<IClientOutboxStorageV2, ClientOutboxPersisterV2>();

        services.AddApiAuthentication(_configuration, isDevelopment);
        services.AddApiAuthorization(isDevelopment);

        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo {
                Version = "v1",
                Title = "Provider Relationships API"
            });
        });

        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.AddConfigurationSections(_configuration)
            .Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; })
            .AddMvc(opt =>
            {
                if (!_configuration.IsDevOrLocal())
                {
                    opt.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
                }
            });

        services.AddLogging();
        services.AddApplicationInsightsTelemetry();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection()
            .UseApiGlobalExceptionHandler(loggerFactory.CreateLogger("Startup"))
            .UseStaticFiles()
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