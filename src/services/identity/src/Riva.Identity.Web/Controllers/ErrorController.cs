using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Riva.Identity.Web.Models.ViewModels;

namespace Riva.Identity.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ErrorController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index(string message = null, int? statusCode = null, string errorId = null)
        {
            message ??= "Unexpected error has occured. Please contact with Administrator.";
            statusCode ??= 500;

            if (!string.IsNullOrWhiteSpace(errorId))
            {
                var identityServerError = await _interaction.GetErrorContextAsync(errorId);
                message = $"{identityServerError.ErrorDescription}";
                statusCode = 422;
            }

            return View("Error", new ErrorViewModel(statusCode.Value, message));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("error/{statusCode}")]
        public IActionResult StatusCodeError(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (feature != null)
            {
                var originalRequestUrl = $"{feature.OriginalPathBase}{feature.OriginalPath}{feature.OriginalQueryString}";

                if (statusCode == 401)
                    return RedirectToAction("Login", "LocalLogin", new { returnUrl = originalRequestUrl });
            }

            return View("Error", new ErrorViewModel(statusCode, GetStatusCodeErrorMessage(statusCode)));
        }

        private static string GetStatusCodeErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                403 => "You are not authorized to perform this operation.",
                404 => "Page was not found.",
                _ => "Unexpected error has occured. Please contact with Administrator."
            };
        }
    }
}