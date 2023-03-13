using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.NServiceBus.Features.ClientOutbox.Data;
using SFA.DAS.Provider.Shared.UI.Startup;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.Filters;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.Mvc.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;
using ClientOutboxPersisterV2 = SFA.DAS.ProviderRelationships.Web.ServiceRegistrations.ClientOutboxPersisterV2;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfigurationRoot _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration.BuildDasConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddLogging();
            services.AddHttpContextAccessor();

            services.AddAutoConfiguration();
            services.AddConfigurationOptions(_configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddApplicationServices()
                    .AddApiClients();

            services.AddMediatR(typeof(FindProviderToAddQuery))
                    .AddAutoMapper(typeof(AccountProviderLegalEntityMappings));

            var providerRelationshipsConfiguration = _configuration
                .Get<ProviderRelationshipsConfiguration>();

            services.AddEntityFramework(providerRelationshipsConfiguration);

            services.AddNServiceBusClientUnitOfWork()
                    .AddEntityFrameworkUnitOfWork<ProviderRelationshipsDbContext>()
                    .AddUnitOfWork();

            services.AddAuthenticationServices(useStubAuthHandler: _configuration.IsDevOrLocal());

            var identityServerConfiguration = _configuration
                .GetSection("Oidc")
                .Get<IdentityServerConfiguration>();

            var clientId = "no-auth-id";
            if (_configuration.UseGovUkSignIn())
            {
                services.AddAndConfigureGovUkAuthentication(
                    _configuration,
                    $"{typeof(Startup).Assembly.GetName().Name}.Auth",
                    typeof(EmployerAccountPostAuthenticationClaimsHandler));
                clientId = identityServerConfiguration.ClientId;
            }
            else
            {
                services.AddAndConfigureEmployerAuthentication(identityServerConfiguration);
            }

            services.AddProviderUiServiceRegistration(_configuration);

            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
            services.AddMaMenuConfiguration(RouteNames.EmployerSignOut, clientId, _configuration["Environment"]);

            services.Configure<RouteOptions>(options => { })
                .AddMvc(options =>
                {
                    options.Filters.Add(new GoogleAnalyticsFilter());
                    if (!_configuration.IsDev())
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }

                })
                .SetDefaultNavigationSection(NavigationSection.None);

            services.AddApplicationInsightsTelemetry();

            if (!_environment.IsDevelopment())
            {
                services.AddHealthChecks();
                services.AddDataProtection();
            }
#if DEBUG
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
#endif
        }

        public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
        {
            serviceProvider.StartNServiceBus(_configuration, _configuration.IsDevOrLocal() || _configuration.IsTest());
            var outboxStorageService = serviceProvider.FirstOrDefault(serv => serv.ServiceType == typeof(IClientOutboxStorageV2));
            serviceProvider.Remove(outboxStorageService);
            serviceProvider.AddScoped<IClientOutboxStorageV2, ClientOutboxPersisterV2>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHealthChecks();
                app.UseExceptionHandler("/Error/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseUnitOfWork();
            app.UseRouting();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                if (context.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.Response.Headers.Remove("X-Frame-Options");
                }

                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                await next();

                if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    var originalPath = context.Request.Path.Value;
                    context.Items["originalPath"] = originalPath;
                    context.Request.Path = "/error/404";
                    await next();
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
