using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.WebApi.HostBuilderExtensions;
using Riva.Identity.Core.IntegrationEvents;

namespace Riva.Identity.Web.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await RegisterIntegrationEventSubscriptionsAsync(host);
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .AddAppConfiguration<Startup>()
                .AddLogging(typeof(Program), typeof(Startup))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }

        private static async Task RegisterIntegrationEventSubscriptionsAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var integrationEventBusSubscriptionsManager = scope.ServiceProvider.GetRequiredService<IIntegrationEventBusSubscriptionsManager>();
                await integrationEventBusSubscriptionsManager.AddSubscriptionRuleAsync<UserCreationCompletedIntegrationEvent>();
                await integrationEventBusSubscriptionsManager.AddSubscriptionRuleAsync<UserCreationCompletedIntegrationEventFailure>();
                await integrationEventBusSubscriptionsManager.AddSubscriptionRuleAsync<UserDeletionCompletedIntegrationEvent>();
                await integrationEventBusSubscriptionsManager.AddSubscriptionRuleAsync<UserDeletionCompletedIntegrationEventFailure>();
            }
        }
    }
}