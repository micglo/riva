using System.Threading.Tasks;

namespace Riva.Identity.Core.Interactors.ExternalLogin
{
    public interface IExternalLoginInteractor
    {
        Task<ExternalLoginResultOutput> ExecuteAsync(string scheme);
    }
}