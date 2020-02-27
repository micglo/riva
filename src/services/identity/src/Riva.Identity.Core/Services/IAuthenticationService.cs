using System.Threading.Tasks;
using Riva.Identity.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResult> AuthenticateAsync(string scheme);
    }
}