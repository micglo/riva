using System.Threading.Tasks;
using IdentityServer4.Services;
using Riva.Identity.Core.Services;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Models;

namespace Riva.Identity.Infrastructure.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IIdentityServerInteractionService _interaction; 
        private readonly IMapper _mapper;

        public AuthorizationService(IIdentityServerInteractionService interaction, IMapper mapper)
        {
            _interaction = interaction;
            _mapper = mapper;
        }

        public async Task<AuthorizationRequest> GetAuthorizationRequestAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            return context != null ? _mapper.Map<IdentityServer4.Models.AuthorizationRequest, AuthorizationRequest>(context) : null;
        }
    }
}