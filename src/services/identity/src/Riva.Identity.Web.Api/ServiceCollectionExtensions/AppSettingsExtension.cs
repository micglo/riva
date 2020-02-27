using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.Identity.Infrastructure.Models.AppSettings;
using Riva.Identity.Web.Api.Models.Constants;

namespace Riva.Identity.Web.Api.ServiceCollectionExtensions
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<RivaWebApplicationSetting>(configuration.GetSection(AppSettingsConstants.ApplicationUrls));
        }
    }
}