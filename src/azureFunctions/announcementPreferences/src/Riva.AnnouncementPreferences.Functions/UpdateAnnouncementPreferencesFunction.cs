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
    public class UpdateAnnouncementPreferencesFunction
    {
        private readonly IFlatForRentAnnouncementPreferenceService _flatForRentAnnouncementPreferenceService;
        private readonly IRoomForRentAnnouncementPreferenceService _roomForRentAnnouncementPreferenceService;
        private readonly ILogger _logger;

        public UpdateAnnouncementPreferencesFunction(IFlatForRentAnnouncementPreferenceService flatForRentAnnouncementPreferenceService,
            IRoomForRentAnnouncementPreferenceService roomForRentAnnouncementPreferenceService, ILogger logger)
        {
            _flatForRentAnnouncementPreferenceService = flatForRentAnnouncementPreferenceService;
            _roomForRentAnnouncementPreferenceService = roomForRentAnnouncementPreferenceService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.UpdateAnnouncementPreferencesFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName,
                ConstantVariables.UpdateAnnouncementPreferencesSubscriptionName,
                Connection = ConstantVariables.ServiceBusConnectionStringName)]
            Message inputMessage,
            [ServiceBus(ConstantVariables.ServiceBusTopicName, Connection = ConstantVariables.ServiceBusConnectionStringName)]
            IAsyncCollector<Message> outcome)
        {
            var integrationEvent = JsonConvert.DeserializeObject<UserUpdatedIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                await Task.WhenAll(_flatForRentAnnouncementPreferenceService.UpdateAsync(integrationEvent), _roomForRentAnnouncementPreferenceService.UpdateAsync(integrationEvent));
                var announcementPreferencesUpdateCompletedIntegrationEvent = new AnnouncementPreferencesUpdateCompletedIntegrationEvent(
                    integrationEvent.CorrelationId,
                    integrationEvent.UserId);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferencesUpdateCompletedIntegrationEvent)),
                    Label = announcementPreferencesUpdateCompletedIntegrationEvent.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferencesUpdateCompletedIntegrationEvent.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
                var announcementPreferencesUpdateCompletedIntegrationEventFailure = new AnnouncementPreferencesUpdateCompletedIntegrationEventFailure(
                    integrationEvent.CorrelationId,
                    IntegrationEventErrorCodeEnumeration.UnexpectedError.DisplayName,
                    IntegrationEventErrorMessage.UnexpectedError,
                    integrationEvent.UserId);
                var outputMessage = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcementPreferencesUpdateCompletedIntegrationEventFailure)),
                    Label = announcementPreferencesUpdateCompletedIntegrationEventFailure.GetType().Name.Replace(ConstantVariables.IntegrationEventSuffix, ""),
                    CorrelationId = announcementPreferencesUpdateCompletedIntegrationEventFailure.CorrelationId.ToString()
                };
                await outcome.AddAsync(outputMessage);
            }
        }
    }
}