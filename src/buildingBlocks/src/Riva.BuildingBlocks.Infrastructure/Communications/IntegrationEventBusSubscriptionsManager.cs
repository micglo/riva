using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Logger;

namespace Riva.BuildingBlocks.Infrastructure.Communications
{
    public class IntegrationEventBusSubscriptionsManager : IIntegrationEventBusSubscriptionsManager
    {
        private readonly SubscriptionClient _subscriptionClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly Dictionary<string, Type> _integrationEventTypes;
        private const string IntegrationEventSuffix = "IntegrationEvent";

        public IntegrationEventBusSubscriptionsManager(IServiceBusPersisterConnection serviceBusPersisterConnection,
            IServiceProvider serviceProvider, ILogger logger, string subscriptionName)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder,
                subscriptionName);
            _integrationEventTypes = new Dictionary<string, Type>();
        }

        public async Task RemoveDefaultSubscriptionRuleAsync()
        {
            var rules = await _subscriptionClient.GetRulesAsync();
            var ruleDescription = rules.SingleOrDefault(x => x.Name.Equals(RuleDescription.DefaultRuleName));
            if (ruleDescription != null)
                await _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName);
        }

        public void RegisterSubscriptionMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var messageData = Encoding.UTF8.GetString(message.Body);

                    if (await ProcessEvent(message.Label, messageData))
                    {
                        await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        public async Task AddSubscriptionRuleAsync<TIntegrationEvent>() 
            where TIntegrationEvent : IIntegrationEvent
        {
            var integrationEventType = typeof(TIntegrationEvent);
            var integrationEventName = integrationEventType.Name.Replace(IntegrationEventSuffix, "");
            var rules = await _subscriptionClient.GetRulesAsync();
            var ruleDescription = rules.SingleOrDefault(x => x.Name.Equals(integrationEventName));
            if (ruleDescription is null)
                await _subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { Label = integrationEventName },
                    Name = integrationEventName
                });

            if(!HasSubscriptionsForIntegrationEvent(integrationEventName))
                _integrationEventTypes.Add(integrationEventName, integrationEventType);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

            return Task.CompletedTask;
        }

        private async Task<bool> ProcessEvent(string integrationEventName, string message)
        {
            if (!HasSubscriptionsForIntegrationEvent(integrationEventName)) 
                return false;

            var integrationEventType = GetIntegrationEventTypeByName(integrationEventName);
            var integrationEvent = JsonConvert.DeserializeObject(message, integrationEventType);

            using (var scope = _serviceProvider.CreateScope())
            {
                var communicationBus = scope.ServiceProvider.GetRequiredService<ICommunicationBus>();
                await communicationBus.PublishIntegrationEventAsync((IIntegrationEvent)integrationEvent);
            }
            
            return true;
        }

        private bool HasSubscriptionsForIntegrationEvent(string integrationEventName)
        {
            return _integrationEventTypes.ContainsKey(integrationEventName);
        }

        private Type GetIntegrationEventTypeByName(string integrationEventName)
        {
            return _integrationEventTypes.GetValueOrDefault(integrationEventName);
        }
    }
}