using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.BuildingBlocks.Core.Queries;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;

namespace Riva.Identity.Core.Queries.Handlers
{
    public class GetAccountsQueryHandler : IQueryHandler<GetAccountsInputQuery, CollectionOutputQuery<GetAccountsOutputQuery>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public GetAccountsQueryHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<CollectionOutputQuery<GetAccountsOutputQuery>> HandleAsync(GetAccountsInputQuery inputQuery, CancellationToken cancellationToken = default)
        {
            List<Account> accounts;
            long totalCount;

            if (inputQuery != null)
            {
                accounts = await _accountRepository.FindAsync(inputQuery.Page, inputQuery.PageSize, inputQuery.Sort,
                    inputQuery.Email, inputQuery.Confirmed);
                totalCount = await _accountRepository.CountAsync(inputQuery.Email, inputQuery.Confirmed);
            }
            else
            {
                accounts = await _accountRepository.GetAllAsync();
                totalCount = await _accountRepository.CountAsync();
            }

            var results = _mapper.Map<List<Account>, IEnumerable<GetAccountsOutputQuery>>(accounts);
            return new CollectionOutputQuery<GetAccountsOutputQuery>(totalCount, results);
        }
    }
}