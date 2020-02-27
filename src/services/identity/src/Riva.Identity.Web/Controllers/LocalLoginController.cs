using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Riva.Identity.Core.Interactors.LocalLogin;
using Riva.Identity.Web.ActionFilterAttributes;
using Riva.Identity.Web.ServiceCollectionExtensions;
using Riva.BuildingBlocks.Core.Models;
using Riva.Identity.Core.Enumerations;
using Riva.Identity.Web.Extensions;
using Riva.Identity.Web.Models.AppSettings;
using Riva.Identity.Web.Models.ErrorMessages;
using Riva.Identity.Web.Models.Requests;
using Riva.Identity.Web.Models.ViewModels;

namespace Riva.Identity.Web.Controllers
{
    [Route("auth")]
    [SecurityHeaders]
    [AllowAnonymous]
    public class LocalLoginController : Controller
    {
        private readonly ILocalLoginInteractor _localLoginInteractor;
        private readonly ApplicationUrlsAppSettings _applicationUrlsAppSettings;

        public LocalLoginController(ILocalLoginInteractor localLoginInteractor, IOptions<ApplicationUrlsAppSettings> applicationUrlsOptions)
        {
            _localLoginInteractor = localLoginInteractor;
            _applicationUrlsAppSettings = applicationUrlsOptions.Value;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var localLoginOutput = await _localLoginInteractor.ExecuteAsync(returnUrl);

            if (localLoginOutput.IsExternalLoginOnly)
                return RedirectToAction("Challenge", "ExternalLogin",
                    new {scheme = localLoginOutput.ExternalLoginScheme, returnUrl});

            var vm = BuildLoginViewModel(localLoginOutput, returnUrl);
            return View(vm);
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LocalLoginRequest request)
        {
            IEnumerable<string> errors;
            var localLoginResultOutput = await _localLoginInteractor.ExecuteAsync(request.Email, request.Password, request.RememberLogin, request.ReturnUrl);
            if (localLoginResultOutput.Success)
            {
                if (localLoginResultOutput.IsInTheContextOfAuthorizationRequest)
                {
                    if (localLoginResultOutput.IsNativeClient.HasValue && localLoginResultOutput.IsNativeClient.Value)
                        return this.LoadingPage("Redirect", request.ReturnUrl);
                    return Redirect(request.ReturnUrl);
                }

                if (Url.IsLocalUrl(request.ReturnUrl))
                    return Redirect(request.ReturnUrl);
                if (string.IsNullOrWhiteSpace(request.ReturnUrl))
                    return Redirect("~/");
                errors = new List<string>{ LocalLoginErrorMessage.InvalidReturnUrl };
            }
            else
                errors = MapToErrorMessages(localLoginResultOutput.Errors);

            var vm = await BuildLoginViewModelWithErrorsAsync(request, errors);
            return View(vm);
        }

        private LocalLoginViewModel BuildLoginViewModel(LocalLoginOutput localLoginOutput, string returnUrl)
        {
            var googleLoginEnabled =
                localLoginOutput.ExternalProviders.Any(x =>
                    x.AuthenticationScheme == AuthenticationExtension.GoogleAuthScheme);
            var facebookLoginEnabled =
                localLoginOutput.ExternalProviders.Any(x =>
                    x.AuthenticationScheme == AuthenticationExtension.FacebookAuthScheme);

            return new LocalLoginViewModel(localLoginOutput.LocalLoginEnabled, googleLoginEnabled, facebookLoginEnabled,
                _applicationUrlsAppSettings.RivaWebRegistrationUrl,
                _applicationUrlsAppSettings.RivaWebRequestRegistrationConfirmationEmailUrl,
                _applicationUrlsAppSettings.RivaWebRequestPasswordResetEmailUrl)
            {
                ReturnUrl = returnUrl
            };
        }

        private async Task<LocalLoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var localLoginOutput = await _localLoginInteractor.ExecuteAsync(returnUrl);
            var googleLoginEnabled =
                localLoginOutput.ExternalProviders.Any(x => x.AuthenticationScheme == AuthenticationExtension.GoogleAuthScheme);
            var facebookLoginEnabled =
                localLoginOutput.ExternalProviders.Any(x => x.AuthenticationScheme == AuthenticationExtension.FacebookAuthScheme);

            return new LocalLoginViewModel(localLoginOutput.LocalLoginEnabled, googleLoginEnabled, facebookLoginEnabled,
                _applicationUrlsAppSettings.RivaWebRegistrationUrl,
                _applicationUrlsAppSettings.RivaWebRequestRegistrationConfirmationEmailUrl,
                _applicationUrlsAppSettings.RivaWebRequestPasswordResetEmailUrl)
            {
                ReturnUrl = returnUrl
            };
        }

        private async Task<LocalLoginViewModel> BuildLoginViewModelWithErrorsAsync(LocalLoginRequest request, IEnumerable<string> errors)
        {
            var vm = await BuildLoginViewModelAsync(request.ReturnUrl);
            vm.SetErrors(errors);
            vm.Email = request.Email;
            vm.RememberLogin = request.RememberLogin;
            vm.Password = request.Password;
            return vm;
        }

        private static IEnumerable<string> MapToErrorMessages(IEnumerable<IError> errors)
        {
            return errors.Select(x =>
            {
                if (Equals(x.ErrorCode, AccountErrorCodeEnumeration.NotFound) ||
                    Equals(x.ErrorCode, AccountErrorCodeEnumeration.IncorrectPassword))
                    return LocalLoginErrorMessage.InvalidCredentials;
                return x.ErrorMessage;
            });
        }
    }
}