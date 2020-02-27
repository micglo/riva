using System;
using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Core.Services
{
    public interface IAccountProviderService
    {
        Task<Account> ProvideAccountForExternalLoginAsync(string email, Guid correlationId);
    }
}