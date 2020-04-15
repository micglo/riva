using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Riva.Users.Web.Api.DependencyInjection
{
    public static class DependencyInjectionRegistrar
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            return services
                .RegisterRepositories()
                .RegisterCommandHandlers()
                .RegisterQueryHandlers()
                .RegisterServices(config)
                .RegisterIntegrationEventHandlers()
                .RegisterDomainEventHandlers();
        }
    }
}