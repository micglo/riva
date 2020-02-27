using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Roles.Aggregates;

namespace Riva.Identity.Core.Queries.Handlers
{
    public class GetRoleQueryHandler : IQueryHandler<GetRoleInputQuery, RoleOutputQuery>
    {
        private readonly IRoleGetterService _roleGetterService;
        private readonly IMapper _mapper;

        public GetRoleQueryHandler(IRoleGetterService roleGetterService, IMapper mapper)
        {
            _roleGetterService = roleGetterService;
            _mapper = mapper;
        }

        public async Task<RoleOutputQuery> HandleAsync(GetRoleInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getRoleResult = await _roleGetterService.GetByIdAsync(inputQuery.RoleId);
            if(getRoleResult.Success)
                return _mapper.Map<Role, RoleOutputQuery>(getRoleResult.Value);
            throw new ResourceNotFoundException(getRoleResult.Errors);
        }
    }
}