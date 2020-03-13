using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Extensions;
using Riva.BuildingBlocks.WebApi.Models.Environments;

namespace Riva.AdministrativeDivisions.Web.Api.Test.IntegrationTestConfigs
{
    public class AdministratorWebApplicationFactory : WebApplicationFactory<AdministratorIntegrationTestStartup>
    {
        private IServiceScope _serviceScope;
        private readonly string _databaseName;

        public AdministratorWebApplicationFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public RivaAdministrativeDivisionsDbContext DbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<RivaAdministrativeDivisionsDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services
                    .AddDbContext<RivaAdministrativeDivisionsDbContext>((options, context) =>
                        context.UseInMemoryDatabase(_databaseName));

                var sp = services.BuildServiceProvider();
                _serviceScope = sp.CreateScope();
                var scopedServices = _serviceScope.ServiceProvider;
                DbContext = scopedServices.GetRequiredService<RivaAdministrativeDivisionsDbContext>();
                DbContext.Database.EnsureCreated();
                DbContext.ClearDatabase();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<AdministratorIntegrationTestStartup>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope?.Dispose();
                DbContext?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class UserWebApplicationFactory : WebApplicationFactory<UserIntegrationTestStartup>
    {
        private IServiceScope _serviceScope;
        private readonly string _databaseName;

        public UserWebApplicationFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public RivaAdministrativeDivisionsDbContext DbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<RivaAdministrativeDivisionsDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services
                    .AddDbContext<RivaAdministrativeDivisionsDbContext>((options, context) =>
                        context.UseInMemoryDatabase(_databaseName));

                var sp = services.BuildServiceProvider();
                _serviceScope = sp.CreateScope();
                var scopedServices = _serviceScope.ServiceProvider;
                DbContext = scopedServices.GetRequiredService<RivaAdministrativeDivisionsDbContext>();
                DbContext.Database.EnsureCreated();
                DbContext.ClearDatabase();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<UserIntegrationTestStartup>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope?.Dispose();
                DbContext?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class AnonymousWebApplicationFactory : WebApplicationFactory<AnonymousIntegrationTestStartup>
    {
        private IServiceScope _serviceScope;
        private readonly string _databaseName;

        public AnonymousWebApplicationFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public RivaAdministrativeDivisionsDbContext DbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<RivaAdministrativeDivisionsDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services
                    .AddDbContext<RivaAdministrativeDivisionsDbContext>((options, context) =>
                        context.UseInMemoryDatabase(_databaseName));

                var sp = services.BuildServiceProvider();
                _serviceScope = sp.CreateScope();
                var scopedServices = _serviceScope.ServiceProvider;
                DbContext = scopedServices.GetRequiredService<RivaAdministrativeDivisionsDbContext>();
                DbContext.Database.EnsureCreated();
                DbContext.ClearDatabase();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<AnonymousIntegrationTestStartup>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope?.Dispose();
                DbContext?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}