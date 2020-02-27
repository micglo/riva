using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace Riva.Identity.Infrastructure.Services
{
    public class CookieOptionsService : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        public void Configure(CookieAuthenticationOptions options)
        {
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            options.AccessDeniedPath = "/error/403";
            options.LoginPath = "/login";
            options.ReturnUrlParameter = "returnUrl";
        }
    }
}