using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Communications.Commands;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;

namespace Riva.Announcements.Core.Commands.Handlers
{
    public class CreateFlatForRentAnnouncementCommandHandler : ICommandHandler<CreateFlatForRentAnnouncementCommand>
    {
        private readonly IFlatForRentAnnouncementRepository _flatForRentAnnouncementRepository;
        private readonly ICityVerificationService _cityVerificationService;
        private readonly ICityDistrictVerificationService _cityDistrictVerificationService;
        private readonly IMapper _mapper;

        public CreateFlatForRentAnnouncementCommandHandler(IFlatForRentAnnouncementRepository flatForRentAnnouncementRepository, 
            ICityVerificationService cityVerificationService, ICityDistrictVerificationService cityDistrictVerificationService, 
            IMapper mapper)
        {
            _flatForRentAnnouncementRepository = flatForRentAnnouncementRepository;
            _cityVerificationService = cityVerificationService;
            _cityDistrictVerificationService = cityDistrictVerificationService;
            _mapper = mapper;
        }

        public async Task HandleAsync(CreateFlatForRentAnnouncementCommand command, CancellationToken cancellationToken = default)
        {
            var cityVerificationResult = await _cityVerificationService.VerifyCityExistsAsync(command.CityId);
            if(!cityVerificationResult.Success)
                throw new ValidationException(cityVerificationResult.Errors);

            var cityDistrictsVerificationResult = await _cityDistrictVerificationService.VerifyCityDistrictsExistAsync(command.CityId, command.CityDistricts);
            if(!cityDistrictsVerificationResult.Success)
                throw new ValidationException(cityDistrictsVerificationResult.Errors);

            var flatForRentAnnouncement = _mapper.Map<CreateFlatForRentAnnouncementCommand, FlatForRentAnnouncement>(command);
            await _flatForRentAnnouncementRepository.AddAsync(flatForRentAnnouncement);
        }
    }
}