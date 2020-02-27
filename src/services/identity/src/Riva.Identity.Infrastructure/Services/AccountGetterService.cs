using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Riva.Identity.Core.ErrorMessages;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountGetterService : IAccountGetterService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountGetterService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<GetResult<Account>> GetByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
                };
                return GetResult<Account>.Fail(errors);
            }

            return GetResult<Account>.Ok(account);
        }

        public async Task<GetResult<Account>> GetByEmailAsync(string email)
        {
            var account = await _accountRepository.GetByEmailAsync(email);
            if (account is null)
            {
                var errors = new Collection<IError>
                {
                    new Error(AccountErrorCodeEnumeration.NotFound, AccountErrorMessage.NotFound)
                };
                return GetResult<Account>.Fail(errors);
            }

            return GetResult<Account>.Ok(account);
        }
    }
}