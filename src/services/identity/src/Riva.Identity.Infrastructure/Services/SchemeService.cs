using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class SchemeService : ISchemeService
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SchemeService(IAuthenticationSchemeProvider schemeProvider, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _schemeProvider = schemeProvider;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Core.Models.AuthenticationScheme>> GetAllSchemesAsync()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();
            return _mapper.Map<IEnumerable<AuthenticationScheme>, IEnumerable<Core.Models.AuthenticationScheme>>(schemes);
        }

        public async Task<Core.Models.AuthenticationScheme> GetSchemeAsync(string name)
        {
            var schema = await _schemeProvider.GetSchemeAsync(name);
            return schema != null ? _mapper.Map<AuthenticationScheme, Core.Models.AuthenticationScheme>(schema) : null;
        }

        public Task<bool> SchemeSupportsSignOutAsync(string scheme)
        {
            return _httpContextAccessor.HttpContext.GetSchemeSupportsSignOutAsync(scheme);
        }
    }
}