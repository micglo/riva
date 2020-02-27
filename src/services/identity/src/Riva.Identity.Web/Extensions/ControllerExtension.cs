using Microsoft.AspNetCore.Mvc;
using Riva.Identity.Web.Models.ViewModels;

namespace Riva.Identity.Web.Extensions
{
    public static class ControllerExtension
    {
        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel(redirectUri));
        }
    }
}