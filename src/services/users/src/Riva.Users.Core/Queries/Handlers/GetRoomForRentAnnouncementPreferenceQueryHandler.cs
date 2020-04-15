using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Core.Queries.Handlers
{
    public class GetRoomForRentAnnouncementPreferenceQueryHandler : IQueryHandler<GetRoomForRentAnnouncementPreferenceInputQuery, RoomForRentAnnouncementPreferenceOutputQuery>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IRoomForRentAnnouncementPreferenceGetterService _roomForRentAnnouncementPreferenceGetterService;
        private readonly IMapper _mapper;

        public GetRoomForRentAnnouncementPreferenceQueryHandler(IUserGetterService userGetterService,
            IRoomForRentAnnouncementPreferenceGetterService roomForRentAnnouncementPreferenceGetterService, IMapper mapper)
        {
            _userGetterService = userGetterService;
            _roomForRentAnnouncementPreferenceGetterService = roomForRentAnnouncementPreferenceGetterService;
            _mapper = mapper;
        }

        public async Task<RoomForRentAnnouncementPreferenceOutputQuery> HandleAsync(GetRoomForRentAnnouncementPreferenceInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(inputQuery.UserId);
            if (!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var getRoomForRentAnnouncementPreferenceResult =
                _roomForRentAnnouncementPreferenceGetterService.GetByByUserAndId(
                    getUserResult.Value, inputQuery.RoomForRentAnnouncementPreferenceId);
            if (!getRoomForRentAnnouncementPreferenceResult.Success)
                throw new ResourceNotFoundException(getRoomForRentAnnouncementPreferenceResult.Errors);

            return _mapper.Map<RoomForRentAnnouncementPreference, RoomForRentAnnouncementPreferenceOutputQuery>(getRoomForRentAnnouncementPreferenceResult.Value);
        }
    }
}