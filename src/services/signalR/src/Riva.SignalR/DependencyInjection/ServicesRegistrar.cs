using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.Core.Logger;
using Riva.BuildingBlocks.Infrastructure.Logger;
using Riva.SignalR.Services;

namespace Riva.SignalR.DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHubWrapperService, HubWrapperService>()
                .AddSingleton<IHubService, HubService>()
                .AddSingleton<ILogger, Logger>();
        }
    }
}