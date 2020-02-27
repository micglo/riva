using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.Configs;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class AuthenticationExtension
    {
        public const string JwtBearerAuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            AuthenticationExtensionConfig config)
        {
            return services
                .AddAuthentication(config.DefaultScheme)
                .AddJwtBearer(JwtBearerAuthenticationScheme, options =>
                {
                    options.Authority = config.Authority;
                    options.RequireHttpsMetadata = config.Environment.IsNotLocalOrDocker();
                    options.Audience = config.Audience;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ValidateIssuerSigningKey = config.Environment.IsNotLocalOrDocker(),
                        ValidAudience = config.Audience,
                        ValidIssuer = config.Authority,
                        AuthenticationType = JwtBearerAuthenticationScheme
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments($"/{SignalRHubNameConstants.RivaHub}"))
                                context.Token = accessToken;

                            return Task.CompletedTask;
                        }
                    };

                    if (config.Environment.IsNotLocalOrDocker())
                    {
                        var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
                        var certificateBundle =
                            kv.GetCertificateAsync($"https://{config.KeyVaultName}.vault.azure.net/",
                                config.SigningCredentialCertificateName).GetAwaiter().GetResult();
                        var certificate = new X509Certificate2(certificateBundle.Cer);
                        options.TokenValidationParameters.IssuerSigningKey = new X509SecurityKey(certificate);
                    }
                })
                .Services;
        }
    }
}