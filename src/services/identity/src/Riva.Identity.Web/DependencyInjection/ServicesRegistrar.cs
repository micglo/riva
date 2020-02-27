using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.Infrastructure.Logger;
using Riva.BuildingBlocks.Infrastructure.Mapper;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;

namespace Riva.Identity.Web.DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAccountGetterService, AccountGetterService>()
                .AddScoped<IAccountVerificationService, AccountVerificationService>()
                .AddScoped<IAccountProviderService, AccountProviderService>()
                .AddScoped<IAccountCreatorService, AccountCreatorService>()
                .AddScoped<IPasswordService, PasswordService>()
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<ISchemeService, SchemeService>()
                .AddScoped<IClaimsPrincipalService, ClaimsPrincipalService>()
                .AddScoped<ILogoutService, LogoutService>()
                .AddScoped<IAccountClaimsCreatorService, AccountClaimsCreatorService>()
                .AddScoped<ISignOutService, SignOutService>()
                .AddScoped<ISignInService, SignInService>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddScoped<IEventSink, IdentityServerEventSink>()
                .AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, CookieOptionsService>()
                .AddSingleton<IMapper, Mapper>()
                .AddSingleton<ILogger, Logger>()
                .AddSingleton(typeof(IOrderByExpressionCreator<>), typeof(OrderByExpressionCreator<>));
        }
    }
}