using System;
using Microsoft.EntityFrameworkCore;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Contexts;
using Riva.Identity.Infrastructure.DataAccess.RivaIdentitySqlServer.Extensions;

namespace Riva.Identity.Infrastructure.Test.DataAccessTests.RivaIdentitySqlServerTests
{
    public class DatabaseFixture : IDisposable
    {
        public RivaIdentityDbContext Context { get; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<RivaIdentityDbContext>()
                .UseInMemoryDatabase("RivaIdentityDatabaseIntegrationTestsDb").Options;
            Context = new RivaIdentityDbContext(options);
            Context.ClearDatabase();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                Context.Dispose();
        }
    }
}