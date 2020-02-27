using System;
using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;
using Riva.BuildingBlocks.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface IAccountGetterService
    {
        Task<GetResult<Account>> GetByIdAsync(Guid id);
        Task<GetResult<Account>> GetByEmailAsync(string email);
    }
}