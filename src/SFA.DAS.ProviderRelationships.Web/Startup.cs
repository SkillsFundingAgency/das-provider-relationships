using System;
using System.IO;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.Web.AppStart;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Filters;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;
using SFA.DAS.UnitOfWork.Mvc.Extensions;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfigurationRoot _configuration;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            if (!configuration.IsDev())
            {
                config.AddJsonFile("appsettings.json", false)
                    .AddJsonFile("appsettings.Development.json", true);
            }
#endif
            
            config.AddEnvironmentVariables();
            if (!configuration.IsDev())
            {
                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["EnvironmentName"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }
            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);

            var identityServerConfiguration = _configuration
                .GetSection("Oidc")
                .Get<IdentityServerConfiguration>();
            
            services.AddLogging();

            services.AddAutoConfiguration();
            services.AddConfigurationOptions(_configuration);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddApplicationServices();
            services.AddApiClients();
            
            services.AddServiceRegistration(_configuration);
            services.AddMediatR(typeof(FindProviderToAddQuery));
            services.AddAutoMapper(typeof(AccountProviderLegalEntityMappings));
            services.AddDatabaseRegistration(_configuration, _configuration["EnvironmentName"]);
            
            services.AddEmployerAuthorisationServices();
            
            var clientId = "no-auth-id";
            if (_configuration["ProviderRelationshipsWebConfiguration:UseGovUkSignIn"] != null && 
                _configuration["ProviderRelationshipsWebConfiguration:UseGovUkSignIn"]
                    .Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddAndConfigureGovUkAuthentication(
                    _configuration, 
                    $"{typeof(Startup).Assembly.GetName().Name}.Auth",
                    typeof(PostAuthenticationHandler));
                clientId = identityServerConfiguration.ClientId;
            }
            else //legacy auth
            {
                if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                        .Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    services.AddEmployerStubAuthentication();    
                }
                else
                {
                    services.AddAndConfigureEmployerAuthentication(identityServerConfiguration);
                    clientId = identityServerConfiguration.ClientId;
                }
                services.AddAuthenticationCookie();
            }
            
            services.AddLogging();
            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });
            services.AddMaMenuConfiguration(RouteNames.EmployerSignOut, clientId,_configuration["Environment"]);

            services.Configure<RouteOptions>(options => { })
                .AddMvc(options =>
                {
                    options.Filters.Add(new GoogleAnalyticsFilter());
                    if (!_configuration.IsDev())
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }

                });

            services.AddApplicationInsightsTelemetry();

            if (!_environment.IsDevelopment())
            {
                services.AddHealthChecks();
            }
#if DEBUG
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
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
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseUnitOfWork();

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
