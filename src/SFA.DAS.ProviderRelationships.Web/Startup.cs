﻿using System;
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
using Microsoft.Extensions.Options;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Encoding;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.AppStart;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
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
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }
            _configuration = config.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConfigurationOptions(_configuration);
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddOptions();
            
            var clientId = "no-auth-id";
            services.AddEmployerAuthorisationServices();
            if (_configuration["ProviderRelationshipsConfiguration:UseGovSignIn"] != null && _configuration["ProviderRelationshipsConfiguration:UseGovSignIn"]
                    .Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddAndConfigureGovUkAuthentication(_configuration, $"{typeof(AddServiceRegistrationsExtensions).Assembly.GetName().Name}.Auth",typeof(EmployerAccountPostAuthenticationClaimsHandler));
            }
            else
            {
                if (_configuration["StubAuth"] != null && _configuration["StubAuth"]
                        .Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    services.AddEmployerStubAuthentication();    
                }
                else
                {
                    var config = _configuration
                        .GetSection("Oidc")
                        .Get<IdentityServerConfiguration>();
                    services.AddAndConfigureEmployerAuthentication(config);
                    clientId = config.ClientId;
                }
                services.AddAuthenticationCookie();
            }

            services.AddMaMenuConfiguration(RouteNames.EmployerSignOut, clientId,_configuration["Environment"]);
            services.Configure<EmployerUrlsConfiguration>(_configuration.GetSection(nameof(EmployerUrlsConfiguration)));
            
            services.AddMediatR(typeof(FindProviderToAddQuery).Assembly);
            //todo add validation DI
            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

            services.Configure<RouteOptions>(options => { })
                .AddMvc(options =>
                {
                    if (!_configuration.IsDev())
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }

                })
                ;//todo .EnableGoogleAnalytics(); - this is provider ui code

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
            var conf = app.ApplicationServices.GetService<IOptions<EncodingConfig>>();

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
            app.UseUnitOfWork();

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
