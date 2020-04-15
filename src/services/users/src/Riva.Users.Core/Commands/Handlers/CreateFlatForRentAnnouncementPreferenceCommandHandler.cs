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
    public class CreateFlatForRentAnnouncementPreferenceCommandHandler : ICommandHandler<CreateFlatForRentAnnouncementPreferenceCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly IUserVerificationService _userVerificationService;
        private readonly IFlatForRentAnnouncementPreferenceVerificationService _flatForRentAnnouncementPreferenceVerificationService;
        private readonly IMapper _mapper;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public CreateFlatForRentAnnouncementPreferenceCommandHandler(IUserGetterService userGetterService, ICityVerificationService cityVerificationService, 
            IUserVerificationService userVerificationService, IFlatForRentAnnouncementPreferenceVerificationService flatForRentAnnouncementPreferenceVerificationService, 
            IMapper mapper, ICommunicationBus communicationBus, IUserRepository userRepository, IIntegrationEventBus integrationEventBus)
        {
            _userGetterService = userGetterService;
            _cityVerificationService = cityVerificationService;
            _userVerificationService = userVerificationService;
            _flatForRentAnnouncementPreferenceVerificationService = flatForRentAnnouncementPreferenceVerificationService;
            _mapper = mapper;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(CreateFlatForRentAnnouncementPreferenceCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if(!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var cityAndCityDistrictsVerificationResult = await _cityVerificationService.VerifyCityAndCityDistrictsAsync(command.CityId, command.CityDistricts);
            if(!cityAndCityDistrictsVerificationResult.Success)
                throw new ValidationException(cityAndCityDistrictsVerificationResult.Errors);

            var announcementPreferenceLimitIsNotExceededVerificationResult =
                _userVerificationService.VerifyAnnouncementPreferenceLimitIsNotExceeded(
                    getUserResult.Value.AnnouncementPreferenceLimit,
                    getUserResult.Value.FlatForRentAnnouncementPreferences.Count +
                    getUserResult.Value.RoomForRentAnnouncementPreferences.Count);
            if (!announcementPreferenceLimitIsNotExceededVerificationResult.Success)
                throw new ValidationException(announcementPreferenceLimitIsNotExceededVerificationResult.Errors);

            var flatForRentAnnouncementPreference =
                _mapper.Map<CreateFlatForRentAnnouncementPreferenceCommand, FlatForRentAnnouncementPreference>(command);
            var flatForRentAnnouncementPreferences = getUserResult.Value.FlatForRentAnnouncementPreferences.ToList();
            flatForRentAnnouncementPreferences.Add(flatForRentAnnouncementPreference);
            var flatForRentAnnouncementPreferencesVerificationResult =
                _flatForRentAnnouncementPreferenceVerificationService.VerifyFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences);
            if(!flatForRentAnnouncementPreferencesVerificationResult.Success)
                throw new ValidationException(flatForRentAnnouncementPreferencesVerificationResult.Errors);

            getUserResult.Value.AddFlatForRentAnnouncementPreference(flatForRentAnnouncementPreference, command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);

            var userFlatForRentAnnouncementPreferenceCreatedIntegrationEvent =
                new UserFlatForRentAnnouncementPreferenceCreatedIntegrationEvent(command.CorrelationId,
                    getUserResult.Value.Id,
                    flatForRentAnnouncementPreference.Id,
                    getUserResult.Value.Email,
                    flatForRentAnnouncementPreference.CityId, 
                    getUserResult.Value.ServiceActive,
                    getUserResult.Value.AnnouncementSendingFrequency.ConvertToEnum(),
                    flatForRentAnnouncementPreference.PriceMin, 
                    flatForRentAnnouncementPreference.PriceMax,
                    flatForRentAnnouncementPreference.RoomNumbersMin, 
                    flatForRentAnnouncementPreference.RoomNumbersMax,
                    flatForRentAnnouncementPreference.CityDistricts);
            await _integrationEventBus.PublishIntegrationEventAsync(userFlatForRentAnnouncementPreferenceCreatedIntegrationEvent);
        }
    }
}