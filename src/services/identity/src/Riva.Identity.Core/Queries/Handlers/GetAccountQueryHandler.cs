using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Exceptions;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Core.Queries.Handlers
{
    public class GetAccountQueryHandler : IQueryHandler<GetAccountInputQuery, GetAccountOutputQuery>
    {
        private readonly IAccountGetterService _accountGetterService;
        private readonly IMapper _mapper;

        public GetAccountQueryHandler(IAccountGetterService accountGetterService, IMapper mapper)
        {
            _accountGetterService = accountGetterService;
            _mapper = mapper;
        }

        public async Task<GetAccountOutputQuery> HandleAsync(GetAccountInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            var getAccountResult = await _accountGetterService.GetByIdAsync(inputQuery.AccountId);
            if(getAccountResult.Success)
                return _mapper.Map<Account, GetAccountOutputQuery>(getAccountResult.Value);
            throw new ResourceNotFoundException(getAccountResult.Errors);
        }
    }
}