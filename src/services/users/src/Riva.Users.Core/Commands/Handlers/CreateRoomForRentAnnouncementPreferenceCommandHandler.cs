using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Users.Core.Extensions;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class CreateRoomForRentAnnouncementPreferenceCommandHandler : ICommandHandler<CreateRoomForRentAnnouncementPreferenceCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly IUserVerificationService _userVerificationService;
        private readonly IRoomForRentAnnouncementPreferenceVerificationService _roomForRentAnnouncementPreferenceVerificationService;
        private readonly IMapper _mapper;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public CreateRoomForRentAnnouncementPreferenceCommandHandler(IUserGetterService userGetterService, ICityVerificationService cityVerificationService, 
            IUserVerificationService userVerificationService, IRoomForRentAnnouncementPreferenceVerificationService roomForRentAnnouncementPreferenceVerificationService, 
            IMapper mapper, ICommunicationBus communicationBus, IUserRepository userRepository, IIntegrationEventBus integrationEventBus)
        {
            _userGetterService = userGetterService;
            _cityVerificationService = cityVerificationService;
            _userVerificationService = userVerificationService;
            _roomForRentAnnouncementPreferenceVerificationService = roomForRentAnnouncementPreferenceVerificationService;
            _mapper = mapper;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(CreateRoomForRentAnnouncementPreferenceCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if (!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var cityAndCityDistrictsVerificationResult = await _cityVerificationService.VerifyCityAndCityDistrictsAsync(command.CityId, command.CityDistricts);
            if (!cityAndCityDistrictsVerificationResult.Success)
                throw new ValidationException(cityAndCityDistrictsVerificationResult.Errors);

            var announcementPreferenceLimitIsNotExceededVerificationResult =
                _userVerificationService.VerifyAnnouncementPreferenceLimitIsNotExceeded(
                    getUserResult.Value.AnnouncementPreferenceLimit,
                    getUserResult.Value.RoomForRentAnnouncementPreferences.Count +
                    getUserResult.Value.RoomForRentAnnouncementPreferences.Count);
            if (!announcementPreferenceLimitIsNotExceededVerificationResult.Success)
                throw new ValidationException(announcementPreferenceLimitIsNotExceededVerificationResult.Errors);

            var roomForRentAnnouncementPreference =
                _mapper.Map<CreateRoomForRentAnnouncementPreferenceCommand, RoomForRentAnnouncementPreference>(command);
            var roomForRentAnnouncementPreferences = getUserResult.Value.RoomForRentAnnouncementPreferences.ToList();
            roomForRentAnnouncementPreferences.Add(roomForRentAnnouncementPreference);
            var roomForRentAnnouncementPreferencesVerificationResult =
                _roomForRentAnnouncementPreferenceVerificationService.VerifyRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences);
            if (!roomForRentAnnouncementPreferencesVerificationResult.Success)
                throw new ValidationException(roomForRentAnnouncementPreferencesVerificationResult.Errors);

            getUserResult.Value.AddRoomForRentAnnouncementPreference(roomForRentAnnouncementPreference, command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);

            var userRoomForRentAnnouncementPreferenceCreatedIntegrationEvent =
                new UserRoomForRentAnnouncementPreferenceCreatedIntegrationEvent(command.CorrelationId,
                    getUserResult.Value.Id,
                    roomForRentAnnouncementPreference.Id,
                    getUserResult.Value.Email,
                    roomForRentAnnouncementPreference.CityId, 
                    getUserResult.Value.ServiceActive,
                    getUserResult.Value.AnnouncementSendingFrequency.ConvertToEnum(),
                    roomForRentAnnouncementPreference.PriceMin, 
                    roomForRentAnnouncementPreference.PriceMax,
                    roomForRentAnnouncementPreference.RoomType.ConvertToEnum(),
                    roomForRentAnnouncementPreference.CityDistricts);
            await _integrationEventBus.PublishIntegrationEventAsync(userRoomForRentAnnouncementPreferenceCreatedIntegrationEvent);
        }
    }
}