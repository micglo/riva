using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Core.Services
{
    public interface IAccountClaimsCreatorService
    {
        public Task<List<Claim>> CreateAccountClaimsAsync(Account account);
    }
}