using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.Authorization.Policies;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Infrastructure.CoreAutoMapperProfiles;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Web.Api.Configs;
using Riva.Identity.Web.Api.DependencyInjection;
using Riva.Identity.Web.Api.ServiceCollectionExtensions;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
{
    public abstract class IntegrationTestStartupBase : Startup
    {
        protected IntegrationTestStartupBase(IWebHostEnvironment env, IConfiguration config) : base(env, config)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var authAppSettings = Config.GetSectionAppSettings<AuthAppSettings>(AppSettingsConstants.Auth);
            var keyVaultAppSettings = Config.GetSectionAppSettings<KeyVaultAppSettings>(AppSettingsConstants.KeyVault);
            var swaggerAppSettings = Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger);

            var authenticationExtensionConfig = new AuthenticationExtensionConfig(Env,
                ApiResourcesConstants.RivaIdentityApiResource.Name, authAppSettings.Authority,
                AuthenticationExtension.JwtBearerAuthenticationScheme, keyVaultAppSettings.Name,
                keyVaultAppSettings.SigningCredentialCertificateName);
            var authorizationExtensionConfig = new AuthorizationExtensionConfig(
                ApiResourcesConstants.RivaIdentityApiResource.Name, ResourceOwnerPolicy.CreateResourceOwnerPolicy());
            var swaggerExtensionConfig = new SwaggerExtensionConfig(ApiResourcesConstants.RivaIdentityApiResource.Name, swaggerAppSettings.IdentityUrl);
            var webApiExtensionConfig = new WebApiExtensionConfig(Env, typeof(Startup).GetTypeInfo().Assembly,
                authorizationExtensionConfig, authenticationExtensionConfig, swaggerExtensionConfig,
                typeof(Startup).Assembly, typeof(AccountProfile).Assembly);

            services
                .AddWebApiForIntegrationTest(webApiExtensionConfig)
                .AddAuthorizationHandlers()
                .AddHealthChecks(Config)
                .AddAppSettings(Config)
                .AddIntegrationEventBus(Config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings).CentralServiceBusConnectionString,
                    Config.GetSectionAppSettings<CentralServiceBusAppSettings>(AppSettingsConstants.CentralServiceBus).SubscriptionName)
                .AddDependencies()
                .AddDbContext<RivaIdentityDbContext>(SqlServerConfigurator.Configure(services, Config, Env))
                .AddDomainEventsStore<RivaIdentityDbContext>()
                .AddHttpContextAccessor();
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseExceptionHandler(WebApiPipeline.ErrorHandlingPath)
                .UseStatusCodePagesWithReExecute(WebApiPipeline.StatusCodeErrorHandlingPath)
                .UseHsts()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(CorsExtension.AllowAnyCorsPolicyName)
                .UseApiVersioning()
                .UseHealthChecks()
                .UseSwagger(ApiResourcesConstants.RivaIdentityApiResource.Name,
                    new Guid(Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId));

            UseAuthorizeMiddleware(app);
            
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

        protected abstract void UseAuthorizeMiddleware(IApplicationBuilder app);
    }

    public class AdministratorIntegrationTestStartup : IntegrationTestStartupBase
    {
        public AdministratorIntegrationTestStartup(IWebHostEnvironment env, IConfiguration config) : base(env, config)
        {
        }

        protected override void UseAuthorizeMiddleware(IApplicationBuilder app)
        {
            app
                .UseMiddleware<AdministratorIntegrationTestAuthorizeMiddleware>()
                .UseAuthorization();
        }
    }

    public class AccountIntegrationTestStartup : IntegrationTestStartupBase
    {
        public AccountIntegrationTestStartup(IWebHostEnvironment env, IConfiguration config) : base(env, config)
        {
        }

        protected override void UseAuthorizeMiddleware(IApplicationBuilder app)
        {
            app
                .UseMiddleware<AccountIntegrationTestAuthorizeMiddleware>()
                .UseAuthorization();
        }
    }

    public class AnonymousIntegrationTestStartup : IntegrationTestStartupBase
    {
        public AnonymousIntegrationTestStartup(IWebHostEnvironment env, IConfiguration config) : base(env, config)
        {
        }

        protected override void UseAuthorizeMiddleware(IApplicationBuilder app)
        {
            app.UseAuthorization();
        }
    }
}