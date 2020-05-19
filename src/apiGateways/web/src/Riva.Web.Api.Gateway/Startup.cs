using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MMLib.SwaggerForOcelot.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Infrastructure.Logger;

namespace Riva.Web.Api.Gateway
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<ILogger, Logger>()
                .AddSwaggerForOcelot(_config)
                .AddOcelot();
        }

        public void Configure(IApplicationBuilder app)
        {
            app
                .UseSwaggerForOcelotUI(SetupAction)
                .UseWebSockets()
                .UseOcelot();
        }

        private void SetupAction(SwaggerForOcelotUIOptions options)
        {
            options.PathToSwaggerGenerator = "/swagger/docs";
            options.OAuthClientId(_config.GetValue<string>("Swagger:ClientId"));
            options.OAuthAppName("Riva Identity");
        }
    }
}
