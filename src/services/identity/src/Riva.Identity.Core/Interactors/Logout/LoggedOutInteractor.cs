using System.Threading.Tasks;
using Riva.Identity.Core.Services;
using Riva.Identity.Domain.PersistedGrants.Repositories;

namespace Riva.Identity.Core.Interactors.Logout
{
    public class LoggedOutInteractor : ILoggedOutInteractor
    {
        private readonly ILogoutService _logoutService;
        private readonly IPersistedGrantRepository _persistedGrantRepository;
        private readonly IClaimsPrincipalService _claimsPrincipalService;
        private readonly ISignOutService _signOutService;
        private readonly ISchemeService _schemeService;

        public LoggedOutInteractor(ILogoutService logoutService, IPersistedGrantRepository persistedGrantRepository, 
            IClaimsPrincipalService claimsPrincipalService, ISignOutService signOutService, ISchemeService schemeService)
        {
            _logoutService = logoutService;
            _persistedGrantRepository = persistedGrantRepository;
            _claimsPrincipalService = claimsPrincipalService;
            _signOutService = signOutService;
            _schemeService = schemeService;
        }

        public async Task<LoggedOutOutput> ExecuteAsync(string logoutId)
        {
            var getLogoutRequestTask = _logoutService.GetLogoutRequestAsync(logoutId);
            var claimsPrincipal = _claimsPrincipalService.GetClaimsPrincipal();
            var logoutRequest = await getLogoutRequestTask;

            if (claimsPrincipal?.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                var signOutTask = _signOutService.SignOutAsync();
                
                if (logoutRequest.SubjectId.HasValue)
                    await _persistedGrantRepository.DeleteAllBySubjectIdAsync(logoutRequest.SubjectId.Value);

                var idp = _claimsPrincipalService.GetNonLocalIdentityProvider(claimsPrincipal);
                await signOutTask;

                if (!string.IsNullOrWhiteSpace(idp) && await _schemeService.SchemeSupportsSignOutAsync(idp))
                {
                    if (string.IsNullOrWhiteSpace(logoutId))
                        logoutId = await _logoutService.CreateLogoutContextAsync();

                    return new LoggedOutOutput(logoutId, logoutRequest.PostLogoutRedirectUri,
                        logoutRequest.SignOutIFrameUrl, logoutRequest.ClientId, idp);
                }
            }

            return new LoggedOutOutput(logoutId, logoutRequest?.PostLogoutRedirectUri,
                logoutRequest?.SignOutIFrameUrl, logoutRequest?.ClientId, null);
        }
    }
}