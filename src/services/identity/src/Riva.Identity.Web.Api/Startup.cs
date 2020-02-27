using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.Identity.Web.Api.DependencyInjection;
using Riva.Identity.Web.Api.ServiceCollectionExtensions;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Web.Api.Configs;

namespace Riva.Identity.Web.Api
{
    public class Startup
    {
        protected readonly IConfiguration Config;
        protected readonly IWebHostEnvironment Env;

        public Startup(IWebHostEnvironment env, IConfiguration config)
        {
            Config = config;
            Env = env;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddWebApi(Config, Env)
                .AddAuthorizationHandlers()
                .AddHealthChecks(Config)
                .AddAppSettings(Config)
                .AddIntegrationEventBus(Config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings).CentralServiceBusConnectionString,
                    Config.GetSectionAppSettings<CentralServiceBusAppSettings>(AppSettingsConstants.CentralServiceBus).SubscriptionName)
                .AddDependencies()
                .AddDbContext<RivaIdentityDbContext>(SqlServerConfigurator.Configure(services, Config, Env))
                .AddScoped(sp => new OperationalStoreOptions { ResolveDbContextOptions = SqlServerConfigurator.Configure(services, Config, Env) })
                .AddDbContext<PersistedGrantDbContext>(SqlServerConfigurator.Configure(services, Config, Env))
                .AddDomainEventsStore<RivaIdentityDbContext>();
        }
        
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseWebApi(env, ApiResourcesConstants.RivaIdentityApiResource.Name,
                    new Guid(Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId))
                .UseIntegrationEventBus();
        }
    }
}
