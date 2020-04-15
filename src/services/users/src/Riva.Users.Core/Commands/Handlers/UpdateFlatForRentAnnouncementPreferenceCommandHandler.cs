using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.Users.Core.IntegrationEvents.UserIntegrationEvents;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Commands.Handlers
{
    public class UpdateFlatForRentAnnouncementPreferenceCommandHandler : ICommandHandler<UpdateFlatForRentAnnouncementPreferenceCommand>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly IFlatForRentAnnouncementPreferenceGetterService _flatForRentAnnouncementPreferenceGetterService;
        private readonly IFlatForRentAnnouncementPreferenceVerificationService _flatForRentAnnouncementPreferenceVerificationService;
        private readonly ICommunicationBus _communicationBus;
        private readonly IUserRepository _userRepository;
        private readonly IIntegrationEventBus _integrationEventBus;

        public UpdateFlatForRentAnnouncementPreferenceCommandHandler(IUserGetterService userGetterService, ICityVerificationService cityVerificationService,
            IFlatForRentAnnouncementPreferenceGetterService flatForRentAnnouncementPreferenceGetterService, IUserRepository userRepository, 
            IFlatForRentAnnouncementPreferenceVerificationService flatForRentAnnouncementPreferenceVerificationService, ICommunicationBus communicationBus, 
            IIntegrationEventBus integrationEventBus)
        {
            _userGetterService = userGetterService;
            _cityVerificationService = cityVerificationService;
            _flatForRentAnnouncementPreferenceGetterService = flatForRentAnnouncementPreferenceGetterService;
            _flatForRentAnnouncementPreferenceVerificationService = flatForRentAnnouncementPreferenceVerificationService;
            _communicationBus = communicationBus;
            _userRepository = userRepository;
            _integrationEventBus = integrationEventBus;
        }

        public async Task HandleAsync(UpdateFlatForRentAnnouncementPreferenceCommand command, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(command.UserId);
            if (!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var getFlatForRentAnnouncementPreferenceResult =
                _flatForRentAnnouncementPreferenceGetterService.GetByByUserAndId(
                    getUserResult.Value, command.FlatForRentAnnouncementPreferenceId);
            if (!getFlatForRentAnnouncementPreferenceResult.Success)
                throw new ResourceNotFoundException(getFlatForRentAnnouncementPreferenceResult.Errors);

            await UpdateCityAndCityDistrictsAsync(getFlatForRentAnnouncementPreferenceResult.Value, command.CityId, command.CityDistricts.ToList());
            getFlatForRentAnnouncementPreferenceResult.Value.ChangePriceMin(command.PriceMin);
            getFlatForRentAnnouncementPreferenceResult.Value.ChangePriceMax(command.PriceMax);
            getFlatForRentAnnouncementPreferenceResult.Value.ChangeRoomNumbersMin(command.RoomNumbersMin);
            getFlatForRentAnnouncementPreferenceResult.Value.ChangeRoomNumbersMax(command.RoomNumbersMax);

            var flatForRentAnnouncementPreferencesVerificationResult =
                _flatForRentAnnouncementPreferenceVerificationService.VerifyFlatForRentAnnouncementPreferences(getUserResult.Value.FlatForRentAnnouncementPreferences);
            if (!flatForRentAnnouncementPreferencesVerificationResult.Success)
                throw new ValidationException(flatForRentAnnouncementPreferencesVerificationResult.Errors);

            getUserResult.Value.AddFlatForRentAnnouncementPreferenceChangedEvent(getFlatForRentAnnouncementPreferenceResult.Value, command.CorrelationId);
            await _communicationBus.DispatchDomainEventsAsync(getUserResult.Value, cancellationToken);
            await _userRepository.UpdateAsync(getUserResult.Value);

            var userFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent =
                new UserFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent(command.CorrelationId,
                    getUserResult.Value.Id,
                    getFlatForRentAnnouncementPreferenceResult.Value.Id,
                    getFlatForRentAnnouncementPreferenceResult.Value.CityId,
                    getFlatForRentAnnouncementPreferenceResult.Value.PriceMin,
                    getFlatForRentAnnouncementPreferenceResult.Value.PriceMax,
                    getFlatForRentAnnouncementPreferenceResult.Value.RoomNumbersMin,
                    getFlatForRentAnnouncementPreferenceResult.Value.RoomNumbersMax,
                    getFlatForRentAnnouncementPreferenceResult.Value.CityDistricts);
            await _integrationEventBus.PublishIntegrationEventAsync(userFlatForRentAnnouncementPreferenceUpdatedIntegrationEvent);
        }

        private async Task UpdateCityAndCityDistrictsAsync(FlatForRentAnnouncementPreference flatForRentAnnouncementPreference, Guid cityId, ICollection<Guid> cityDistricts)
        {
            if (flatForRentAnnouncementPreference.CityId != cityId)
            {
                var cityAndCityDistrictsVerificationResult = await _cityVerificationService.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);
                if (!cityAndCityDistrictsVerificationResult.Success)
                    throw new ValidationException(cityAndCityDistrictsVerificationResult.Errors);

                flatForRentAnnouncementPreference.ChangeCityId(cityId);
                flatForRentAnnouncementPreference.ChangeCityDistricts(cityDistricts);
            }
            else
            {
                var sameCityDistricts = flatForRentAnnouncementPreference.CityDistricts.All(cityDistricts.Contains) &&
                                        flatForRentAnnouncementPreference.CityDistricts.Count == cityDistricts.Count;
                if (!sameCityDistricts)
                {
                    var cityAndCityDistrictsVerificationResult = await _cityVerificationService.VerifyCityAndCityDistrictsAsync(cityId, cityDistricts);
                    if (!cityAndCityDistrictsVerificationResult.Success)
                        throw new ValidationException(cityAndCityDistrictsVerificationResult.Errors);

                    flatForRentAnnouncementPreference.ChangeCityDistricts(cityDistricts);
                }
            }
        }
    }
}