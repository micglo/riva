using System;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications.IntegrationEvents;
using Riva.BuildingBlocks.Core.Enumerations;
using Riva.BuildingBlocks.Core.Logger;
using Riva.SignalR.Services;

namespace Riva.SignalR.IntegrationEvents.Handlers
{
    public class UserAnnouncementPreferenceCreationCompletedIntegrationEventFailureHandler : IIntegrationEventHandler<UserAnnouncementPreferenceCreationCompletedIntegrationEventFailure>
    {
        private readonly IHubService _hubService;
        private readonly ILogger _logger;

        public UserAnnouncementPreferenceCreationCompletedIntegrationEventFailureHandler(IHubService hubService, ILogger logger)
        {
            _hubService = hubService;
            _logger = logger;
        }

        public Task HandleAsync(UserAnnouncementPreferenceCreationCompletedIntegrationEventFailure integrationEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                return _hubService.SendToUserAsync(integrationEvent.UserId, integrationEvent);
            }
            catch (Exception e)
            {
                _logger.LogIntegrationEventError(ServiceComponentEnumeration.RivaSignalR, integrationEvent,
                    "message={message}, stackTrace={stackTrace}", e.Message, e.StackTrace);
                return Task.CompletedTask;
            }
        }
    }
}