using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Infrastructure.DataAccess.RivaAnnouncementsCosmosDb.Entities;

namespace Riva.Announcements.Infrastructure.Test.DataAccessTests.RivaAnnouncementsCosmosDbTests
{
    public class DatabaseFixture
    {
        private const string EmulatorUri = "https://localhost:8081";
        private const string DatabaseId = "RivaAnnouncementsDatabaseIntegrationTestsDb";
        private readonly string _flatForRentAnnouncementRepositoryTestCollectionName = $"{nameof(FlatForRentAnnouncement)}Col";
        private readonly string _roomForRentAnnouncementRepositoryTestCollectionName = $"{nameof(RoomForRentAnnouncement)}Col";
        private const string EmulatorKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public ICosmosStore<FlatForRentAnnouncementEntity> FlatForRentAnnouncementCosmosStore { get; }
        public ICosmosStore<RoomForRentAnnouncementEntity> RoomForRentAnnouncementCosmosStore { get; }

        public DatabaseFixture()
        {
            var cosmonautClient = new CosmonautClient(EmulatorUri, EmulatorKey);
            var database = cosmonautClient.GetDatabaseAsync(DatabaseId).GetAwaiter().GetResult();
            if (database != null)
                cosmonautClient.DeleteDatabaseAsync(DatabaseId);
            var cosmosSettings = new CosmosStoreSettings(DatabaseId, EmulatorUri, EmulatorKey);
            var serviceCollection = new ServiceCollection()
                .AddCosmosStore<FlatForRentAnnouncementEntity>(cosmosSettings, _flatForRentAnnouncementRepositoryTestCollectionName)
                .AddCosmosStore<RoomForRentAnnouncementEntity>(cosmosSettings, _roomForRentAnnouncementRepositoryTestCollectionName);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            FlatForRentAnnouncementCosmosStore = serviceProvider.GetService<ICosmosStore<FlatForRentAnnouncementEntity>>();
            RoomForRentAnnouncementCosmosStore = serviceProvider.GetService<ICosmosStore<RoomForRentAnnouncementEntity>>();
        }
    }
}