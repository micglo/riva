using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Web.Api.Models.AppSettings;
using Riva.BuildingBlocks.WebApi.ConfigurationExtensions;
using Riva.BuildingBlocks.WebApi.HostEnvironmentExtensions;
using Riva.BuildingBlocks.WebApi.Models.Constants;

namespace Riva.AdministrativeDivisions.Web.Api.Configs
{
    public static class SqlServerConfigurator
    {
        public static Action<IServiceProvider, DbContextOptionsBuilder> Configure(IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
        {
            var connectionStringAppSettings = config.GetSectionAppSettings<ConnectionStringsAppSettings>(AppSettingsConstants.ConnectionStrings);
            services.AddScoped(s => new SqlConnection(connectionStringAppSettings.RivaAdministrativeDivisionsSQLServerDatabaseConnectionString));

            return (locator, builder) => {
                var connection = locator.GetRequiredService<SqlConnection>();

                if (env.IsLocalOrDockerOrDevelopment())
                    builder.EnableSensitiveDataLogging();

                builder.UseSqlServer(connection, sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(typeof(RivaAdministrativeDivisionsDbContext).Assembly.GetName().Name);
                    sqlServerOptions.EnableRetryOnFailure();
                });
            };
        }
    }
}