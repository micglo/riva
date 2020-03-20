using System.Threading;
using System.Threading.Tasks;
using Riva.Announcements.Core.Services;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;

namespace Riva.Announcements.Core.Queries.Handlers
{
    public class GetFlatForRentAnnouncementQueryHandler : IQueryHandler<GetFlatForRentAnnouncementInputQuery, FlatForRentAnnouncementOutputQuery>
    {
        private readonly IFlatForRentAnnouncementGetterService _flatForRentAnnouncementGetterService;
        private readonly IMapper _mapper;

        public GetFlatForRentAnnouncementQueryHandler(IFlatForRentAnnouncementGetterService flatForRentAnnouncementGetterService, 
            IMapper mapper)
        {
            _flatForRentAnnouncementGetterService = flatForRentAnnouncementGetterService;
            _mapper = mapper;
        }

        public async Task<FlatForRentAnnouncementOutputQuery> HandleAsync(GetFlatForRentAnnouncementInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getFlatForRentAnnouncementResult = await _flatForRentAnnouncementGetterService.GetByIdAsync(inputQuery.FlatForRentAnnouncementId);
            if (!getFlatForRentAnnouncementResult.Success)
                throw new ResourceNotFoundException(getFlatForRentAnnouncementResult.Errors);

            return _mapper.Map<FlatForRentAnnouncement, FlatForRentAnnouncementOutputQuery>(getFlatForRentAnnouncementResult.Value);
        }
    }
}