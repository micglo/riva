using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.SignalR.DependencyInjection;
using Riva.SignalR.Hubs;
using Riva.SignalR.ServiceCollectionExtensions;

namespace Riva.SignalR
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddWebApi(_config, _env)
                .AddHealthChecks(_config)
                .AddIntegrationEventBus(_config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings).CentralServiceBusConnectionString,
                    _config.GetSectionAppSettings<CentralServiceBusAppSettings>(AppSettingsConstants.CentralServiceBus).SubscriptionName)
                .AddDependencies()
                .AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseWebApi(_env, ApiResourcesConstants.RivaSignalRApiResource.Name,
                    new Guid(_config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId))
                .UseEndpoints(endpoints => endpoints.MapHub<RivaHub>($"/{SignalRHubNameConstants.RivaHub}"))
                .UseIntegrationEventBus();
        }
    }
}
