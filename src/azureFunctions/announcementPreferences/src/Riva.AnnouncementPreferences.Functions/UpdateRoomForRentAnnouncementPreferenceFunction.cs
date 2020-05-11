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
    public class UpdateRoomForRentAnnouncementPreferenceFunction
    {
        private readonly IRoomForRentAnnouncementPreferenceService _roomForRentAnnouncementPreferenceService;
        private readonly ILogger _logger;

        public UpdateRoomForRentAnnouncementPreferenceFunction(IRoomForRentAnnouncementPreferenceService roomForRentAnnouncementPreferenceService, ILogger logger)
        {
            _roomForRentAnnouncementPreferenceService = roomForRentAnnouncementPreferenceService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.UpdateRoomForRentAnnouncementPreferenceFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName, 
                ConstantVariables.UpdateRoomForRentAnnouncementPreferenceSubscriptionName, 
                Connection = ConstantVariables.ServiceBusConnectionStringName)] 
            Message inputMessage,
            [ServiceBus(ConstantVariables.ServiceBusTopicName, Connection = ConstantVariables.ServiceBusConnectionStringName)]
            IAsyncCollector<Message> outcome)
        {
            var integrationEvent = JsonConvert.DeserializeObject<UserRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                await _roomForRentAnnouncementPreferenceService.UpdateAsync(integrationEvent);
                var announcementPreferenceUpdateCompletedIntegrationEvent = new AnnouncementPreferenceUpdateCompletedIntegrationEvent(
                    integrationEvent.CorrelationId, 
                    integrationEvent.UserId, 
                    integrationEvent.RoomForRentAnnouncementPreferenceId, 
                    AnnouncementPreferenceType.RoomForRentAnnouncementPreference);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferenceUpdateCompletedIntegrationEvent)),
                    Label = announcementPreferenceUpdateCompletedIntegrationEvent.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferenceUpdateCompletedIntegrationEvent.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
                var announcementPreferenceUpdateCompletedIntegrationEventFailure = new AnnouncementPreferenceUpdateCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId,
                    IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError,
                    integrationEvent.UserId,
                    integrationEvent.RoomForRentAnnouncementPreferenceId,
                    AnnouncementPreferenceType.RoomForRentAnnouncementPreference);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferenceUpdateCompletedIntegrationEventFailure)),
                    Label = announcementPreferenceUpdateCompletedIntegrationEventFailure.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferenceUpdateCompletedIntegrationEventFailure.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
        }
    }
}
