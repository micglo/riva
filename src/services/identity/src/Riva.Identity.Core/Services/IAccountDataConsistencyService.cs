using System.Threading.Tasks;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Core.Services
{
    public interface IAccountDataConsistencyService
    {
        Task DeleteAccountWithRelatedPersistedGrants(Account account);
    }
}