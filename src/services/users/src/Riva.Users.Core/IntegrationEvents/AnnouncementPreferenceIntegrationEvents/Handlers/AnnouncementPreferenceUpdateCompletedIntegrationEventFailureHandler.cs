using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using Riva.Users.Core.Enums;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Core.IntegrationEvents.AnnouncementPreferenceIntegrationEvents.Handlers
{
    public class AnnouncementPreferenceUpdateCompletedIntegrationEventFailureHandler : IIntegrationEventHandler<AnnouncementPreferenceUpdateCompletedIntegrationEventFailure>
    {
        private readonly ILogger _logger;
        private readonly IUserRevertService _userRevertService;
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferenceUpdateCompletedIntegrationEventFailureHandler(ILogger logger, IUserRevertService userRevertService, IIntegrationEventBus integrationEventBus)
        {
            _logger = logger;
            _userRevertService = userRevertService;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(AnnouncementPreferenceUpdateCompletedIntegrationEventFailure integrationEvent, CancellationToken cancellationToken = default)
        {
            var announcementPreferenceName = integrationEvent.AnnouncementPreferenceType == AnnouncementPreferenceType.FlatForRentAnnouncementPreference
                ? nameof(FlatForRentAnnouncementPreference)
                : nameof(RoomForRentAnnouncementPreference);
            var message = $"Could not finish {announcementPreferenceName} update process.";
            _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                "userId={userId}, message={message}, reason={reason}, code={code}",
                integrationEvent.UserId, message, integrationEvent.Reason, integrationEvent.Code);

            var userAnnouncementPreferenceUpdateCompletedIntegrationEventFailure =
                new UserAnnouncementPreferenceUpdateCompletedIntegrationEventFailure(integrationEvent.CorrelationId,
                    integrationEvent.Code, integrationEvent.Reason, integrationEvent.UserId,
                    integrationEvent.AnnouncementPreferenceId, integrationEvent.AnnouncementPreferenceType);
            var userAnnouncementPreferenceUpdateCompletedIntegrationEventFailureTask =
                _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceUpdateCompletedIntegrationEventFailure);

            try
            {
                await _userRevertService.RevertUserAsync(integrationEvent.UserId, integrationEvent.CorrelationId);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                    "userId={userId}, message={message}, stackTrace={stackTrace}", integrationEvent.UserId,
                    e.Message, e.StackTrace);
            }

            await userAnnouncementPreferenceUpdateCompletedIntegrationEventFailureTask;
        }
    }
}