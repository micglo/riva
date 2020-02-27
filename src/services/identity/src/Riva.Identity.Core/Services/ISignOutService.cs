using System.Threading.Tasks;

namespace Riva.Identity.Core.Services
{
    public interface ISignOutService
    {
        Task SignOutAsync();
        Task SignOutAsync(string scheme);
    }
}