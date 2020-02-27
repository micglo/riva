using System.Threading.Tasks;

namespace Riva.Identity.Core.Interactors.LocalLogin
{
    public interface ILocalLoginInteractor
    {
        Task<LocalLoginOutput> ExecuteAsync(string returnUrl);
        Task<LocalLoginResultOutput> ExecuteAsync(string email, string password, bool rememberLogin, string returnUrl);
    }
}