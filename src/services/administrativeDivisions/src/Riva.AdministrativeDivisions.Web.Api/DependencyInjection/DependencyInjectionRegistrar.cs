using Microsoft.Extensions.DependencyInjection;

namespace Riva.AdministrativeDivisions.Web.Api.DependencyInjection
{
    public static class DependencyInjectionRegistrar
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            return services
                .RegisterRepositories()
                .RegisterCommandHandlers()
                .RegisterQueryHandlers()
                .RegisterServices();
        }
    }
}