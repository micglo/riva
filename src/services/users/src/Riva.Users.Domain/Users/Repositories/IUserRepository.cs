using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Users.Domain.Users.Aggregates;

namespace Riva.Users.Domain.Users.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<List<User>> FindAsync(int? skip, int? take, string sort, string email, bool? serviceActive);
        Task<User> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<long> CountAsync();
        Task<long> CountAsync(string email, bool? serviceActive);
    }
}