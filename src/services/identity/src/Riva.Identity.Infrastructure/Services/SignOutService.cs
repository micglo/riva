using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class SignOutService : ISignOutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignOutService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task SignOutAsync()
        {
            return _httpContextAccessor.HttpContext.SignOutAsync();
        }

        public Task SignOutAsync(string scheme)
        {
            return _httpContextAccessor.HttpContext.SignOutAsync(scheme);
        }
    }
}