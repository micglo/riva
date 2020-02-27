using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace Riva.Identity.Web.ServiceCollectionExtensions
{
    public static class CookiePolicyOptionsExtension
    {
        public static IServiceCollection AddCookiePolicyOption(this IServiceCollection services, IWebHostEnvironment env)
        {
            return services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions, env);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions, env);
            });
        }

        private static void CheckSameSite(HttpContext httpContext, CookieOptions options, IHostEnvironment env)
        {
            if (!env.IsProduction() &&
                options.SameSite == Microsoft.AspNetCore.Http.SameSiteMode.None &&
                httpContext.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var stringValue))
                if (stringValue.Any(s => s.ToLower().Contains("postman")))
                    options.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Unspecified;
        }
    }
}