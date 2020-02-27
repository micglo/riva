using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Riva.Identity.Web.DependencyInjection;
using Riva.Identity.Web.ServiceCollectionExtensions;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.Identity.Infrastructure.CoreAutoMapperProfiles;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Web.Configs;
using Riva.Identity.Web.Models.AppSettings;

namespace Riva.Identity.Web
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
                .AddWebApp(Env)
                .AddIdentityServer(Config, Env)
                .AddAuthentication(Config)
                .AddHealthChecks(Config)
                .AddAppSettings(Config, Env)
                .AddIntegrationEventBus(Config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings).CentralServiceBusConnectionString)
                .AddDependencies()
                .AddDbContext<RivaIdentityDbContext>(SqlServerConfigurator.Configure(services, Config, Env))
                .AddDomainEventsStore<RivaIdentityDbContext>()
                .AddAutoMapper(typeof(Startup).Assembly, typeof(AuthProfile).Assembly)
                .AddCommunicationBus(typeof(Startup).GetTypeInfo().Assembly);
        }
        
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsLocalOrDocker())
                app.UseDeveloperExceptionPage();
            else
                app
                    .UseExceptionHandler("/error")
                    .UseStatusCodePagesWithReExecute("/error/{0}")
                    .UseHsts()
                    .UseHttpsRedirection();

            app
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseRouting()
                .UseHealthChecks()
                .UseIdentityServer()
                .UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            if (!env.IsProduction())
                app.InitializeIdentityServerDatabase();

            if (env.IsLocalOrDocker())
            {
                var rivaIdentityDbContext = app.ApplicationServices.GetRequiredService<RivaIdentityDbContext>();
                rivaIdentityDbContext.Database.EnsureCreated();
                rivaIdentityDbContext.Database.Migrate();
            }
        }
    }
}
