using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Riva.Identity.Core.Interactors.Logout;
using Riva.Identity.Web.ActionFilterAttributes;
using Riva.BuildingBlocks.Core.Mapper;
using Riva.Identity.Web.Models.ViewModels;

namespace Riva.Identity.Web.Controllers
{
    [SecurityHeaders]
    [Route("auth")]
    public class LogoutController : Controller
    {
        private readonly ILogoutInteractor _logoutInteractor;
        private readonly ILoggedOutInteractor _loggedOutInteractor;
        private readonly IMapper _mapper;

        public LogoutController(ILogoutInteractor logoutInteractor, ILoggedOutInteractor loggedOutInteractor, IMapper mapper)
        {
            _logoutInteractor = logoutInteractor;
            _loggedOutInteractor = loggedOutInteractor;
            _mapper = mapper;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logoutOutput = await _logoutInteractor.ExecuteAsync(logoutId);
            var logoutViewModel = _mapper.Map<LogoutOutput, LogoutViewModel>(logoutOutput);
            return logoutViewModel.ShowLogoutPrompt
                ? View(logoutViewModel)
                : await Logout(logoutViewModel.LogoutId, true.ToString());
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string logoutId, string processLogout)
        {
            if (string.Equals(processLogout, bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
            {
                var loggedOutOutput = await _loggedOutInteractor.ExecuteAsync(logoutId);
                var loggedOutViewModel = _mapper.Map<LoggedOutOutput, LoggedOutViewModel>(loggedOutOutput);
                if (loggedOutViewModel.TriggerExternalSignOut)
                {
                    var url = Url.Action("Logout", new { logoutId = loggedOutViewModel.LogoutId });
                    return SignOut(new AuthenticationProperties { RedirectUri = url }, loggedOutViewModel.ExternalAuthenticationScheme);
                }
                return View("LoggedOut", loggedOutViewModel);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}