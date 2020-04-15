using Xunit;

namespace Riva.Users.Infrastructure.Test.DataAccessTests
{
    [CollectionDefinition("RivaUsersSqlServer tests collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
}