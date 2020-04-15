using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.Authorization.Policies;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;
using Riva.Users.Infrastructure.CoreAutoMapperProfiles;

namespace Riva.Users.Web.Api.ServiceCollectionExtensions
{
    public static class WebApiExtension
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            var authAppSettings = config.GetSectionAppSettings<AuthAppSettings>(AppSettingsConstants.Auth);
            var keyVaultAppSettings = config.GetSectionAppSettings<KeyVaultAppSettings>(AppSettingsConstants.KeyVault);
            var swaggerAppSettings = config.GetSectionAppSettings<SwaggerAppSettings>(AppSettingsConstants.Swagger);

            var authenticationExtensionConfig = new AuthenticationExtensionConfig(env,
                ApiResourcesConstants.RivaUsersApiResource.Name, authAppSettings.Authority,
                AuthenticationExtension.JwtBearerAuthenticationScheme, keyVaultAppSettings.Name,
                keyVaultAppSettings.SigningCredentialCertificateName);
            var authorizationExtensionConfig = new AuthorizationExtensionConfig(
                ApiResourcesConstants.RivaUsersApiResource.Name, ResourceOwnerPolicy.CreateResourceOwnerPolicy());
            var swaggerExtensionConfig = new SwaggerExtensionConfig(ApiResourcesConstants.RivaUsersApiResource.Name, swaggerAppSettings.IdentityUrl);
            var webApiExtensionConfig = new WebApiExtensionConfig(env, typeof(Startup).GetTypeInfo().Assembly,
                authorizationExtensionConfig, authenticationExtensionConfig, swaggerExtensionConfig,
                typeof(Startup).Assembly, typeof(UserProfile).Assembly);

            return services.AddWebApi(webApiExtensionConfig);
        }
    }
}