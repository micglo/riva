using Xunit;

namespace Riva.Announcements.Infrastructure.Test.DataAccessTests.RivaAnnouncementsCosmosDbTests
{
    [CollectionDefinition("RivaAnnouncementsCosmosDb tests collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}