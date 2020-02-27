using System;
using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Core.Services
{
    public interface IAccountCreatorService
    {
        Task<Account> CreateAsync(Guid id, string email, string password, Guid correlationId);
    }
}