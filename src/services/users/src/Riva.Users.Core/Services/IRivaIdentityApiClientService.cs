using System;
using System.Threading.Tasks;
using Riva.Users.Core.Models;

namespace Riva.Users.Core.Services
{
    public interface IRivaIdentityApiClientService
    {
        Task<IAccount> GetAccountAsync(Guid accountId);
    }
}