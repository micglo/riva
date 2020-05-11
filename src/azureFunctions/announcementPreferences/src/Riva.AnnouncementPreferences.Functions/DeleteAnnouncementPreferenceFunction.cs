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
    public class DeleteAnnouncementPreferenceFunction
    {
        private readonly IFlatForRentAnnouncementPreferenceService _flatForRentAnnouncementPreferenceService;
        private readonly IRoomForRentAnnouncementPreferenceService _roomForRentAnnouncementPreferenceService;
        private readonly ILogger _logger;

        public DeleteAnnouncementPreferenceFunction(IFlatForRentAnnouncementPreferenceService flatForRentAnnouncementPreferenceService, 
            IRoomForRentAnnouncementPreferenceService roomForRentAnnouncementPreferenceService, ILogger logger)
        {
            _flatForRentAnnouncementPreferenceService = flatForRentAnnouncementPreferenceService;
            _roomForRentAnnouncementPreferenceService = roomForRentAnnouncementPreferenceService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.DeleteAnnouncementPreferenceFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName, 
                ConstantVariables.DeleteAnnouncementPreferenceSubscriptionName, 
                Connection = ConstantVariables.ServiceBusConnectionStringName)] 
            Message inputMessage,
            [ServiceBus(ConstantVariables.ServiceBusTopicName, Connection = ConstantVariables.ServiceBusConnectionStringName)]
            IAsyncCollector<Message> outcome)
        {
            var integrationEvent = JsonConvert.DeserializeObject<UserAnnouncementPreferenceDeletedIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                if(integrationEvent.AnnouncementPreferenceType == AnnouncementPreferenceType.FlatForRentAnnouncementPreference)
                    await _flatForRentAnnouncementPreferenceService.DeleteByIdAsync(integrationEvent.AnnouncementPreferenceId);
                else
                    await _roomForRentAnnouncementPreferenceService.DeleteByIdAsync(integrationEvent.AnnouncementPreferenceId);
                var announcementPreferenceDeletionCompletedIntegrationEvent = new AnnouncementPreferenceDeletionCompletedIntegrationEvent(
                    integrationEvent.CorrelationId, 
                    integrationEvent.UserId, 
                    integrationEvent.AnnouncementPreferenceId, 
                    integrationEvent.AnnouncementPreferenceType);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferenceDeletionCompletedIntegrationEvent)),
                    Label = announcementPreferenceDeletionCompletedIntegrationEvent.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferenceDeletionCompletedIntegrationEvent.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
                var announcementPreferenceDeletionCompletedIntegrationEventFailure = new AnnouncementPreferenceDeletionCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId,
                    IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError,
                    integrationEvent.UserId,
                    integrationEvent.AnnouncementPreferenceId,
                    AnnouncementPreferenceType.FlatForRentAnnouncementPreference);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferenceDeletionCompletedIntegrationEventFailure)),
                    Label = announcementPreferenceDeletionCompletedIntegrationEventFailure.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferenceDeletionCompletedIntegrationEventFailure.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
        }
    }
}
