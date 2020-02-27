using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Riva.BuildingBlocks.Infrastructure.Models.Constants;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.Identity.Infrastructure.Services;
using Riva.Identity.Web.Configs;

namespace Riva.Identity.Web.ServiceCollectionExtensions
{
    public static class IdentityServerExtension
    {
        public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            var configSqlServer = SqlServerConfigurator.Configure(services, config, env);
            var builder = services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "~/auth/login";
                    options.UserInteraction.LogoutUrl = "~/auth/logout";
                    options.UserInteraction.ConsentUrl = "~/auth/consent";
                    options.UserInteraction.ErrorUrl = "~/error";
                    options.UserInteraction.DeviceVerificationUrl = "~/auth/deviceVerification";
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddConfigurationStore(options => options.ResolveDbContextOptions = configSqlServer)
                .AddOperationalStore(options => options.ResolveDbContextOptions = configSqlServer)
                .AddInMemoryCaching()
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            if(env.IsLocalOrDocker())
                builder.AddDeveloperSigningCredential();
            else
            {
                var keyVaultAppSettings = config.GetSectionAppSettings<KeyVaultAppSettings>(AppSettingsConstants.KeyVault);
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
                var certificateSecret =
                    keyVaultClient.GetSecretAsync($"https://{keyVaultAppSettings.Name}.vault.azure.net/",
                        keyVaultAppSettings.SigningCredentialCertificateName).GetAwaiter().GetResult();
                var privateKeyBytes = Convert.FromBase64String(certificateSecret.Value);
                var certificate = new X509Certificate2(privateKeyBytes);
                builder.AddSigningCredential(certificate);
            }

            return services;
        }

        public static IApplicationBuilder InitializeIdentityServerDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerConfig.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                var rivaApiResources = new List<BuildingBlocks.Infrastructure.Models.ApiResource>
                {
                    ApiResourcesConstants.RivaIdentityApiResource,
                    ApiResourcesConstants.RivaAdministrativeDivisionsApiResource,
                    ApiResourcesConstants.RivaAnnouncementsApiResource,
                    ApiResourcesConstants.RivaUsersApiResource,
                    ApiResourcesConstants.RivaSignalRApiResource
                };
                var apiResources = rivaApiResources.Select(x => new ApiScope(x.Name, x.DisplayName));
                foreach (var resource in apiResources)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                var rivaApiResources = new List<BuildingBlocks.Infrastructure.Models.ApiResource>
                {
                    ApiResourcesConstants.RivaIdentityApiResource,
                    ApiResourcesConstants.RivaAdministrativeDivisionsApiResource,
                    ApiResourcesConstants.RivaAnnouncementsApiResource,
                    ApiResourcesConstants.RivaUsersApiResource,
                    ApiResourcesConstants.RivaSignalRApiResource
                };
                var apiResources = rivaApiResources.Select(x => new ApiResource(x.Name, x.DisplayName)
                {
                    Scopes = { x.Name }
                });
                foreach (var resource in apiResources)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.Clients.Any())
            {
                var clientsAppSetting = serviceScope.ServiceProvider.GetRequiredService<IOptions<List<Client>>>();
                foreach (var client in clientsAppSetting.Value)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            return app;
        }
    }

    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}