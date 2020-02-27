using System.Security.Claims;

namespace Riva.Identity.Core.Services
{
    public interface IClaimsPrincipalService
    {
        ClaimsPrincipal GetClaimsPrincipal();
        string GetNonLocalIdentityProvider(ClaimsPrincipal claimsPrincipal);
    }
}