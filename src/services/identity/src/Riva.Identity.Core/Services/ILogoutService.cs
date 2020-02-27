using System.Threading.Tasks;
using Riva.Identity.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface ILogoutService
    {
        Task<LogoutRequest> GetLogoutRequestAsync(string logoutId);
        Task<string> CreateLogoutContextAsync();
    }
}