using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;

namespace Riva.Announcements.Core.Commands.Handlers
{
    public class UpdateRoomForRentAnnouncementCommandHandler : ICommandHandler<UpdateRoomForRentAnnouncementCommand>
    {
        private readonly IRoomForRentAnnouncementGetterService _roomForRentAnnouncementGetterService;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly ICityDistrictVerificationService _cityDistrictVerificationService;
        private readonly IRoomForRentAnnouncementRepository _roomForRentAnnouncementRepository;

        public UpdateRoomForRentAnnouncementCommandHandler(IRoomForRentAnnouncementGetterService roomForRentAnnouncementGetterService, 
            ICityVerificationService cityVerificationService, ICityDistrictVerificationService cityDistrictVerificationService, 
            IRoomForRentAnnouncementRepository roomForRentAnnouncementRepository)
        {
            _roomForRentAnnouncementGetterService = roomForRentAnnouncementGetterService;
            _cityVerificationService = cityVerificationService;
            _cityDistrictVerificationService = cityDistrictVerificationService;
            _roomForRentAnnouncementRepository = roomForRentAnnouncementRepository;
        }

        public async Task HandleAsync(UpdateRoomForRentAnnouncementCommand command, CancellationToken cancellationToken = default)
        {
            var getRoomForRentAnnouncementResult = await _roomForRentAnnouncementGetterService.GetByIdAsync(command.RoomForRentAnnouncementId);
            if (!getRoomForRentAnnouncementResult.Success)
                throw new ResourceNotFoundException(getRoomForRentAnnouncementResult.Errors);

            var cityVerificationResult = await _cityVerificationService.VerifyCityExistsAsync(command.CityId);
            if (!cityVerificationResult.Success)
                throw new ValidationException(cityVerificationResult.Errors);

            var cityDistrictsVerificationResult =
                await _cityDistrictVerificationService.VerifyCityDistrictsExistAsync(command.CityId,
                    command.CityDistricts);
            if (!cityDistrictsVerificationResult.Success)
                throw new ValidationException(cityDistrictsVerificationResult.Errors);

            UpdateDetails(getRoomForRentAnnouncementResult.Value, command);
            UpdateCityDistricts(getRoomForRentAnnouncementResult.Value, command.CityDistricts.ToList());
            UpdateRoomTypes(getRoomForRentAnnouncementResult.Value, command.RoomTypes.ToList());

            await _roomForRentAnnouncementRepository.UpdateAsync(getRoomForRentAnnouncementResult.Value);
        }

        private static void UpdateDetails(RoomForRentAnnouncement roomForRentAnnouncement, UpdateRoomForRentAnnouncementCommand command)
        {
            roomForRentAnnouncement.ChangeTitle(command.Title);
            roomForRentAnnouncement.ChangeSourceUrl(command.SourceUrl);
            roomForRentAnnouncement.ChangeCityId(command.CityId);
            roomForRentAnnouncement.ChangeDescription(command.Description);
            roomForRentAnnouncement.ChangePrice(command.Price);
        }

        private static void UpdateCityDistricts(RoomForRentAnnouncement roomForRentAnnouncement, ICollection<Guid> cityDistricts)
        {
            var cityDistrictsToRemove = roomForRentAnnouncement.CityDistricts.Except(cityDistricts).ToList();
            var cityDistrictsToAdd = cityDistricts.Except(roomForRentAnnouncement.CityDistricts).ToList();

            foreach (var cityDistrict in cityDistrictsToRemove)
            {
                roomForRentAnnouncement.RemoveCityDistrict(cityDistrict);
            }

            foreach (var cityDistrict in cityDistrictsToAdd)
            {
                roomForRentAnnouncement.AddCityDistrict(cityDistrict);
            }
        }

        private static void UpdateRoomTypes(RoomForRentAnnouncement roomForRentAnnouncement, ICollection<RoomTypeEnumeration> roomTypes)
        {
            var roomTypesToRemove = roomForRentAnnouncement.RoomTypes.Except(roomTypes).ToList();
            var roomTypesToAdd = roomTypes.Except(roomForRentAnnouncement.RoomTypes).ToList();

            foreach (var roomType in roomTypesToRemove)
            {
                roomForRentAnnouncement.RemoveRoomType(roomType);
            }

            foreach (var roomType in roomTypesToAdd)
            {
                roomForRentAnnouncement.AddRoomType(roomType);
            }
        }
    }
}