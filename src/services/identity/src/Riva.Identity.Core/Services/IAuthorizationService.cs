using System.Threading.Tasks;
using Riva.Identity.Core.Models;

namespace Riva.Identity.Core.Services
{
    public interface IAuthorizationService
    {
        Task<AuthorizationRequest> GetAuthorizationRequestAsync(string returnUrl);
    }
}