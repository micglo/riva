using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Domain.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAsync();
        Task<List<Account>> FindAsync(int? skip, int? take, string sort, string email, bool? confirmed);
        Task<Account> GetByIdAsync(Guid id);
        Task<Account> GetByEmailAsync(string email);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(Account account);
        Task<long> CountAsync();
        Task<long> CountAsync(string email, bool? confirmed);
    }
}