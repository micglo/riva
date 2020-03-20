using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Infrastructure.CoreAutoMapperProfiles;
using Riva.Announcements.Web.Api.DependencyInjection;
using Riva.Announcements.Web.Api.ServiceCollectionExtensions;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
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
                ApiResourcesConstants.RivaAnnouncementsApiResource.Name, authAppSettings.Authority,
                AuthenticationExtension.JwtBearerAuthenticationScheme, keyVaultAppSettings.Name,
                keyVaultAppSettings.SigningCredentialCertificateName);
            var authorizationExtensionConfig = new AuthorizationExtensionConfig(ApiResourcesConstants.RivaAnnouncementsApiResource.Name);
            var swaggerExtensionConfig = new SwaggerExtensionConfig(ApiResourcesConstants.RivaAnnouncementsApiResource.Name, swaggerAppSettings.IdentityUrl);
            var webApiExtensionConfig = new WebApiExtensionConfig(Env, typeof(Startup).GetTypeInfo().Assembly,
                authorizationExtensionConfig, authenticationExtensionConfig, swaggerExtensionConfig,
                typeof(Startup).Assembly, typeof(RoomForRentAnnouncementProfile).Assembly);

            services
                .AddWebApiForIntegrationTest(webApiExtensionConfig)
                .AddHttpContextAccessor()
                .AddHealthChecks(Config)
                .AddCosmonaut(Config)
                .AddAppSettings(Config)
                .AddDependencies();
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
                .UseSwagger(ApiResourcesConstants.RivaAnnouncementsApiResource.Name,
                    new Guid(Config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger).ClientId));
            UseAuthorizeMiddleware(app);
            app.UseEndpoints(endpoints => endpoints.MapControllers());
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

    public class UserIntegrationTestStartup : IntegrationTestStartupBase
    {
        public UserIntegrationTestStartup(IWebHostEnvironment env, IConfiguration config) : base(env, config)
        {
        }

        protected override void UseAuthorizeMiddleware(IApplicationBuilder app)
        {
            app
                .UseMiddleware<UserIntegrationTestAuthorizeMiddleware>()
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