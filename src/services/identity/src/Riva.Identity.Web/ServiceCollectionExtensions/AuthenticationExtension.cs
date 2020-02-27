using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.Constants;
using Riva.Identity.Web.Models.AppSettings;

namespace Riva.Identity.Web.ServiceCollectionExtensions
{
    public static class AuthenticationExtension
    {
        public const string GoogleAuthScheme = "Google";
        public const string FacebookAuthScheme = "Facebook";

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var authAppSettings = config.GetSectionAppSettings<AuthAppSettings>(AppSettingsConstants.Auth);
            return services
                .AddAuthentication()
                .AddGoogle(GoogleAuthScheme, options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.ClientId = authAppSettings.GoogleClientId;
                    options.ClientSecret = authAppSettings.GoogleClientSecret;
                    options.Scope.Add("profile");
                    options.Events.OnCreatingTicket = context =>
                    {
                        var picture = context.User.GetProperty("picture").GetString();
                        context.Identity.AddClaim(new Claim(JwtClaimTypes.Picture, picture));
                        return Task.CompletedTask;
                    };
                })
                .AddFacebook(FacebookAuthScheme, options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.AppId = authAppSettings.FacebookAppId;
                    options.AppSecret = authAppSettings.FacebookAppSecret;
                    options.Fields.Add("picture");
                    options.Events.OnCreatingTicket = context =>
                    {
                        var profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url")
                            .ToString();
                        context.Identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                        return Task.CompletedTask;
                    };
                })
                .Services;
        }
    }
}