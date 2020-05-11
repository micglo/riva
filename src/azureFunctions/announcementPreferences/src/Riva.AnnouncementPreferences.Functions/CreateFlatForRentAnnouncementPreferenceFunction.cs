using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.AnnouncementPreferences.Core.Services;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.ErrorMessages;
using Riva.BuildingBlocks.Core.Logger;

namespace Riva.AnnouncementPreferences.Functions
{
    public class CreateFlatForRentAnnouncementPreferenceFunction
    {
        private readonly IFlatForRentAnnouncementPreferenceService _flatForRentAnnouncementPreferenceService;
        private readonly ILogger _logger;

        public CreateFlatForRentAnnouncementPreferenceFunction(IFlatForRentAnnouncementPreferenceService flatForRentAnnouncementPreferenceService, ILogger logger)
        {
            _flatForRentAnnouncementPreferenceService = flatForRentAnnouncementPreferenceService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.CreateFlatForRentAnnouncementPreferenceFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName, 
                ConstantVariables.CreateFlatForRentAnnouncementPreferenceSubscriptionName, 
                Connection = ConstantVariables.ServiceBusConnectionStringName)] 
            Message inputMessage,
            [ServiceBus(ConstantVariables.ServiceBusTopicName, Connection = ConstantVariables.ServiceBusConnectionStringName)]
            IAsyncCollector<Message> outcome)
        {
            var integrationEvent = JsonConvert.DeserializeObject<UserFlatForRentAnnouncementPreferenceCreatedIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                await _flatForRentAnnouncementPreferenceService.AddAsync(integrationEvent);
                var announcementPreferenceCreationCompletedIntegrationEvent = new AnnouncementPreferenceCreationCompletedIntegrationEvent(
                    integrationEvent.CorrelationId, 
                    integrationEvent.UserId, 
                    integrationEvent.FlatForRentAnnouncementPreferenceId, 
                    AnnouncementPreferenceType.FlatForRentAnnouncementPreference);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferenceCreationCompletedIntegrationEvent)),
                    Label = announcementPreferenceCreationCompletedIntegrationEvent.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferenceCreationCompletedIntegrationEvent.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);

                var announcementPreferenceCreationCompletedIntegrationEventFailure = new AnnouncementPreferenceCreationCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId,
                    IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError,
                    integrationEvent.UserId,
                    integrationEvent.FlatForRentAnnouncementPreferenceId,
                    AnnouncementPreferenceType.FlatForRentAnnouncementPreference);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferenceCreationCompletedIntegrationEventFailure)),
                    Label = announcementPreferenceCreationCompletedIntegrationEventFailure.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferenceCreationCompletedIntegrationEventFailure.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
        }
    }
}
