using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Riva.Messages
{
    public class MessageBroadcasterFunction
    {
        private readonly ILogger _logger;
        private readonly ISendGridClient _sendGridClient;

        public MessageBroadcasterFunction(ILogger logger, ISendGridClient sendGridClient)
        {
            _logger = logger;
            _sendGridClient = sendGridClient;
        }

        [FunctionName(ConstantVariables.MessageBroadcasterFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(ConstantVariables.ServiceBusTopicName, ConstantVariables.MessageBroadcasterSubscriptionName, Connection = ConstantVariables.ServiceBusConnectionStringName)]
            Message inputMessage)
        {
            var integrationEvent = JsonConvert.DeserializeObject<MessageIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                var sendGridMessage = MailHelper.CreateSingleEmail(new EmailAddress(integrationEvent.From),
                    new EmailAddress(integrationEvent.To), integrationEvent.Subject, string.Empty,
                    integrationEvent.Body);
                await _sendGridClient.SendEmailAsync(sendGridMessage);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaMessages, integrationEvent,
                    "message={message}, stackTrace={stackTrace}", e.Message, e.StackTrace);
            }
        }
    }
}
