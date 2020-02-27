using System.Threading.Tasks;

namespace Riva.Identity.Core.Interactors.Logout
{
    public interface ILogoutInteractor
    {
        Task<LogoutOutput> ExecuteAsync(string logoutId);
    }
}