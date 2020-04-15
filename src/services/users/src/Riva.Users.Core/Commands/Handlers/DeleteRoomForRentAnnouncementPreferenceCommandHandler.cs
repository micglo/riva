using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Users.Core.Enums;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class DeleteRoomForRentAnnouncementPreferenceCommandHandler : ICommandHandler<DeleteRoomForRentAnnouncementPreferenceCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IRoomForRentAnnouncementPreferenceGetterService _roomForRentAnnouncementPreferenceGetterService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public DeleteRoomForRentAnnouncementPreferenceCommandHandler(IUserGetterService userGetterService,
            IRoomForRentAnnouncementPreferenceGetterService roomForRentAnnouncementPreferenceGetterService, 
            ICommunicationBus communicationBus, IUserRepository userRepository, IIntegrationEventBus integrationEventBus)
        {
            _userGetterService = userGetterService;
            _roomForRentAnnouncementPreferenceGetterService = roomForRentAnnouncementPreferenceGetterService;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(DeleteRoomForRentAnnouncementPreferenceCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if (!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var getRoomForRentAnnouncementPreferenceResult =
                _roomForRentAnnouncementPreferenceGetterService.GetByByUserAndId(
                    getUserResult.Value, command.RoomForRentAnnouncementPreferenceId);
            if (!getRoomForRentAnnouncementPreferenceResult.Success)
                throw new ResourceNotFoundException(getRoomForRentAnnouncementPreferenceResult.Errors);

            getUserResult.Value.DeleteRoomForRentAnnouncementPreference(getRoomForRentAnnouncementPreferenceResult.Value, command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);

            var userAnnouncementPreferenceDeletedIntegrationEvent =
                new UserAnnouncementPreferenceDeletedIntegrationEvent(command.CorrelationId, getUserResult.Value.Id,
                    getRoomForRentAnnouncementPreferenceResult.Value.Id,
                    AnnouncementPreferenceType.RoomForRentAnnouncementPreference);
            await _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceDeletedIntegrationEvent);
        }
    }
}