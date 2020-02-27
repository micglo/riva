using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Riva.Identity.Domain.PersistedGrants.Repositories;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions;
using Riva.BuildingBlocks.Core.Communications;
using Riva.BuildingBlocks.WebApi.Models.Environments;
using Riva.BuildingBlocks.WebApiTest.IntegrationTests;
using Riva.Identity.Core.Services;

namespace Riva.Identity.Web.Api.Test.IntegrationTestConfigs
{
    public class AdministratorWebApplicationFactory : WebApplicationFactory<AdministratorIntegrationTestStartup>
    {
        private IServiceScope _serviceScope;
        private readonly string _databaseName;

        public AdministratorWebApplicationFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public RivaIdentityDbContext DbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<RivaIdentityDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IIntegrationEventBus));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IPersistedGrantRepository));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IAccountDataConsistencyService));
                if (descriptor != null)
                    services.Remove(descriptor);

                services
                    .AddDbContext<RivaIdentityDbContext>((options, context) =>
                        context.UseInMemoryDatabase(_databaseName));

                var sp = services.BuildServiceProvider();
                _serviceScope = sp.CreateScope();
                var scopedServices = _serviceScope.ServiceProvider;
                DbContext = scopedServices.GetRequiredService<RivaIdentityDbContext>();
                DbContext.Database.EnsureCreated();
                DbContext.ClearDatabase();

                services.AddSingleton<IIntegrationEventBus, IntegrationEventBusStub>();
                services.AddScoped<IPersistedGrantRepository, PersistedGrantRepositoryStub>();
                services.AddScoped<IAccountDataConsistencyService, AccountDataConsistencyServiceStub>();
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

    public class UserWebApplicationFactory : WebApplicationFactory<AccountIntegrationTestStartup>
    {
        private IServiceScope _serviceScope;
        private readonly string _databaseName;

        public UserWebApplicationFactory(string databaseName)
        {
            _databaseName = databaseName;
        }

        public RivaIdentityDbContext DbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<RivaIdentityDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IIntegrationEventBus));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IPersistedGrantRepository));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IAccountDataConsistencyService));
                if (descriptor != null)
                    services.Remove(descriptor);

                services
                    .AddDbContext<RivaIdentityDbContext>((options, context) =>
                        context.UseInMemoryDatabase(_databaseName));

                var sp = services.BuildServiceProvider();
                _serviceScope = sp.CreateScope();
                var scopedServices = _serviceScope.ServiceProvider;
                DbContext = scopedServices.GetRequiredService<RivaIdentityDbContext>();
                DbContext.Database.EnsureCreated();
                DbContext.ClearDatabase();

                services.AddSingleton<IIntegrationEventBus, IntegrationEventBusStub>();
                services.AddScoped<IPersistedGrantRepository, PersistedGrantRepositoryStub>();
                services.AddScoped<IAccountDataConsistencyService, AccountDataConsistencyServiceStub>();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<AccountIntegrationTestStartup>();
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

        public RivaIdentityDbContext DbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<RivaIdentityDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IIntegrationEventBus));
                if (descriptor != null)
                    services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IPersistedGrantRepository));
                if (descriptor != null)
                    services.Remove(descriptor);

                services
                    .AddDbContext<RivaIdentityDbContext>((options, context) =>
                        context.UseInMemoryDatabase(_databaseName));

                var sp = services.BuildServiceProvider();
                _serviceScope = sp.CreateScope();
                var scopedServices = _serviceScope.ServiceProvider;
                DbContext = scopedServices.GetRequiredService<RivaIdentityDbContext>();
                DbContext.Database.EnsureCreated();
                DbContext.ClearDatabase();

                services.AddSingleton<IIntegrationEventBus, IntegrationEventBusStub>();
                services.AddScoped<IPersistedGrantRepository, PersistedGrantRepositoryStub>();
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