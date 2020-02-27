using Microsoft.Extensions.DependencyInjection;
using Riva.Identity.Core.Interactors.ExternalLogin;
using Riva.Identity.Core.Interactors.LocalLogin;
using Riva.Identity.Core.Interactors.Logout;

namespace Riva.Identity.Web.DependencyInjection
{
    public static class InteractorsRegistrar
    {
        public static IServiceCollection RegisterInteractors(this IServiceCollection services)
        {
            return services
                .AddScoped<ILocalLoginInteractor, LocalLoginInteractor>()
                .AddScoped<IExternalLoginInteractor, ExternalLoginInteractor>()
                .AddScoped<ILogoutInteractor, LogoutInteractor>()
                .AddScoped<ILoggedOutInteractor, LoggedOutInteractor>();
        }
    }
}