using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Web.Api.Configs;
using Riva.AdministrativeDivisions.Web.Api.DependencyInjection;
using Riva.AdministrativeDivisions.Web.Api.ServiceCollectionExtensions;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.AdministrativeDivisions.Web.Api
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
                .AddHealthChecks(Config)
                .AddMemoryCache()
                .AddDependencies()
                .AddDbContext<RivaAdministrativeDivisionsDbContext>(SqlServerConfigurator.Configure(services, Config, Env));
        }

        
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebApi(env, ApiResourcesConstants.RivaAdministrativeDivisionsApiResource.Name,
                new Guid(Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId));

            if (env.IsLocalOrDocker())
            {
                var rivaAdministrativeDivisionsDbContext = app.ApplicationServices.GetRequiredService<RivaAdministrativeDivisionsDbContext>();
                rivaAdministrativeDivisionsDbContext.Database.Migrate();
            }
        }
    }
}
