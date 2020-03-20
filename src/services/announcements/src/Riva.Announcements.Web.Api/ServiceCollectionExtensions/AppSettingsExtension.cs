using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Infrastructure.Models.AppSettings;
using Riva.Announcements.Web.Api.Models.Constants;

namespace Riva.Announcements.Web.Api.ServiceCollectionExtensions
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<ApiClientsAppSettings>(configuration.GetSection(AppSettingsConstants.ApiClients));
        }
    }
}