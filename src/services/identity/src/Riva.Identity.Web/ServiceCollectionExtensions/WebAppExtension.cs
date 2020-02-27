using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Riva.BuildingBlocks.WebApi.Attributes.ActionFilters;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions;

namespace Riva.Identity.Web.ServiceCollectionExtensions
{
    public static class WebAppExtension
    {
        public static IServiceCollection AddWebApp(this IServiceCollection services, IWebHostEnvironment env)
        {
            return services
                .AddMvc(options =>
                {
                    options.Filters.Add(new ApiModelStateValidatorFilter());
                    options.RequireHttpsPermanent = env.IsNotLocalOrDocker();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .Services
                .AddHttpContextAccessor()
                .AddCookiePolicyOption(env)
                .AddHsts();
        }
    }
}