using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries.Handlers
{
    public class GetRoomForRentAnnouncementsQueryHandler : IQueryHandler<GetRoomForRentAnnouncementsInputQuery, CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>>
    {
        private readonly IRoomForRentAnnouncementRepository _roomForRentAnnouncementRepository;
        private readonly IMapper _mapper;

        public GetRoomForRentAnnouncementsQueryHandler(IRoomForRentAnnouncementRepository roomForRentAnnouncementRepository, IMapper mapper)
        {
            _roomForRentAnnouncementRepository = roomForRentAnnouncementRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>> HandleAsync(GetRoomForRentAnnouncementsInputQuery inputQuery, 
            CancellationToken cancellationToken = default)
        {
            List<RoomForRentAnnouncement> roomForRentAnnouncements;
            long totalCount;
            
            if (inputQuery != null)
            {
                var findTask = _roomForRentAnnouncementRepository.FindAsync(inputQuery.Page,
                    inputQuery.PageSize, inputQuery.Sort, inputQuery.CityId, inputQuery.CreatedFrom, inputQuery.CreatedTo, inputQuery.PriceFrom,
                    inputQuery.PriceTo, inputQuery.CityDistrict, inputQuery.RoomType);
                totalCount = await _roomForRentAnnouncementRepository.CountAsync(inputQuery.CityId, inputQuery.CreatedFrom,
                    inputQuery.CreatedTo, inputQuery.PriceFrom, inputQuery.PriceTo, inputQuery.CityDistrict, inputQuery.RoomType);
                roomForRentAnnouncements = await findTask;
            }
            else
            {
                var getAllTask = _roomForRentAnnouncementRepository.GetAllAsync();
                totalCount = await _roomForRentAnnouncementRepository.CountAsync();
                roomForRentAnnouncements = await getAllTask;
            }

            var results = _mapper.Map<List<RoomForRentAnnouncement>, IEnumerable<RoomForRentAnnouncementOutputQuery>>(roomForRentAnnouncements);
            return new CollectionOutputQuery<RoomForRentAnnouncementOutputQuery>(totalCount, results);
        }
    }
}