using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Users.Domain.Users.Aggregates;
using Riva.Users.Domain.Users.Repositories;

namespace Riva.Users.Core.Queries.Handlers
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersInputQuery, CollectionOutputQuery<UserOutputQuery>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<UserOutputQuery>> HandleAsync(GetUsersInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            List<User> users;
            long totalCount;

            if (inputQuery != null)
            {
                users = await _userRepository.FindAsync(inputQuery.Page, inputQuery.PageSize, inputQuery.Sort,
                    inputQuery.Email, inputQuery.ServiceActive);
                totalCount = await _userRepository.CountAsync(inputQuery.Email, inputQuery.ServiceActive);
            }
            else
            {
                users = await _userRepository.GetAllAsync();
                totalCount = await _userRepository.CountAsync();
            }

            var results = _mapper.Map<List<User>, IEnumerable<UserOutputQuery>>(users);
            return new CollectionOutputQuery<UserOutputQuery>(totalCount, results);
        }
    }
}