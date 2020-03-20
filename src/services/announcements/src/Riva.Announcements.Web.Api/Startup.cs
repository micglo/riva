using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Web.Api.DependencyInjection;
using Riva.Announcements.Web.Api.ServiceCollectionExtensions;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.Announcements.Web.Api
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
                .AddHttpContextAccessor()
                .AddHealthChecks(Config)
                .AddCosmonaut(Config)
                .AddAppSettings(Config)
                .AddDependencies();
        }

        
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebApi(env, ApiResourcesConstants.RivaAnnouncementsApiResource.Name,
                new Guid(Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId));
        }
    }
}
