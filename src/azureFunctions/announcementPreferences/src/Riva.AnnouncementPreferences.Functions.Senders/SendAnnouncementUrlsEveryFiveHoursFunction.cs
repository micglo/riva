using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Riva.AnnouncementPreferences.Core.Constants;
using Riva.AnnouncementPreferences.Core.Enums;
using Riva.AnnouncementPreferences.Core.Services;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;

namespace Riva.AnnouncementPreferences.Functions.Senders
{
    public class SendAnnouncementUrlsEveryFiveHoursFunction
    {
        private readonly IAnnouncementUrlsSenderService _announcementUrlsSenderService;
        private readonly ILogger _logger;

        public SendAnnouncementUrlsEveryFiveHoursFunction(IAnnouncementUrlsSenderService announcementUrlsSenderService, ILogger logger)
        {
            _announcementUrlsSenderService = announcementUrlsSenderService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.SendAnnouncementUrlsEveryFiveHoursFunctionName)]
        public async Task Run([TimerTrigger(ConstantVariables.SendAnnouncementUrlsEveryFiveHoursCronExpr)]TimerInfo timerInfo)
        {
            try
            {
                await _announcementUrlsSenderService.SendAnnouncementUrlsAsync(AnnouncementSendingFrequency.EveryFiveHours);
            }
            catch (Exception e)
            {
                _logger.LogError(ServiceComponentEnumeration.RivaAnnouncementPreferences,
                    "message={message}, stackTrace={stackTrace}", e.Message, e.StackTrace);
            }
        }
    }
}
