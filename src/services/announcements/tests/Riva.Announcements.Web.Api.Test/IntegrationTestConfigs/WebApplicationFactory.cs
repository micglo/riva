using System.Linq;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;
using Riva.BuildingBlocks.Core.Services;
using Riva.BuildingBlocks.WebApi.Models.Environments;

namespace Riva.Announcements.Web.Api.Test.IntegrationTestConfigs
{
    public class AdministratorWebApplicationFactory : WebApplicationFactory<AdministratorIntegrationTestStartup>
    {
        private const string DatabaseId = "RivaAnnouncementsWebApiIntegrationTestsDb";
        private const string EmulatorUri = "https://localhost:8081";
        private const string EmulatorKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public ICosmosStore<FlatForRentAnnouncementEntity> FlatForRentAnnouncementEntityCosmosStore { get; private set; }
        public ICosmosStore<RoomForRentAnnouncementEntity> RoomForRentAnnouncementEntityCosmosStore { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var cosmonautClient = new CosmonautClient(EmulatorUri, EmulatorKey);
                var database = cosmonautClient.GetDatabaseAsync(DatabaseId).GetAwaiter().GetResult();
                if(database != null)
                    cosmonautClient.DeleteDatabaseAsync(DatabaseId);

                var descriptors = services
                    .Where(x =>
                        x.ServiceType == typeof(ICosmosStore<FlatForRentAnnouncementEntity>) ||
                        x.ServiceType == typeof(ICosmosStore<RoomForRentAnnouncementEntity>) ||
                        x.ServiceType == typeof(IHttpClientService))
                    .ToList();
                if (descriptors.Any())
                {
                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }
                }

                var cosmosSettings = new CosmosStoreSettings(DatabaseId, EmulatorUri, EmulatorKey, settings =>
                {
                    settings.JsonSerializerSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Formatting = Formatting.Indented,
                        Converters = { new StringEnumConverter(), new IsoDateTimeConverter() }
                    };
                });
                services.AddCosmosStore<FlatForRentAnnouncementEntity>(cosmosSettings);
                services.AddCosmosStore<RoomForRentAnnouncementEntity>(cosmosSettings);
                services.AddScoped<IHttpClientService, HttpClientServiceStub>();

                var sp = services.BuildServiceProvider();
                FlatForRentAnnouncementEntityCosmosStore = sp.GetRequiredService<ICosmosStore<FlatForRentAnnouncementEntity>>();
                RoomForRentAnnouncementEntityCosmosStore = sp.GetRequiredService<ICosmosStore<RoomForRentAnnouncementEntity>>();
            });
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<AdministratorIntegrationTestStartup>();
        }
    }

    public class UserWebApplicationFactory : WebApplicationFactory<UserIntegrationTestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<UserIntegrationTestStartup>();
        }
    }

    public class AnonymousWebApplicationFactory : WebApplicationFactory<AnonymousIntegrationTestStartup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseEnvironment(Environment.LocalEnvironment)
                .UseStartup<AnonymousIntegrationTestStartup>();
        }
    }
}