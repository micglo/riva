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
    public class DeleteFlatForRentAnnouncementPreferenceCommandHandler : ICommandHandler<DeleteFlatForRentAnnouncementPreferenceCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IFlatForRentAnnouncementPreferenceGetterService _flatForRentAnnouncementPreferenceGetterService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public DeleteFlatForRentAnnouncementPreferenceCommandHandler(IUserGetterService userGetterService,
            IFlatForRentAnnouncementPreferenceGetterService flatForRentAnnouncementPreferenceGetterService, 
            ICommunicationBus communicationBus, IUserRepository userRepository, IIntegrationEventBus integrationEventBus)
        {
            _userGetterService = userGetterService;
            _flatForRentAnnouncementPreferenceGetterService = flatForRentAnnouncementPreferenceGetterService;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(DeleteFlatForRentAnnouncementPreferenceCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if (!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var getFlatForRentAnnouncementPreferenceResult =
                _flatForRentAnnouncementPreferenceGetterService.GetByByUserAndId(
                    getUserResult.Value, command.FlatForRentAnnouncementPreferenceId);
            if (!getFlatForRentAnnouncementPreferenceResult.Success)
                throw new ResourceNotFoundException(getFlatForRentAnnouncementPreferenceResult.Errors);

            getUserResult.Value.DeleteFlatForRentAnnouncementPreference(getFlatForRentAnnouncementPreferenceResult.Value, command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);

            var userAnnouncementPreferenceDeletedIntegrationEvent =
                new UserAnnouncementPreferenceDeletedIntegrationEvent(command.CorrelationId, getUserResult.Value.Id,
                    getFlatForRentAnnouncementPreferenceResult.Value.Id,
                    AnnouncementPreferenceType.FlatForRentAnnouncementPreference);
            await _integrationEventBus.PublishIntegrationEventAsync(userAnnouncementPreferenceDeletedIntegrationEvent);
        }
    }
}