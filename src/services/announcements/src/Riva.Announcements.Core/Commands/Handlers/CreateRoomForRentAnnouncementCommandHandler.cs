using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;

namespace Riva.Announcements.Core.Commands.Handlers
{
    public class CreateRoomForRentAnnouncementCommandHandler : ICommandHandler<CreateRoomForRentAnnouncementCommand>
    {
        private readonly IRoomForRentAnnouncementRepository _roomForRentAnnouncementRepository;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly ICityDistrictVerificationService _cityDistrictVerificationService;
        private readonly IMapper _mapper;

        public CreateRoomForRentAnnouncementCommandHandler(IRoomForRentAnnouncementRepository roomForRentAnnouncementRepository, 
            ICityVerificationService cityVerificationService, ICityDistrictVerificationService cityDistrictVerificationService, 
            IMapper mapper)
        {
            _roomForRentAnnouncementRepository = roomForRentAnnouncementRepository;
            _cityVerificationService = cityVerificationService;
            _cityDistrictVerificationService = cityDistrictVerificationService;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateRoomForRentAnnouncementCommand command, CancellationToken cancellationToken = default)
        {
            var cityVerificationResult = await _cityVerificationService.VerifyCityExistsAsync(command.CityId);
            if(!cityVerificationResult.Success)
                throw new ValidationException(cityVerificationResult.Errors);

            var cityDistrictsVerificationResult = await _cityDistrictVerificationService.VerifyCityDistrictsExistAsync(command.CityId, command.CityDistricts);
            if(!cityDistrictsVerificationResult.Success)
                throw new ValidationException(cityDistrictsVerificationResult.Errors);

            var roomForRentAnnouncement = _mapper.Map<CreateRoomForRentAnnouncementCommand, RoomForRentAnnouncement>(command);
            await _roomForRentAnnouncementRepository.AddAsync(roomForRentAnnouncement);
        }
    }
}