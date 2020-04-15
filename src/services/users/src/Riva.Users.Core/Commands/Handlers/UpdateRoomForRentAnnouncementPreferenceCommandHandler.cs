using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Users.Core.Extensions;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class UpdateRoomForRentAnnouncementPreferenceCommandHandler : ICommandHandler<UpdateRoomForRentAnnouncementPreferenceCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly IRoomForRentAnnouncementPreferenceGetterService _roomForRentAnnouncementPreferenceGetterService;
        private readonly IRoomForRentAnnouncementPreferenceVerificationService _roomForRentAnnouncementPreferenceVerificationService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public UpdateRoomForRentAnnouncementPreferenceCommandHandler(IUserGetterService userGetterService, ICityVerificationService cityVerificationService,
            IRoomForRentAnnouncementPreferenceGetterService roomForRentAnnouncementPreferenceGetterService, IUserRepository userRepository,
            IRoomForRentAnnouncementPreferenceVerificationService roomForRentAnnouncementPreferenceVerificationService, ICommunicationBus communicationBus,
            IIntegrationEventBus integrationEventBus)
        {
            _userGetterService = userGetterService;
            _cityVerificationService = cityVerificationService;
            _roomForRentAnnouncementPreferenceGetterService = roomForRentAnnouncementPreferenceGetterService;
            _roomForRentAnnouncementPreferenceVerificationService = roomForRentAnnouncementPreferenceVerificationService;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(UpdateRoomForRentAnnouncementPreferenceCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if (!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var getRoomForRentAnnouncementPreferenceResult =
                _roomForRentAnnouncementPreferenceGetterService.GetByByUserAndId(
                    getUserResult.Value, command.RoomForRentAnnouncementPreferenceId);
            if (!getRoomForRentAnnouncementPreferenceResult.Success)
                throw new ResourceNotFoundException(getRoomForRentAnnouncementPreferenceResult.Errors);

            await UpdateCityAndCityDistrictsAsync(getRoomForRentAnnouncementPreferenceResult.Value, command.CityId, command.CityDistricts.ToList());
            getRoomForRentAnnouncementPreferenceResult.Value.ChangePriceMin(command.PriceMin);
            getRoomForRentAnnouncementPreferenceResult.Value.ChangePriceMax(command.PriceMax);
            getRoomForRentAnnouncementPreferenceResult.Value.ChangeRoomType(command.RoomType);

            var roomForRentAnnouncementPreferencesVerificationResult =
                _roomForRentAnnouncementPreferenceVerificationService.VerifyRoomForRentAnnouncementPreferences(getUserResult.Value.RoomForRentAnnouncementPreferences);
            if (!roomForRentAnnouncementPreferencesVerificationResult.Success)
                throw new ValidationException(roomForRentAnnouncementPreferencesVerificationResult.Errors);

            getUserResult.Value.AddRoomForRentAnnouncementPreferenceChangedEvent(getRoomForRentAnnouncementPreferenceResult.Value, command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);

            var userRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent =
                new UserRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent(command.CorrelationId,
                    getUserResult.Value.Id,
                    getRoomForRentAnnouncementPreferenceResult.Value.Id,
                    getRoomForRentAnnouncementPreferenceResult.Value.CityId,
                    getRoomForRentAnnouncementPreferenceResult.Value.PriceMin,
                    getRoomForRentAnnouncementPreferenceResult.Value.PriceMax,
                    getRoomForRentAnnouncementPreferenceResult.Value.RoomType.ConvertToEnum(),
                    getRoomForRentAnnouncementPreferenceResult.Value.CityDistricts);
            await _integrationEventBus.PublishIntegrationEventAsync(userRoomForRentAnnouncementPreferenceUpdatedIntegrationEvent);
        }

        private async Task UpdateCityAndCityDistrictsAsync(RoomForRentAnnouncementPreference roomForRentAnnouncementPreference, Guid cityId, ICollection<Guid> cityDistricts)
        {
            var sameCityDistricts = roomForRentAnnouncementPreference.CityDistricts.All(cityDistricts.Contains) &&
                                    roomForRentAnnouncementPreference.CityDistricts.Count == cityDistricts.Count;
            if (roomForRentAnnouncementPreference.CityId != cityId)
            {
                var cityAndCityDistrictsVerificationResult = await _cityVerificationService.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);
                if (!cityAndCityDistrictsVerificationResult.Success)
                    throw new ValidationException(cityAndCityDistrictsVerificationResult.Errors);

                roomForRentAnnouncementPreference.ChangeCityId(cityId);
                roomForRentAnnouncementPreference.ChangeCityDistricts(cityDistricts);
            }

            if (roomForRentAnnouncementPreference.CityId == cityId && !sameCityDistricts)
            {
                var cityAndCityDistrictsVerificationResult = await _cityVerificationService.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);
                if (!cityAndCityDistrictsVerificationResult.Success)
                    throw new ValidationException(cityAndCityDistrictsVerificationResult.Errors);

                roomForRentAnnouncementPreference.ChangeCityDistricts(cityDistricts);
            }
        }
    }
}