using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class ClaimsPrincipalService : IClaimsPrincipalService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsPrincipalService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext.User;
        }

        public string GetNonLocalIdentityProvider(ClaimsPrincipal claimsPrincipal)
        {
            var idp = claimsPrincipal.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            return !string.IsNullOrWhiteSpace(idp) && !idp.Equals(IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                ? idp
                : null;
        }
    }
}