using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Riva.Identity.Domain.Roles.Aggregates;

namespace Riva.Identity.Domain.Roles.Repositories
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<List<Role>> FindByIdsAsync(IEnumerable<Guid> ids);
        Task<Role> GetByIdAsync(Guid id);
        Task<Role> GetByNameAsync(string name);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
    }
}