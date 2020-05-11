using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.IntegrationEvents.AnnouncementIntegrationEvents;
using Riva.AnnouncementPreferences.Core.Services;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;

namespace Riva.AnnouncementPreferences.Functions.Matchers
{
    public class MatchRoomForRentAnnouncementsFunction
    {
        private readonly IRoomForRentAnnouncementPreferenceMatchService _roomForRentAnnouncementPreferenceMatchService;
        private readonly ILogger _logger;

        public MatchRoomForRentAnnouncementsFunction(IRoomForRentAnnouncementPreferenceMatchService roomForRentAnnouncementPreferenceMatchService, ILogger logger)
        {
            _roomForRentAnnouncementPreferenceMatchService = roomForRentAnnouncementPreferenceMatchService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.MatchRoomForRentAnnouncementsFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName,
                ConstantVariables.MatchRoomForRentAnnouncementsSubscriptionName,
                Connection = ConstantVariables.ServiceBusConnectionStringName)]
            Message inputMessage)
        {
            var integrationEvent = JsonConvert.DeserializeObject<RoomForRentAnnouncementsIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                await _roomForRentAnnouncementPreferenceMatchService.MatchAnnouncementsToPreferencesAsync(integrationEvent);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "message={message}, stackTrace={stackTrace}", e.Message, e.StackTrace);
            }
        }
    }
}