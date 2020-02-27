using System.Threading.Tasks;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Core.Interactors.Logout
{
    public class LogoutInteractor : ILogoutInteractor
    {
        private readonly ILogoutService _logoutService;
        private readonly IClaimsPrincipalService _claimsPrincipalService;

        public LogoutInteractor(ILogoutService logoutService, IClaimsPrincipalService claimsPrincipalService)
        {
            _logoutService = logoutService;
            _claimsPrincipalService = claimsPrincipalService;
        }

        public async Task<LogoutOutput> ExecuteAsync(string logoutId)
        {
            bool showLogoutPrompt;

            if (!IsAccountAuthenticated())
                showLogoutPrompt = false;
            else
            {
                var logoutRequest = await _logoutService.GetLogoutRequestAsync(logoutId);
                showLogoutPrompt = logoutRequest?.ShowSignOutPrompt ?? true;
            }

            return new LogoutOutput(logoutId, showLogoutPrompt);
        }

        private bool IsAccountAuthenticated()
        {
            var claimsPrincipal = _claimsPrincipalService.GetClaimsPrincipal();
            return claimsPrincipal?.Identity != null && claimsPrincipal.Identity.IsAuthenticated;
        }
    }
}