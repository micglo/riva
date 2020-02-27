using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;

namespace Riva.BuildingBlocks.Infrastructure.Communications
{
    public class IntegrationEventBus : IIntegrationEventBus
    {
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        private const string IntegrationEventSuffix = "IntegrationEvent";

        public IntegrationEventBus(IServiceBusPersisterConnection serviceBusPersisterConnection)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
        }

        public async Task PublishIntegrationEventAsync(IIntegrationEvent integrationEvent)
        {
            var eventName = integrationEvent.GetType().Name.Replace(IntegrationEventSuffix, "");
            var jsonMessage = JsonConvert.SerializeObject(integrationEvent);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
                CorrelationId = integrationEvent.CorrelationId.ToString()
            };

            var topicClient = _serviceBusPersisterConnection.CreateTopicClient();
            await topicClient.SendAsync(message);
        }
    }
}