using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.Users.Infrastructure.DataAccess.RivaUsersSqlServer.Contexts;
using Riva.Users.Web.Api.Configs;
using Riva.Users.Web.Api.DependencyInjection;
using Riva.Users.Web.Api.ServiceCollectionExtensions;

namespace Riva.Users.Web.Api
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
                .AddHttpContextAccessor()
                .AddAppSettings(Config)
                .AddHealthChecks(Config)
                .AddIntegrationEventBus(Config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings).CentralServiceBusConnectionString,
                    Config.GetSectionAppSettings<CentralServiceBusAppSettings>(AppSettingsConstants.CentralServiceBus).SubscriptionName)
                .AddDependencies(Config)
                .AddDbContext<RivaUsersDbContext>(SqlServerConfigurator.Configure(services, Config, Env))
                .AddDomainEventsStore<RivaUsersDbContext>();
        }


        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseWebApi(env, ApiResourcesConstants.RivaUsersApiResource.Name,
                    new Guid(Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId))
                .UseIntegrationEventBus();

            if (env.IsLocalOrDocker())
            {
                var rivaUsersDbContext = app.ApplicationServices.GetRequiredService<RivaUsersDbContext>();
                rivaUsersDbContext.Database.Migrate();
            }
        }
    }
}
