using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Web.Api.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.AdministrativeDivisions.Web.Api.ServiceCollectionExtensions
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration config)
        {
            var connectionStringAppSettings = config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings);

            return BuildingBlocks.WebApi.ServiceCollectionExtensions.HealthChecksExtension.AddHealthChecks(services)
                .AddSqlServer(connectionStringAppSettings.RivaAdministrativeDivisionsSQLServerDatabaseConnectionString,
                    name: "RivaAdministrativeDivisionsSQLServerDatabase")
                .Services;
        }
    }
}