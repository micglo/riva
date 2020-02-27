using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Riva.Identity.Core.Services
{
    public interface ISignInService
    {
        Task SignInAsync(Guid accountId, string email, bool rememberLogin, IEnumerable<Claim> claims);
        Task ExternalSignInAsync(Guid accountId, string email, string scheme, IEnumerable<Claim> claims);
    }
}