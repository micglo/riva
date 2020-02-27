using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.BuildingBlocks.Core.Communications
{
    public interface IIntegrationEventBusSubscriptionsManager
    {
        Task RemoveDefaultSubscriptionRuleAsync();

        void RegisterSubscriptionMessageHandler();

        Task AddSubscriptionRuleAsync<TIntegrationEvent>() where TIntegrationEvent : IIntegrationEvent;
    }
}