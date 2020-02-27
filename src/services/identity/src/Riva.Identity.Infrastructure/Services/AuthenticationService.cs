using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Riva.BuildingBlocks.Core.Mapper;

namespace Riva.Identity.Infrastructure.Services
{
    public class AuthenticationService : Core.Services.IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public AuthenticationService(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<Core.Models.AuthenticateResult> AuthenticateAsync(string scheme)
        {
            var authResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(scheme);
            return _mapper.Map<AuthenticateResult, Core.Models.AuthenticateResult>(authResult);
        }
    }
}