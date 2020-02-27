using Microsoft.Extensions.DependencyInjection;

namespace Riva.Identity.Web.DependencyInjection
{
    public static class DependencyInjectionRegistrar
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            return services
                .RegisterRepositories()
                .RegisterInteractors()
                .RegisterServices()
                .RegisterDomainEventHandlers();
        }
    }
}