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
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.Filters;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.Mvc.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.Microsoft;
using SFA.DAS.Validation.Mvc.Extensions;

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

            services.AddApplicationServices()
                    .AddApiClients();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(FindProviderToAddQuery)))
                    .AddAutoMapper(typeof(AccountProviderLegalEntityMappings),
                        typeof(Mappings.HealthCheckMappings));

            var providerRelationshipsConfiguration = _configuration.Get<ProviderRelationshipsConfiguration>();

            services.AddDatabaseRegistration(providerRelationshipsConfiguration.DatabaseConnectionString);

            services
                .AddEntityFramework(providerRelationshipsConfiguration)
                .AddEntityFrameworkUnitOfWork<ProviderRelationshipsDbContext>()
                .AddNServiceBusClientUnitOfWork();

            services.AddAuthenticationServices();

            var identityServerConfiguration = _configuration
                .GetSection("Oidc")
                .Get<IdentityServerConfiguration>();

            if (_configuration.UseGovUkSignIn())
            {
                services.AddAndConfigureGovUkAuthentication(
                    _configuration,
                    typeof(EmployerAccountPostAuthenticationClaimsHandler),
                    "",
                    "/service/SignIn-Stub");
                services.AddMaMenuConfiguration(RouteNames.SignOut, _configuration["ResourceEnvironmentName"]);
            }
            else
            {
                services.AddAndConfigureEmployerAuthentication(identityServerConfiguration);
                services.AddMaMenuConfiguration(RouteNames.SignOut, identityServerConfiguration.ClientId, _configuration["ResourceEnvironmentName"]);
            }

            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

            services.Configure<RouteOptions>(options =>
                {

                }).AddMvc(options =>
                {
                    options.AddValidation();

                    if (!_configuration.IsDev())
                    {
                        options.Filters.Add(new GoogleAnalyticsFilterAttribute());
                        options.Filters.Add(new UrlsViewBagFilterAttribute());
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }
                })
                .SetDefaultNavigationSection(NavigationSection.ApprenticesHome);

            services.AddApplicationInsightsTelemetry();

            services.AddSession(options => options.Cookie.IsEssential = true);

            if (!_environment.IsDevelopment())
            {
                services.AddHealthChecks();
                services.AddDataProtection(_configuration);
            }
#if DEBUG
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
#endif
        }

        public void ConfigureContainer(UpdateableServiceProvider serviceProvider)
        {
            serviceProvider.StartNServiceBus(_configuration, _configuration.IsDevOrLocal());
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
                app.UseExceptionHandler("/error/500");
                app.UseHealthChecks("/ping");
            }
            
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseUnitOfWork();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseSession();

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
