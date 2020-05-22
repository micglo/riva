using Microsoft.Extensions.DependencyInjection;

namespace Riva.SignalR.DependencyInjection
{
    public static class DependencyInjectionRegistrar
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            return services
                .RegisterServices()
                .RegisterIntegrationEventHandlers();
        }
    }
}