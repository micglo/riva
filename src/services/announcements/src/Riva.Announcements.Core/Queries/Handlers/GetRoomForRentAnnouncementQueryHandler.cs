using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries.Handlers
{
    public class GetRoomForRentAnnouncementQueryHandler : IQueryHandler<GetRoomForRentAnnouncementInputQuery, RoomForRentAnnouncementOutputQuery>
    {
        private readonly IRoomForRentAnnouncementGetterService _roomForRentAnnouncementGetterService;
        private readonly IMapper _mapper;

        public GetRoomForRentAnnouncementQueryHandler(IRoomForRentAnnouncementGetterService roomForRentAnnouncementGetterService, IMapper mapper)
        {
            _roomForRentAnnouncementGetterService = roomForRentAnnouncementGetterService;
            _mapper = mapper;
        }

        public async Task<RoomForRentAnnouncementOutputQuery> HandleAsync(GetRoomForRentAnnouncementInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getRoomForRentAnnouncementResult = await _roomForRentAnnouncementGetterService.GetByIdAsync(inputQuery.RoomForRentAnnouncementId);
            if (!getRoomForRentAnnouncementResult.Success)
                throw new ResourceNotFoundException(getRoomForRentAnnouncementResult.Errors);

            return _mapper.Map<RoomForRentAnnouncement, RoomForRentAnnouncementOutputQuery>(getRoomForRentAnnouncementResult.Value);
        }
    }
}