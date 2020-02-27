using Microsoft.Extensions.DependencyInjection;
using Riva.Identity.Core.Services;
using Riva.Identity.Infrastructure.Services;

namespace Riva.Identity.Web.Api.DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IRoleGetterService, RoleGetterService>()
                .AddScoped<IRoleVerificationService, RoleVerificationService>()
                .AddScoped<IAccountGetterService, AccountGetterService>()
                .AddScoped<IAccountVerificationService, AccountVerificationService>()
                .AddScoped<IAccountCreatorService, AccountCreatorService>()
                .AddSingleton<IPasswordService, PasswordService>()
                .AddSingleton<IAccountConfirmationRequestService, AccountConfirmationRequestService>()
                .AddSingleton<IPasswordResetTokenRequestService, PasswordResetTokenRequestService>()
                .AddScoped<IAccountDataConsistencyService, AccountDataConsistencyService>();
        }
    }
}