using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;

namespace Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.DesignTimeDbContextFactories
{
    public class RivaIdentityDbContextFactory : IDesignTimeDbContextFactory<RivaIdentityDbContext>
    {
        public RivaIdentityDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("RivaIdentitySQLServerDatabaseConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<RivaIdentityDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(typeof(RivaIdentityDbContext).Assembly.GetName().Name);
                sqlServerOptions.EnableRetryOnFailure();
            });

            return new RivaIdentityDbContext(optionsBuilder.Options);
        }
    }
}