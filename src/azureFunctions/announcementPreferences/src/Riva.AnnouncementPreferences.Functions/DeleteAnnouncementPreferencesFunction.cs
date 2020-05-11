using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.AnnouncementPreferences.Core.Services;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.ErrorMessages;
using Riva.BuildingBlocks.Core.Logger;

namespace Riva.AnnouncementPreferences.Functions
{
    public class DeleteAnnouncementPreferencesFunction
    {
        private readonly IFlatForRentAnnouncementPreferenceService _flatForRentAnnouncementPreferenceService;
        private readonly IRoomForRentAnnouncementPreferenceService _roomForRentAnnouncementPreferenceService;
        private readonly ILogger _logger;

        public DeleteAnnouncementPreferencesFunction(IFlatForRentAnnouncementPreferenceService flatForRentAnnouncementPreferenceService,
            IRoomForRentAnnouncementPreferenceService roomForRentAnnouncementPreferenceService, ILogger logger)
        {
            _flatForRentAnnouncementPreferenceService = flatForRentAnnouncementPreferenceService;
            _roomForRentAnnouncementPreferenceService = roomForRentAnnouncementPreferenceService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.DeleteAnnouncementPreferencesFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName,
                ConstantVariables.DeleteAnnouncementPreferencesSubscriptionName,
                Connection = ConstantVariables.ServiceBusConnectionStringName)]
            Message inputMessage,
            [ServiceBus(ConstantVariables.ServiceBusTopicName, Connection = ConstantVariables.ServiceBusConnectionStringName)]
            IAsyncCollector<Message> outcome)
        {
            var integrationEvent = JsonConvert.DeserializeObject<UserDeletedIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                await Task.WhenAll(
                    _flatForRentAnnouncementPreferenceService.DeleteAllByUserIdAsync(integrationEvent.UserId),
                    _roomForRentAnnouncementPreferenceService.DeleteAllByUserIdAsync(integrationEvent.UserId));
                var announcementPreferencesDeletionCompletedIntegrationEvent = new AnnouncementPreferencesDeletionCompletedIntegrationEvent(
                    integrationEvent.CorrelationId,
                    integrationEvent.UserId);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferencesDeletionCompletedIntegrationEvent)),
                    Label = announcementPreferencesDeletionCompletedIntegrationEvent.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferencesDeletionCompletedIntegrationEvent.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
                var announcementPreferencesDeletionCompletedIntegrationEventFailure = new AnnouncementPreferencesDeletionCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId,
                    IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError,
                    integrationEvent.UserId);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferencesDeletionCompletedIntegrationEventFailure)),
                    Label = announcementPreferencesDeletionCompletedIntegrationEventFailure.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferencesDeletionCompletedIntegrationEventFailure.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
        }
    }
}