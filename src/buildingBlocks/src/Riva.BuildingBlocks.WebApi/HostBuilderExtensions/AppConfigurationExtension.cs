using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.BuildingBlocks.WebApi.HostBuilderExtensions
{
    public static class AppConfigurationExtension
    {
        public static IHostBuilder AddAppConfiguration<TStartup>(this IHostBuilder builder) where TStartup : class
        {
            return builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                configurationBuilder
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();

                var config = configurationBuilder.Build();

                if (hostingContext.HostingEnvironment.IsLocal())
                    configurationBuilder.AddUserSecrets<TStartup>();

                if (hostingContext.HostingEnvironment.IsNotLocalOrDocker())
                {
                    var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
                    configurationBuilder.AddAzureKeyVault(
                        $"https://{config.GetSectionAppSettings<KeyVaultAppSettings>(AppSettingsConstants.KeyVault).Name}.vault.azure.net/",
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());
                }
            });
        }
    }
}