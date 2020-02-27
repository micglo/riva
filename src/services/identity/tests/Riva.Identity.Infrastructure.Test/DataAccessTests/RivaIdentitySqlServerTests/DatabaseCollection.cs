using Xunit;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.RivaIdentitySqlServerTests
{
    [CollectionDefinition("RivaIdentitySqlServer tests collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
}