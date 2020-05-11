using System.Threading.Tasks;
using Microsoft.Azure.CosmosDB.BulkExecutor;

namespace Riva.AnnouncementPreferences.Core.Services
{
    public interface IBulkExecutorInitializer
    {
        Task<IBulkExecutor> InitializeBulkExecutorAsync();
    }
}