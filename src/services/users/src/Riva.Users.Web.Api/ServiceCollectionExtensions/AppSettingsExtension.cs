using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.Users.Infrastructure.Models.AppSettings;
using Riva.Users.Web.Api.Models.Constants;

namespace Riva.Users.Web.Api.ServiceCollectionExtensions
{
    public static class AppSettingsExtension
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<ApiClientsAppSettings>(configuration.GetSection(AppSettingsConstants.ApiClients));
        }
    }
}