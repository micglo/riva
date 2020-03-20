using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Repositories;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries.Handlers
{
    public class GetFlatForRentAnnouncementsQueryHandler : IQueryHandler<GetFlatForRentAnnouncementsInputQuery, CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>>
    {
        private readonly IFlatForRentAnnouncementRepository _flatForRentAnnouncementRepository;
        private readonly IMapper _mapper;

        public GetFlatForRentAnnouncementsQueryHandler(IFlatForRentAnnouncementRepository flatForRentAnnouncementRepository, IMapper mapper)
        {
            _flatForRentAnnouncementRepository = flatForRentAnnouncementRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>> HandleAsync(GetFlatForRentAnnouncementsInputQuery inputQuery, 
            CancellationToken cancellationToken = default)
        {
            List<FlatForRentAnnouncement> flatForRentAnnouncements;
            long totalCount;
            
            if (inputQuery != null)
            {
                var findTask = _flatForRentAnnouncementRepository.FindAsync(inputQuery.Page,
                    inputQuery.PageSize, inputQuery.Sort, inputQuery.CityId, inputQuery.CreatedFrom, inputQuery.CreatedTo, inputQuery.PriceFrom,
                    inputQuery.PriceTo, inputQuery.CityDistrict, inputQuery.NumberOfRooms);
                totalCount = await _flatForRentAnnouncementRepository.CountAsync(inputQuery.CityId, inputQuery.CreatedFrom,
                    inputQuery.CreatedTo, inputQuery.PriceFrom, inputQuery.PriceTo, inputQuery.CityDistrict, inputQuery.NumberOfRooms);
                flatForRentAnnouncements = await findTask;
            }
            else
            {
                var getAllTask = _flatForRentAnnouncementRepository.GetAllAsync();
                totalCount = await _flatForRentAnnouncementRepository.CountAsync();
                flatForRentAnnouncements = await getAllTask;
            }

            var results = _mapper.Map<List<FlatForRentAnnouncement>, IEnumerable<FlatForRentAnnouncementOutputQuery>>(flatForRentAnnouncements);
            return new CollectionOutputQuery<FlatForRentAnnouncementOutputQuery>(totalCount, results);
        }
    }
}