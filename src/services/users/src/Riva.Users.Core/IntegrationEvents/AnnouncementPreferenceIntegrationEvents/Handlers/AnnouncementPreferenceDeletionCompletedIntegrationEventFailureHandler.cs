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
    public class AnnouncementPreferenceDeletionCompletedIntegrationEventFailureHandler : IIntegrationEventHandler<AnnouncementPreferenceDeletionCompletedIntegrationEventFailure>
    {
        private readonly ILogger _logger;
        private readonly IUserRevertService _userRevertService;
        private readonly IIntegrationEventBus _integrationEventBus;

        public AnnouncementPreferenceDeletionCompletedIntegrationEventFailureHandler(ILogger logger, IUserRevertService userRevertService, IIntegrationEventBus integrationEventBus)
        {
            _logger = logger;
            _userRevertService = userRevertService;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(AnnouncementPreferenceDeletionCompletedIntegrationEventFailure integrationEvent, CancellationToken cancellationToken = default)
        {
            var announcementPreferenceName = integrationEvent.AnnouncementPreferenceType == AnnouncementPreferenceType.FlatForRentAnnouncementPreference
                ? nameof(FlatForRentAnnouncementPreference)
                : nameof(RoomForRentAnnouncementPreference);
            var message = $"Could not finish {announcementPreferenceName} deletion process.";
            _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaUsers, integrationEvent,
                "userId={userId}, message={message}, reason={reason}, code={code}",
                integrationEvent.UserId, message, integrationEvent.Reason, integrationEvent.Code);

            var userAnnouncementPreferenceDeletionCompletedIntegrationEventFailure =
                new UserAnnouncementPreferenceDeletionCompletedIntegrationEventFailure(integrationEvent.CorrelationId,
                    integrationEvent.Code, integrationEvent.Reason, integrationEvent.UserId,
                    integrationEvent.AnnouncementPreferenceId, integrationEvent.AnnouncementPreferenceType);
            var userAnnouncementPreferenceDeletionCompletedIntegrationEventFailureTask =
                _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceDeletionCompletedIntegrationEventFailure);

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

            await userAnnouncementPreferenceDeletionCompletedIntegrationEventFailureTask;
        }
    }
}