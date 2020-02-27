using System.Threading.Tasks;
using IdentityServer4.Services;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Core.Models;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Infrastructure.Services
{
    public class LogoutService : ILogoutService
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IMapper _mapper;

        public LogoutService(IIdentityServerInteractionService interaction, IMapper mapper)
        {
            _interaction = interaction;
            _mapper = mapper;
        }

        public async Task<LogoutRequest> GetLogoutRequestAsync(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            return context != null ? _mapper.Map<IdentityServer4.Models.LogoutRequest, LogoutRequest>(context) : null;
        }

        public Task<string> CreateLogoutContextAsync()
        {
            return _interaction.CreateLogoutContextAsync();
        }
    }
}