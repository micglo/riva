using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Entities;

namespace Riva.Users.Core.Queries.Handlers
{
    public class GetFlatForRentAnnouncementPreferenceQueryHandler : IQueryHandler<GetFlatForRentAnnouncementPreferenceInputQuery, FlatForRentAnnouncementPreferenceOutputQuery>
    {
        private readonly IUserGetterService _userGetterService; 
        private readonly IFlatForRentAnnouncementPreferenceGetterService _flatForRentAnnouncementPreferenceGetterService;
        private readonly IMapper _mapper;

        public GetFlatForRentAnnouncementPreferenceQueryHandler(IUserGetterService userGetterService,
            IFlatForRentAnnouncementPreferenceGetterService flatForRentAnnouncementPreferenceGetterService, IMapper mapper)
        {
            _userGetterService = userGetterService;
            _flatForRentAnnouncementPreferenceGetterService = flatForRentAnnouncementPreferenceGetterService;
            _mapper = mapper;
        }

        public async Task<FlatForRentAnnouncementPreferenceOutputQuery> HandleAsync(GetFlatForRentAnnouncementPreferenceInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(inputQuery.UserId);
            if(!getUserResult.Success)
                throw new ResourceNotFoundException(getUserResult.Errors);

            var getFlatForRentAnnouncementPreferenceResult =
                _flatForRentAnnouncementPreferenceGetterService.GetByByUserAndId(
                    getUserResult.Value, inputQuery.FlatForRentAnnouncementPreferenceId);
            if (!getFlatForRentAnnouncementPreferenceResult.Success)
                throw new ResourceNotFoundException(getFlatForRentAnnouncementPreferenceResult.Errors);

            return _mapper.Map<FlatForRentAnnouncementPreference, FlatForRentAnnouncementPreferenceOutputQuery>(getFlatForRentAnnouncementPreferenceResult.Value);
        }
    }
}