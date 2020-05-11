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
    public class MatchFlatForRentAnnouncementsFunction
    {
        private readonly IFlatForRentAnnouncementPreferenceMatchService _flatForRentAnnouncementPreferenceMatchService;
        private readonly ILogger _logger;

        public MatchFlatForRentAnnouncementsFunction(IFlatForRentAnnouncementPreferenceMatchService flatForRentAnnouncementPreferenceMatchService, ILogger logger)
        {
            _flatForRentAnnouncementPreferenceMatchService = flatForRentAnnouncementPreferenceMatchService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.MatchFlatForRentAnnouncementsFunctionName)]
        public async Task Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName,
                ConstantVariables.MatchFlatForRentAnnouncementsSubscriptionName,
                Connection = ConstantVariables.ServiceBusConnectionStringName)]
            Message inputMessage)
        {
            var integrationEvent = JsonConvert.DeserializeObject<FlatForRentAnnouncementsIntegrationEvent>(Encoding.UTF8.GetString(inputMessage.Body));

            try
            {
                await _flatForRentAnnouncementPreferenceMatchService.MatchAnnouncementsToPreferencesAsync(integrationEvent);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaAnnouncementPreferences, integrationEvent,
                    "message={message}, stackTrace={stackTrace}", e.Message, e.StackTrace);
            }
        }
    }
}