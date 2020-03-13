using Xunit;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests
{
    [CollectionDefinition("RivaAdministrativeDivisionsSqlServer tests collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
}