using System.Threading.Tasks;

namespace Riva.Identity.Core.Interactors.Logout
{
    public interface ILoggedOutInteractor
    {

        Task<LoggedOutOutput> ExecuteAsync(string logoutId);
    }
}