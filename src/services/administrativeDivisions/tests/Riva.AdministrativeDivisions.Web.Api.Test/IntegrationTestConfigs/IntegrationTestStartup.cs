using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Infrastructure.CoreAutoMapperProfiles;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Web.Api.Configs;
using Riva.AdministrativeDivisions.Web.Api.DependencyInjection;
using Riva.AdministrativeDivisions.Web.Api.ServiceCollectionExtensions;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ApplicationPipelines;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs
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
                ApiResourcesConstants.RivaAdministrativeDivisionsApiResource.Name, authAppSettings.Authority,
                AuthenticationExtension.JwtBearerAuthenticationScheme, keyVaultAppSettings.Name,
                keyVaultAppSettings.SigningCredentialCertificateName);
            var authorizationExtensionConfig = new AuthorizationExtensionConfig(ApiResourcesConstants.RivaAdministrativeDivisionsApiResource.Name);
            var swaggerExtensionConfig = new SwaggerExtensionConfig(ApiResourcesConstants.RivaAdministrativeDivisionsApiResource.Name, swaggerAppSettings.IdentityUrl);
            var webApiExtensionConfig = new WebApiExtensionConfig(Env, typeof(Startup).GetTypeInfo().Assembly,
                authorizationExtensionConfig, authenticationExtensionConfig, swaggerExtensionConfig,
                typeof(Startup).Assembly, typeof(StateProfile).Assembly);

            services
                .AddWebApiForIntegrationTest(webApiExtensionConfig)
                .AddHealthChecks(Config)
                .AddMemoryCache()
                .AddDependencies()
                .AddDbContext<RivaAdministrativeDivisionsDbContext>(SqlServerConfigurator.Configure(services, Config, Env));
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
                .UseSwagger(
                    ApiResourcesConstants.RivaAdministrativeDivisionsApiResource.Name,
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