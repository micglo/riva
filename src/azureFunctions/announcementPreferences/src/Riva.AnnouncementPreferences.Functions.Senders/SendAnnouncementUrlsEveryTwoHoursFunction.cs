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
    public class SendAnnouncementUrlsEveryTwoHoursFunction
    {
        private readonly IAnnouncementUrlsSenderService _announcementUrlsSenderService;
        private readonly ILogger _logger;

        public SendAnnouncementUrlsEveryTwoHoursFunction(IAnnouncementUrlsSenderService announcementUrlsSenderService, ILogger logger)
        {
            _announcementUrlsSenderService = announcementUrlsSenderService;
            _logger = logger;
        }

        [FunctionName(ConstantVariables.SendAnnouncementUrlsEveryTwoHoursFunctionName)]
        public async Task Run([TimerTrigger(ConstantVariables.SendAnnouncementUrlsEveryTwoHoursCronExpr)]TimerInfo timerInfo)
        {
            try
            {
                await _announcementUrlsSenderService.SendAnnouncementUrlsAsync(AnnouncementSendingFrequency.EveryTwoHours);
            }
            catch (Exception e)
            {
                _logger.LogError(ServiceComponentEnumeration.RivaAnnouncementPreferences,
                    "message={message}, stackTrace={stackTrace}", e.Message, e.StackTrace);
            }
        }
    }
}
