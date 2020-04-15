using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Core.Services;
using Riva.Users.Domain.Users.Aggregates;

namespace Riva.Users.Core.Queries.Handlers
{
    public class GetUserQueryHandler : IQueryHandler<GetUserInputQuery, UserOutputQuery>
    {
        private readonly IUserGetterService _userGetterService;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUserGetterService userGetterService, IMapper mapper)
        {
            _userGetterService = userGetterService;
            _mapper = mapper;
        }

        public async Task<UserOutputQuery> HandleAsync(GetUserInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getUserResult = await _userGetterService.GetByIdAsync(inputQuery.UserId);
            if (getUserResult.Success)
                return _mapper.Map<User, UserOutputQuery>(getUserResult.Value);
            throw new ResourceNotFoundException(getUserResult.Errors);
        }
    }
}