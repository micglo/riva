using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.Announcements.Core.Commands.Handlers
{
    public class UpdateFlatForRentAnnouncementCommandHandler : ICommandHandler<UpdateFlatForRentAnnouncementCommand>
    {
        private readonly IFlatForRentAnnouncementGetterService _flatForRentAnnouncementGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly ICityDistrictVerificationService _cityDistrictVerificationService;
        private readonly IFlatForRentAnnouncementRepository _flatForRentAnnouncementRepository;

        public UpdateFlatForRentAnnouncementCommandHandler(IFlatForRentAnnouncementGetterService flatForRentAnnouncementGetterService, 
            ICityVerificationService cityVerificationService, ICityDistrictVerificationService cityDistrictVerificationService, 
            IFlatForRentAnnouncementRepository flatForRentAnnouncementRepository)
        {
            _flatForRentAnnouncementGetterService = flatForRentAnnouncementGetterService;
            _cityVerificationService = cityVerificationService;
            _cityDistrictVerificationService = cityDistrictVerificationService;
            _flatForRentAnnouncementRepository = flatForRentAnnouncementRepository;
        }

        public async Task HandleAsync(UpdateFlatForRentAnnouncementCommand command, CancellationToken cancellationToken = default)
        {
            var getFlatForRentAnnouncementResult = await _flatForRentAnnouncementGetterService.GetByIdAsync(command.FlatForRentAnnouncementId);
            if (!getFlatForRentAnnouncementResult.Success)
                throw new ResourceNotFoundException(getFlatForRentAnnouncementResult.Errors);

            var cityVerificationResult = await _cityVerificationService.VerifyCityExistsAsync(command.CityId);
            if (!cityVerificationResult.Success)
                throw new ValidationException(cityVerificationResult.Errors);

            var cityDistrictsVerificationResult = await _cityDistrictVerificationService.VerifyCityDistrictsExistAsync(command.CityId, command.CityDistricts);
            if (!cityDistrictsVerificationResult.Success)
                throw new ValidationException(cityDistrictsVerificationResult.Errors);

            UpdateDetails(getFlatForRentAnnouncementResult.Value, command);
            UpdateCityDistricts(getFlatForRentAnnouncementResult.Value, command.CityDistricts.ToList());

            await _flatForRentAnnouncementRepository.UpdateAsync(getFlatForRentAnnouncementResult.Value);
        }

        private static void UpdateDetails(FlatForRentAnnouncement flatForRentAnnouncement, UpdateFlatForRentAnnouncementCommand command)
        {
            flatForRentAnnouncement.ChangeTitle(command.Title);
            flatForRentAnnouncement.ChangeSourceUrl(command.SourceUrl);
            flatForRentAnnouncement.ChangeCityId(command.CityId);
            flatForRentAnnouncement.ChangeDescription(command.Description);
            flatForRentAnnouncement.ChangePrice(command.Price);
            flatForRentAnnouncement.ChangeNumberOfRooms(command.NumberOfRooms);
        }

        private static void UpdateCityDistricts(FlatForRentAnnouncement flatForRentAnnouncement, ICollection<Guid> cityDistricts)
        {
            var cityDistrictsToRemove = flatForRentAnnouncement.CityDistricts.Except(cityDistricts).ToList();
            var cityDistrictsToAdd = cityDistricts.Except(flatForRentAnnouncement.CityDistricts).ToList();

            foreach (var cityDistrict in cityDistrictsToRemove)
            {
                flatForRentAnnouncement.RemoveCityDistrict(cityDistrict);
            }

            foreach (var cityDistrict in cityDistrictsToAdd)
            {
                flatForRentAnnouncement.AddCityDistrict(cityDistrict);
            }
        }
    }
}