using Microsoft.AspNetCore.Http;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Core.Services;

namespace Riva.Users.Infrastructure.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAdministrator()
        {
            return _httpContextAccessor.HttpContext.User.IsInRole(DefaultRoleEnumeration.Administrator.DisplayName);
        }
    }
}