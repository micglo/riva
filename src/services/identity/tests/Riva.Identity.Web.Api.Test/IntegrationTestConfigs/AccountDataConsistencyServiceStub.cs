using System.Threading.Tasks;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.Accounts.Aggregates;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
{
    public class AccountDataConsistencyServiceStub : IAccountDataConsistencyService
    {
        public Task DeleteAccountWithRelatedPersistedGrants(Account account)
        {
            return Task.CompletedTask;
        }
    }
}