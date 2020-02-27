using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Domain.Roles.Aggregates;
using Riva.Identity.Domain.Roles.Repositories;

namespace Riva.Identity.Core.Queries.Handlers
{
    public class GetRolesQueryHandler : IQueryHandler<GetRolesInputQuery, CollectionOutputQuery<RoleOutputQuery>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<RoleOutputQuery>> HandleAsync(GetRolesInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<List<Role>, CollectionOutputQuery<RoleOutputQuery>>(roles);
        }
    }
}