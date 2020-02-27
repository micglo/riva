using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.Identity.Domain.Accounts.Repositories;
using Riva.Identity.Domain.PersistedGrants.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;

namespace Riva.Identity.Infrastructure.Services
{
    public class AccountDataConsistencyService : IAccountDataConsistencyService
    {
        private readonly RivaIdentityDbContext _rivaIdentityDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IPersistedGrantRepository _persistedGrantRepository;

        public AccountDataConsistencyService(RivaIdentityDbContext rivaIdentityDbContext, PersistedGrantDbContext persistedGrantDbContext,
            IAccountRepository accountRepository, IPersistedGrantRepository persistedGrantRepository)
        {
            _rivaIdentityDbContext = rivaIdentityDbContext;
            _persistedGrantDbContext = persistedGrantDbContext;
            _accountRepository = accountRepository;
            _persistedGrantRepository = persistedGrantRepository;
        }

        public async Task DeleteAccountWithRelatedPersistedGrants(Account account)
        {
            var strategy = _rivaIdentityDbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _rivaIdentityDbContext.Database.BeginTransactionAsync();
                await _accountRepository.DeleteAsync(account);
                await _persistedGrantDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());
                await _persistedGrantRepository.DeleteAllBySubjectIdAsync(account.Id);
                await transaction.CommitAsync();
            });
        }
    }
}