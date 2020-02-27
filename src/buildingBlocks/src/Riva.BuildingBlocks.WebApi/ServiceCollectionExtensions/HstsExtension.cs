using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Riva.BuildingBlocks.WebApi.ServiceCollectionExtensions
{
    public static class HstsExtension
    {
        public static IServiceCollection AddHsts(this IServiceCollection services)
        {
            return services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        }
    }
}