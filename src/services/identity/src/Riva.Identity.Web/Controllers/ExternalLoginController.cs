using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riva.Identity.Core.Interactors.ExternalLogin;
using Riva.Identity.Web.ActionFilterAttributes;
using Riva.Identity.Web.Extensions;
using Riva.Identity.Web.Models.ErrorMessages;

namespace Riva.Identity.Web.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalLoginController : Controller
    {
        private readonly IExternalLoginInteractor _externalLoginInteractor;

        public ExternalLoginController(IExternalLoginInteractor externalLoginInteractor)
        {
            _externalLoginInteractor = externalLoginInteractor;
        }

        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) 
                returnUrl = "~/";

            if (!Url.IsLocalUrl(returnUrl)) 
                return RedirectToAction("Index", "Error",
                new { message = LocalLoginErrorMessage.InvalidReturnUrl, statusCode = 422 });

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", provider }
                }
            };

            return Challenge(props, provider);
        }

        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            var output =
                await _externalLoginInteractor.ExecuteAsync(IdentityServer4.IdentityServerConstants
                    .ExternalCookieAuthenticationScheme);

            return output.IsNativeClient.HasValue && output.IsNativeClient.Value
                ? this.LoadingPage("Redirect", output.ReturnUrl)
                : Redirect(output.ReturnUrl);
        }
    }
}