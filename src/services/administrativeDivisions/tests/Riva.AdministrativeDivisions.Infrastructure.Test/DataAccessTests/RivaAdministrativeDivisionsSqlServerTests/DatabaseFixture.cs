using System;
using Microsoft.EntityFrameworkCore;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Contexts;
using Riva.AdministrativeDivisions.Infrastructure.DataAccess.RivaAdministrativeDivisionsSqlServer.Extensions;

namespace Riva.AdministrativeDivisions.Infrastructure.Test.DataAccessTests.RivaAdministrativeDivisionsSqlServerTests
{
    public class DatabaseFixture : IDisposable
    {
        public RivaAdministrativeDivisionsDbContext DbContext { get; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<RivaAdministrativeDivisionsDbContext>()
                .UseInMemoryDatabase("RivaAdministrativeDivisionsDatabaseIntegrationTestsDb").Options;
            DbContext = new RivaAdministrativeDivisionsDbContext(options);
            DbContext.ClearDatabase();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                DbContext.Dispose();
        }
    }
}